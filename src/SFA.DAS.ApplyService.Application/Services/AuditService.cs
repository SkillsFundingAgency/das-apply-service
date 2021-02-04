using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IStateService _stateService;
        private readonly IDiffService _diffService;
        private readonly IAuditRepository _auditRepository;
        private readonly List<TrackedItem> _trackedItems;
        private UserAction _userAction;
        private string _userId;
        private string _userName;

        public AuditService(IStateService stateService, IDiffService diffService, IAuditRepository auditRepository)
        {
            _stateService = stateService;
            _diffService = diffService;
            _auditRepository = auditRepository;
            _trackedItems = new List<TrackedItem>();
        }

        public void StartTracking(UserAction userAction, string userId, string userName)
        {
            _userAction = userAction;
            _userId = userId;
            _userName = userName;
        }

        public IReadOnlyList<TrackedItem> TrackedItems => _trackedItems.AsReadOnly();

        public void AuditInsert(IAuditable trackedObject)
        {
            _trackedItems.Add(TrackedItem.CreateInsertTrackedItem(trackedObject));
        }

        public void AuditUpdate(IAuditable trackedObject)
        {
            var initialState = _stateService.GetState(trackedObject);
            _trackedItems.Add(TrackedItem.CreateUpdateTrackedItem(trackedObject, initialState));
        }

        public void AuditDelete(IAuditable trackedObject)
        {
            var initialState = _stateService.GetState(trackedObject);
            _trackedItems.Add(TrackedItem.CreateDeleteTrackedItem(trackedObject, initialState));
        }

        public async Task Save()
        {
            var correlationId = Guid.NewGuid();
            var auditDate = DateTime.UtcNow;

            foreach (var item in _trackedItems)
            {
                var updated = item.Operation == AuditOperation.Delete ? null : _stateService.GetState(item.TrackedEntity);

                var initialState = item.InitialState == null ? null : JsonConvert.SerializeObject(item.InitialState);
                var updatedState = updated == null ? null : JsonConvert.SerializeObject(updated);

                var diff = _diffService.GenerateDiff(item.InitialState, updated);

                var audit = new Audit
                {
                    Id = Guid.NewGuid(),
                    EntityType = item.TrackedEntity.GetType().Name,
                    EntityId = item.TrackedEntity.Id,
                    UserId = _userId,
                    UserName = _userName,
                    UserAction = _userAction.ToString(),
                    AuditDate = auditDate,
                    InitialState = initialState,
                    UpdatedState = updatedState,
                    Diff = JsonConvert.SerializeObject(diff),
                    CorrelationId = correlationId
                };

                await _auditRepository.Add(audit);
            }
        }
    }
}

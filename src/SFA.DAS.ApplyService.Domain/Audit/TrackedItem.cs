using System.Collections.Generic;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Domain.Audit
{
    public class TrackedItem
    {
        public Dictionary<string, object> InitialState { get; private set; }
        public IAuditable TrackedEntity { get; private set; }
        public AuditOperation Operation { get; private set; }

        public static TrackedItem CreateInsertTrackedItem(IAuditable trackedEntity)
        {
            return new TrackedItem
            {
                TrackedEntity = trackedEntity,
                Operation = AuditOperation.Insert
            };
        }

        public static TrackedItem CreateDeleteTrackedItem(IAuditable trackedEntity, Dictionary<string, object> initialState)
        {
            return new TrackedItem
            {
                TrackedEntity = trackedEntity,
                InitialState = initialState,
                Operation = AuditOperation.Delete
            };
        }

        public static TrackedItem CreateUpdateTrackedItem(IAuditable trackedEntity, Dictionary<string, object> initialState)
        {
            return new TrackedItem
            {
                TrackedEntity = trackedEntity,
                InitialState = initialState,
                Operation = AuditOperation.Update
            };
        }
    }
}

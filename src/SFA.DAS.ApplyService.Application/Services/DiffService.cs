using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.Services
{
    public class DiffService : IDiffService
    {
        public IReadOnlyList<DiffItem> GenerateDiff(Dictionary<string, object> initial, Dictionary<string, object> updated)
        {
            var result = new List<DiffItem>();

            if (initial == null && updated == null)
            {
                return new List<DiffItem>();
            }

            if (initial == null)
            {
                return GenerateDiffForInsert(updated);
            }

            return GenerateDiffForUpdateOrDelete(initial, updated);
        }

        private IReadOnlyList<DiffItem> GenerateDiffForInsert(Dictionary<string, object> updated)
        {
            var result = new List<DiffItem>();

            foreach (var item in updated.Where(x => x.Value != null))
            {
                result.Add(new DiffItem
                {
                    PropertyName = item.Key,
                    InitialValue = null,
                    UpdatedValue = item.Value
                });
            }

            return result;
        }

        private IReadOnlyList<DiffItem> GenerateDiffForUpdateOrDelete(Dictionary<string, object> initial,
            Dictionary<string, object> updated)
        {
            var result = new List<DiffItem>();

            foreach (var item in initial)
            {
                var initialValue = item.Value;
                var updatedValue = updated == null ? null : updated.ContainsKey(item.Key) ? updated[item.Key] : null;

                if (initialValue == null)
                {
                    if (updatedValue != null)
                    {
                        result.Add(new DiffItem
                        {
                            PropertyName = item.Key,
                            InitialValue = null,
                            UpdatedValue = updatedValue
                        });
                    }
                    continue;
                }

                if (updatedValue == null)
                {
                    result.Add(new DiffItem
                    {
                        PropertyName = item.Key,
                        InitialValue = initialValue,
                        UpdatedValue = null
                    });
                    continue;
                }

                if (!initialValue.Equals(updatedValue))
                {
                    result.Add(new DiffItem
                    {
                        PropertyName = item.Key,
                        InitialValue = initialValue,
                        UpdatedValue = updatedValue
                    });
                }
            }

            return result;
        }
    }
}

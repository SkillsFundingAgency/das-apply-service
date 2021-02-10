using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Extensions
{
    [Serializable]
    public class SerializableModelStateDictionary
    {
        public ICollection<SerializableModelState> Data { get; set; } = new List<SerializableModelState>();
    }

    [Serializable]
    public class SerializableModelState
    {
        public string AttemptedValue { get; set; }
        public string CultureName { get; set; }
        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
        public string Key { get; set; }
        public object RawValue { get; set; }
    }
}

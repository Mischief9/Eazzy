using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    [Serializable]
    public class FailedResponse
    {
        public FailedResponse()
        {
            Errors = new List<string>();
        }

        public IList<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

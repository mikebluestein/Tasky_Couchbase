using System;
using System.Collections.Generic;

namespace Tasky.Core
{
    public class Task
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public bool Done { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var d = new Dictionary<string, object>
            {
                {"name", Name},
                {"notes", Notes},
                {"done", Done},
            };

            return d;
        }
    }
}
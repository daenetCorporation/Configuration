using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration.StronglyTyped.Test
{
    /// <summary>
    /// This is custom type used for testing
    /// </summary>
    public class MySettings
    {
        public int SomeInt { get; set; }

        public string SomeString { get; set; }

        public float SomeFloat { get; set; }

        public string SomeGuid { get;  set; }
    }
}

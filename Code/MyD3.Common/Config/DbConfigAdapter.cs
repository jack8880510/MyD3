using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Config
{
    public class DbConfigAdapter : IMultipleConfigAdapter
    {
        public string[] AllKeys
        {
            get
            {
                return new string[0];
            }
        }

        public long ConfigVersion
        {
            get
            {
                return 0;
            }
        }

        public string Name
        {
            get
            {
                return "dbConfigAdapter";
            }
        }

        public bool UseCache
        {
            get
            {
                return true;
            }
        }

        public IDictionary<string, string> All()
        {
            return new Dictionary<string, string>();
        }

        public string Get(string key)
        {
            return null;
        }

        public bool Set(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}

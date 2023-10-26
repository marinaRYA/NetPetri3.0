using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPetri3._0
{
    internal class Data
    {                           

        public string mark { get; set; } 
        public string path { get; set; }
        public Data(string m, string p) 
        {
            this.mark = m;
            this.path = p;
        }
    }
}

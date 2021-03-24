using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.RequestClasses
{
    public class GenericMultiple<T> where T: new()
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}

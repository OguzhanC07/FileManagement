using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.RequestClasses
{
    public class GenericData<T> where T : class, new()
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

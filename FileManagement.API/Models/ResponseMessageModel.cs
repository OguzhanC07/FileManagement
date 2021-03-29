using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.Models
{
    public class ResponseMessageModel<T>
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.Models
{
    public class MultipleDataResponseMessageModel<T> where T : class,new()
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}

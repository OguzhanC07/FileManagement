using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.RequestClasses
{
    public class ErrorResponse
    {
        public bool Result { get; set; }
        public List<string> Message { get; set; }
    }
}

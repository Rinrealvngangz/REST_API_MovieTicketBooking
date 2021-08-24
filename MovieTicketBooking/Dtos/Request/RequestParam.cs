using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Request
{
    public abstract class RequestParam
    {
        public string Fields { get; set; }

        public string Search { get; set; }
    }

    public class MovieParameters : RequestParam
    {
         public string MinDate { get; set; }

        public string MaxDate { get; set; }

      
    }
}

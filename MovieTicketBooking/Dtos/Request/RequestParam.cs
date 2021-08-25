using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Request
{
    public abstract class RequestParam
    {
        const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get {  return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize :value ; } 
        }

        public string Fields { get; set; }

        public string Search { get; set; }

        public string Sort { get;set; }
    }

    public class MovieParameters : RequestParam
    {
        public MovieParameters()
        {
            Sort = "name";
        }
         public string MinDate { get; set; }

        public string MaxDate { get; set; }

      
    }
}

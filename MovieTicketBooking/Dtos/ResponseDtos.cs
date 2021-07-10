using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
  public  class ResponseDtos
    {
        public bool Success { get; set; }

        public List<string> Erros { get; set; }
    }
}

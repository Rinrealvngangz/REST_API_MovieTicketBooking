using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class VerifyEmailDtos
    {
        public Guid UserId { get; set; }

        public string Code { get; set; }
    }
}

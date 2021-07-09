using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
namespace Utilities.Extension
{
   public static class ExtensionError
    {
        public static string AsToDescription( this IEnumerable<IdentityError> erros)
        {
            var errosString = "\n";
              foreach(var er in erros.ToList())
            {
                errosString += er.Description+"\n";
            }
            return errosString;
        }
    }
}

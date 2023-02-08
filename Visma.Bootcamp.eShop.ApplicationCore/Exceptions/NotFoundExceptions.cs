using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visma.Bootcamp.eShop.ApplicationCore.Exceptions
{
    public class NotFoundExceptions : Exception
    {
        private static readonly string _format = "{0} with ID {1} not found";

        public NotFoundExceptions(Type type, Guid id) 
            :base(string.Format(_format,type.Name,id))
        {

        }
    }
}
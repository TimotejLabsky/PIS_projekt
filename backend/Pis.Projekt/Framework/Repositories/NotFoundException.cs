using System;

namespace Pis.Projekt.Framework.Repositories
{
    public class NotFoundException
        : Exception
    {
        public NotFoundException(string message) 
            : base(message)
        {
            // empty
        }
    }
}
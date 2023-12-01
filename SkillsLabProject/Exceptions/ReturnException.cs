using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsLabProject.Exceptions
{
    public class ReturnException : Exception
    {
        public ReturnException(string message) : base(message) { }

    }
}
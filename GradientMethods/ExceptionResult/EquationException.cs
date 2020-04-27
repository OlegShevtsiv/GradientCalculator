using System;
using System.Collections.Generic;
using System.Text;

namespace GradientMethods.ExceptionResult
{
    public class EquationException : Exception
    {
        public string LocalizationString;
        public EquationException(string? message, string localizationString = null) : base(message)
        {
            this.LocalizationString = localizationString;
        }
    }
}
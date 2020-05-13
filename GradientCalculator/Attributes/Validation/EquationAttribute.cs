using GradientMethods;
using GradientMethods.ExceptionResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Attributes.Validation
{
    public class EquationAttribute : ValidationAttribute
    {
        public EquationAttribute() : base()
        {

        }

        public override bool IsValid(object value)
        {
            try 
            {
                Equation eq = new Equation(value as string);

                Random valueGenerator = new Random();

                _ = eq[eq.VariablesValues.Select(v => new KeyValuePair<int, double>(key: v.Index, valueGenerator.NextDouble())).ToList()];

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}

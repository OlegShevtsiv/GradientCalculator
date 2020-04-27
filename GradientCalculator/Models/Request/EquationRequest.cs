using GradientCalculator.Attributes.Validation;
using GradientMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Models.Request
{
    public class EquationRequest
    {
        [Required(ErrorMessage = "this_is_required_field")]
        //[StringLength(10, MinimumLength = 3, ErrorMessage = "Equation has to contain more than 3 symbols and less than 10 symbols!")]
        [Display(Name = "Equation")]
        [Equation(ErrorMessage = "equation_is_not_valid")]
        public string Equation { get; set; }


        [Required(ErrorMessage = "this_is_required_field")]
        public Dictionary<char, double?> ValuesOfVariables { get; set; }


        [Required(ErrorMessage = "this_is_required_field")]
        [Display(Name = "Accuracy")]
        public string Accuracy { get; set; }


        public EquationRequest()
        {
            this.ValuesOfVariables = new Dictionary<char, double?>();
        }
    }
}

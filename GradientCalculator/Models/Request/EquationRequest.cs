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
        [Required]
        [Display(Name = "Equation")]
        //[Equation(ErrorMessage = "equation_is_not_valid")]
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

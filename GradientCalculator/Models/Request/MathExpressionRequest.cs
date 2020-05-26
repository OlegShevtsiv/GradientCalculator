using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Models.Request
{
    public class MathExpressionRequest
    {
        [Required]
        [Display(Name = "Function")]
        public string Equation { get; set; }


        [Required(ErrorMessage = "this_is_required_field")]
        public Dictionary<int, double?> ValuesOfVariables { get; set; }


        public MathExpressionRequest()
        {
            this.Equation = string.Empty;
            this.ValuesOfVariables = new Dictionary<int, double?>();
        }
    }
}

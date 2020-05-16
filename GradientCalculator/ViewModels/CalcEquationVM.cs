using GradientCalculator.Models.Request;
using GradientCalculator.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.ViewModels
{
    public class CalcEquationVM
    {
        public EquationRequest PostModel { get; private set; }

        public EquationCalcResponse CalculationResultModel { get; set; }

        /// <summary>
        /// Sets default values to properties
        /// </summary>
        public CalcEquationVM()
        {
            this.PostModel = new EquationRequest();
            this.CalculationResultModel = default;
        }

        public CalcEquationVM(EquationRequest postModel)
        {
            this.PostModel = postModel;
        }

        public CalcEquationVM(EquationRequest postModel, EquationCalcResponse calculationResultModel)
        {
            this.PostModel = postModel;
            this.CalculationResultModel = calculationResultModel;
        }
    }
}

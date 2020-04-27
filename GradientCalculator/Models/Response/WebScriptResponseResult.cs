using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Models.Response
{
    public class WebScriptResponseResult
    {

        private object data;
        /// <summary>
        /// Response when result is Ok
        /// </summary>
        public object Data 
        {
            get 
            {
                if (!this.IsError)
                {
                    return this.data;
                }
                else return null;
            }
            set 
            {
                this.data = value;
            }
        }

        public string Note { get; set; }

        public bool IsError { get; set; } = false;

        private string errorMessage;
        public string ErrorMessage 
        {
            get 
            {
                if (this.IsError)
                {
                    return this.errorMessage;
                }
                else return null;
            }
            set 
            {
                this.errorMessage = value;
            }
        }

        public WebScriptResponseResult()
        {

        }

        /// <summary>
        /// Ok response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="note"></param>
        public WebScriptResponseResult(object data, string note = "")
        {
             this.Data = data;
             this.Note = note;
        }


        /// <summary>
        /// Error response
        /// </summary>
        /// <param name="errorMessage"></param>
        public WebScriptResponseResult(string errorMessage)
        {
            this.IsError = true;
            this.ErrorMessage = errorMessage;
        }
    }
}

var input =  document.getElementById('gradientMethod_equation_input_id');

input.oninput = function () {
    $.post(`/api/Methods/GetVariables?eq=${encodeURIComponent(input.value)}`,
        function (response) {
            if (response) {
                if (response.data) {

                    let errorMessage = document.getElementById('input_equation_error_message');
                    errorMessage.innerText = '';

                    let valOfVarsList = document.getElementById('values_of_variables_list_id');

                    let isExist = false;
                    for (let i = 0; i < valOfVarsList.childElementCount; i++) {
                        isExist = false;
                        for (let j = 0; j < response.data.length; j++) {
                            if (valOfVarsList.childNodes[i].id == `variable_element_id_${response.data[j].index}`) {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist) {
                            valOfVarsList.childNodes[i].remove();
                        }
                    }

                    for (let i = 0; i < response.data.length; i++) {
                        if (!document.getElementById(`variable_element_id_${response.data[i].index}`)) {
                            valOfVarsList.insertAdjacentHTML('beforeend',
                                `<div id="variable_element_id_${response.data[i].index}">
                                        <label>${response.data[i].name} :</label><br>
                                        <input type = "number" oninput="" data-val="true" data-val-number="The field must be a number." id="ValuesOfVariables_${response.data[i].index}_" name="ValuesOfVariables[${response.data[i].index}]" value="" required>
                                        <span style = "color:darkred" class="field-validation-valid" data-valmsg-for="ValuesOfVariables[${response.data[i].index}]" data-valmsg-replace="true"></span>
                                    </div>`)
                        }
                    }

                    let formSubmitButton = document.getElementById('equation_form_submit_button_id');
                    formSubmitButton.setAttribute('type', 'submit');
                }
                else if (response.isError)
                {
                    let formSubmitButton = document.getElementById('equation_form_submit_button_id');
                    formSubmitButton.setAttribute('type', 'button');

                    if (response.errorMessage)
                    {
                        let errorMessage = document.getElementById('input_equation_error_message');
                        errorMessage.innerText = response.errorMessage;
                    }




                    //let valOfVarsList = document.getElementById('values_of_variables_list_id');

                    //if (valOfVarsList && valOfVarsList.childNodes.length > 0)
                    //{
                    //    let children = valOfVarsList.childNodes;
                    //    for (let i = children.length - 1; i >= 0; i--)
                    //    {
                    //        children[i].remove();
                    //    }
                    //}
                }
            }
     });
};
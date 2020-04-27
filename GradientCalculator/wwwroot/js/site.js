var input = document.getElementById('gradientMethod_equation_input_id');

input.oninput = function () {
    $.post(`/api/Methods/GetVariables?eq=${input.value}`,
        function (response) {
            if (response) {
                if (response.data) {
                    console.log(response);
                    let valOfVarsList = document.getElementById('values_of_variables_list_id');

                    let isExist = false;
                    for (let i = 0; i < valOfVarsList.childElementCount; i++) {
                        isExist = false;
                        for (let j = 0; j < response.data.length; j++) {
                            if (valOfVarsList.childNodes[i].id == `variable_element_id_${response.data[j].Value}`) {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist) {
                            valOfVarsList.childNodes[i].remove();
                        }
                    }

                    for (let i = 0; i < response.data.length; i++) {
                        if (!document.getElementById(`variable_element_id_${response.data[i].Value}`)) {
                            valOfVarsList.insertAdjacentHTML('beforeend',
                                `<div id="variable_element_id_${response.data[i].Value}">
                                        <label>x${response.data[i].Key} :</label><br>
                                        <input type = "number" oninput="" data-val="true" data-val-number="The field must be a number." id="ValuesOfVariables_${response.data[i].Value}_" name="ValuesOfVariables[${response.data[i].Value}]" value="" required>
                                        <span style = "color:darkred" class="field-validation-valid" data-valmsg-for="ValuesOfVariables[${response.data[i].Value}]" data-valmsg-replace="true"></span>
                                    </div>`)
                        }
                    }
                }
                else {
                    let valOfVarsList = document.getElementById('values_of_variables_list_id');

                    if (valOfVarsList && valOfVarsList.childNodes.length > 0)
                    {
                        let children = valOfVarsList.childNodes;
                        for (let i = children.length - 1; i >= 0; i--)
                        {
                            children[i].remove();
                        }
                    }
                }
            }
     });
};
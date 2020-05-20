using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GradientMethods.ExceptionResult
{
    public class LocalizedException : Exception
    {
        private string localizationMessageKey;

        private readonly CultureInfo DefaultCulture = new CultureInfo("en");

        /// <summary>
        /// Create new instance of object with localized messeges by key, existed in resource collection otherwise creates default Exception
        /// </summary>
        /// <param name="key"></param>
        public LocalizedException(string key) : base()
        {
            if (!string.IsNullOrEmpty(key))
            {
                this.localizationMessageKey = key;
            }
        }

        /// <summary>
        /// Create new instance of object with localized messeges
        /// </summary>
        /// <param name="localizedMessage"></param>
        public LocalizedException(KeyValuePair<string, Dictionary<CultureInfo, string>> localizedMessage) : base()
        {
            if (!localizedMessage.Equals(default(KeyValuePair<string, Dictionary<CultureInfo, string>>))
             && !string.IsNullOrEmpty(localizedMessage.Key)
             && localizedMessage.Value?.Count > 0)
            {
                ExceptionCultureInfo lang;
                this.localizationMessageKey = localizedMessage.Key;
                foreach (var message in localizedMessage.Value)
                {
                    lang = new ExceptionCultureInfo(message.Key);
                    if (this.LocalizedStringsBase.ContainsKey(lang))
                    {
                        if (this.LocalizedStringsBase[lang].ContainsKey(localizedMessage.Key))
                        {
                            if (this.LocalizedStringsBase[lang][localizedMessage.Key] != message.Value) 
                            {
                                this.LocalizedStringsBase[lang][localizedMessage.Key] = message.Value;
                            }
                        }
                        else 
                        {
                            this.LocalizedStringsBase[lang].Add(localizedMessage.Key, message.Value);
                        }
                    }
                    else 
                    {
                        this.LocalizedStringsBase.Add(lang, new Dictionary<string, string>{ { localizedMessage.Key, message.Value } });
                    }
                }
            }
        }

        public string GetLocalizedMessage(CultureInfo cultureInfo)
        {
            var lang = new ExceptionCultureInfo(cultureInfo);
            if (lang != null && !string.IsNullOrEmpty(this.localizationMessageKey))
            {

                if (this.LocalizedStringsBase.ContainsKey(lang))
                {
                    if (this.LocalizedStringsBase[lang].ContainsKey(this.localizationMessageKey))
                    {
                        return this.LocalizedStringsBase[lang][this.localizationMessageKey];
                    }
                }
                else if (this.LocalizedStringsBase[new ExceptionCultureInfo(this.DefaultCulture)].ContainsKey(this.localizationMessageKey)) 
                {
                    return this.LocalizedStringsBase[new ExceptionCultureInfo(this.DefaultCulture)][this.localizationMessageKey];
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private Dictionary<ExceptionCultureInfo, Dictionary<string, string>> LocalizedStringsBase = new Dictionary<ExceptionCultureInfo, Dictionary<string, string>> 
        {
            { 
                new ExceptionCultureInfo(new CultureInfo("en")), 
                new Dictionary<string, string> 
                {
                    { "equation_doesnt_contain_any_variables", "Equation doesn't contain any variables." },
                    { "equation_is_empthy", "Equation is empthy!" },
                    { "equation_is_not_valid", "Equation is not valid!" },
                    { "equation_not_valid_amounts_of_)_and_(_are_not_equal", "Equation is not valid! Amounts of ')' and '(' are not equal!" },
                    { "equation_not_valid_it_contains_not_allowed_cyrillic_symbols", "Equation is not valid! It contains not allowed cyrillic symbols!" },
                    { "equation_not_valid_it_has_to_contains_variables_matching_pattern_X_(_number_0__9_)_", "Equation is not valid! It has to contains variables matching pattern 'X(number=0..9)'" },
                    { "error_braket_missing", "Error!!! Braket(s) missing!" },
                    { "error_creating_value_of_variable", "Error creating value of variable!" },
                    { "error_geting_constant", "Error getting constant!" },
                    { "incorect_input_list_of_variable_values", "Incorrect input list of variable values!" },
                    { "calculation_error", "Сalculation error!" },
                    { "constant_not_found", "Сonstant not found!" },
                    { "dividing_by_zero", "Error! Dividing by zero is not allowed!" },
                    { "extremum_not_found", "Extremum not found." },
                    { "matrix_has_to_be_square", "Matrix has to be square!" }

                }
            },

            {
                new ExceptionCultureInfo(new CultureInfo("uk")),
                new Dictionary<string, string>
                {
                    { "equation_doesnt_contain_any_variables", "Рівняння не містить змінних." },
                    { "equation_is_empthy", "Рівняння порожнє!" },
                    { "equation_is_not_valid", "Рівняння не коректне!" },
                    { "equation_not_valid_amounts_of_)_and_(_are_not_equal", "Рівняння некоректне! Кількість ')' та '(' не однакові!" },
                    { "equation_not_valid_it_contains_not_allowed_cyrillic_symbols", "Рівняння некоректне! Кириличні символи заборонені!" },
                    { "equation_not_valid_it_has_to_contains_variables_matching_pattern_X_(_number_0__9_)_", "Рівняння некоректне! Воно повинне містити змінні, що відповідають шаблону 'X{numer=0..9}'!" },
                    { "error_braket_missing", "Помилка ! Недостатньо дужок!" },
                    { "error_creating_value_of_variable", "Помилка створення значення змінної!" },
                    { "error_geting_constant", "Помилка отримання константи!" },
                    { "incorect_input_list_of_variable_values", "Вхідні значення змінних некоректні!" },
                    { "calculation_error", "Помилка обчислень!" },
                    { "constant_not_found", "Константа не знайдена!" },
                    { "dividing_by_zero", "Помилка! Ділення на нуль не дозволене!" },
                    { "extremum_not_found", "Точка екстремуму не знайдена." },
                    { "matrix_has_to_be_square", "Матриця повинна бути квадратна!" }

                }
            }
        };
    }
}
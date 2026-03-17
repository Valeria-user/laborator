using System;
using System.Globalization;
using System.Windows;

namespace laba_1
{
    /// <summary>
    /// Логика вычислений калькулятора
    /// </summary>
    public class CalculatorEngine
    {
        private string currentInput = "";
        private string previousInput = "";
        private string currentOperation = "";
        private bool isNewInput = true;
        private bool operationPerformed = false;

        public string CurrentInput => currentInput;

        /// <summary>
        /// Обработка ввода цифр и десятичной точки
        /// </summary>
        public void ProcessDigitOrPoint(string content)
        {
            string decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            if (content == ".")
                content = decimalSeparator;

            if (isNewInput)
            {
                if (content == "0")
                    currentInput = "0";
                else if (content == decimalSeparator)
                    currentInput = "0" + decimalSeparator;
                else
                    currentInput = content;

                isNewInput = false;
            }
            else
            {
                if (currentInput == "0" && content == "0")
                    return;
                else if (currentInput == "0" && content != decimalSeparator && content != "0")
                    currentInput = content;
                else if (content == decimalSeparator && !currentInput.Contains(decimalSeparator))
                    currentInput += content;
                else if (content != decimalSeparator && currentInput.Length < 15)
                    currentInput += content;
            }
        }

        /// <summary>
        /// Обработка математических операций
        /// </summary>
        public void ProcessOperation(string content)
        {
            if (content == "=")
            {
                CalculateResult();
                currentOperation = "";
                isNewInput = true;
                operationPerformed = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(currentInput) && !operationPerformed)
                {
                    if (!string.IsNullOrEmpty(previousInput))
                        CalculateResult();

                    previousInput = currentInput;
                    currentOperation = ConvertOperation(content);
                    isNewInput = true;
                    operationPerformed = true;
                }
                else if (operationPerformed)
                {
                    currentOperation = ConvertOperation(content);
                }
            }
        }

        /// <summary>
        /// Обработка специальных функций (C, ±, %)
        /// </summary>
        public void ProcessSpecialFunction(string content)
        {
            switch (content)
            {
                case "C": ClearAll(); break;
                case "±": ToggleSign(); break;
                case "%": CalculatePercent(); break;
            }
        }

        /// <summary>
        /// Обработка клавиши Backspace
        /// </summary>
        public void ProcessBackspace()
        {
            if (!string.IsNullOrEmpty(currentInput) && currentInput != "0")
            {
                if (currentInput.Length > 1)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                else
                {
                    currentInput = "0";
                    isNewInput = true;
                }
            }
        }

        private string ConvertOperation(string operation)
        {
            switch (operation)
            {
                case "÷": return "/";
                case "×": return "*";
                case "−": return "-";
                default: return operation;
            }
        }

        private void CalculateResult()
        {
            if (string.IsNullOrEmpty(previousInput) || string.IsNullOrEmpty(currentInput) || string.IsNullOrEmpty(currentOperation))
                return;

            try
            {
                NumberFormatInfo provider = new NumberFormatInfo
                {
                    NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                    NumberGroupSeparator = ""
                };

                double num1 = double.Parse(previousInput, provider);
                double num2 = double.Parse(currentInput, provider);
                double result = 0;

                switch (currentOperation)
                {
                    case "+": result = num1 + num2; break;
                    case "-": result = num1 - num2; break;
                    case "*": result = num1 * num2; break;
                    case "/":
                        if (num2 == 0)
                        {
                            MessageBox.Show("Деление на ноль невозможно!", "Ошибка",
                                          MessageBoxButton.OK, MessageBoxImage.Warning);
                            ClearAll();
                            return;
                        }
                        result = num1 / num2;
                        break;
                }

                currentInput = result.ToString(provider);
                previousInput = "";
            }
            catch
            {
                MessageBox.Show("Ошибка вычисления!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAll();
            }
        }

        private void ToggleSign()
        {
            if (!string.IsNullOrEmpty(currentInput) && currentInput != "0")
            {
                currentInput = currentInput.StartsWith("-") ?
                              currentInput.Substring(1) : "-" + currentInput;
            }
        }

        private void CalculatePercent()
        {
            if (!string.IsNullOrEmpty(currentInput))
            {
                try
                {
                    NumberFormatInfo provider = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                        NumberGroupSeparator = ""
                    };

                    if (!string.IsNullOrEmpty(previousInput))
                    {
                        double num1 = double.Parse(previousInput, provider);
                        double num2 = double.Parse(currentInput, provider);
                        currentInput = ((num1 * num2) / 100).ToString(provider);
                    }
                    else
                    {
                        double num = double.Parse(currentInput, provider) / 100;
                        currentInput = num.ToString(provider);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка вычисления процента!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearAll()
        {
            currentInput = "";
            previousInput = "";
            currentOperation = "";
            isNewInput = true;
            operationPerformed = false;
        }

        /// <summary>
        /// Возвращает отформатированное число для отображения
        /// </summary>
        public string GetFormattedDisplay()
        {
            string text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;

            if (text.Length > 15)
            {
                try
                {
                    NumberFormatInfo provider = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
                    };

                    double num = double.Parse(text, provider);
                    text = num.ToString("E5", provider);
                }
                catch
                {
                    if (text.Length > 20)
                        text = text.Substring(0, 20);
                }
            }

            return text;
        }
    }
}
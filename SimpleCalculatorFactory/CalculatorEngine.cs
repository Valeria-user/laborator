using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace SimpleCalculatorFactory
{
    public class CalculatorEngine
    {
        private string currentInput = "";
        private string previousInput = "";
        private string currentOperation = "";
        private bool isNewInput = true;
        private bool operationPerformed = false;

        // Память
        private double memory = 0;

        // История для отладки
        private List<string> history = new List<string>();

        public string CurrentInput => currentInput;

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

        public void ProcessSpecialFunction(string content)
        {
            switch (content)
            {
                case "C": ClearAll(); break;
                case "±": ToggleSign(); break;
                case "%": CalculatePercent(); break;
            }
        }

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

        /// <summary>
        /// Выполнение научных функций
        /// </summary>
        public void ExecuteScientificFunction(string function)
        {
            if (string.IsNullOrEmpty(currentInput) || currentInput == "0")
                return;

            try
            {
                double num = double.Parse(currentInput, CultureInfo.InvariantCulture);
                double result = 0;
                string operation = "";

                switch (function)
                {
                    case "sin":
                        result = Math.Sin(num * Math.PI / 180); // градусы
                        operation = $"sin({num}°)";
                        break;
                    case "cos":
                        result = Math.Cos(num * Math.PI / 180);
                        operation = $"cos({num}°)";
                        break;
                    case "tan":
                        result = Math.Tan(num * Math.PI / 180);
                        operation = $"tan({num}°)";
                        break;
                    case "ln":
                        if (num <= 0) throw new Exception("ln(x) требует x > 0");
                        result = Math.Log(num);
                        operation = $"ln({num})";
                        break;
                    case "log":
                        if (num <= 0) throw new Exception("log(x) требует x > 0");
                        result = Math.Log10(num);
                        operation = $"log({num})";
                        break;
                    case "√":
                        if (num < 0) throw new Exception("√(x) требует x ≥ 0");
                        result = Math.Sqrt(num);
                        operation = $"√({num})";
                        break;
                    case "x²":
                        result = num * num;
                        operation = $"{num}²";
                        break;
                    case "eˣ":
                        result = Math.Exp(num);
                        operation = $"e^{num}";
                        break;
                }

                currentInput = result.ToString(CultureInfo.CurrentCulture);
                AddToHistory($"{operation} = {currentInput}");
                isNewInput = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Обработка операций с памятью
        /// </summary>
        public void HandleMemoryOperation(string operation)
        {
            try
            {
                double current = string.IsNullOrEmpty(currentInput) ? 0 :
                    double.Parse(currentInput, CultureInfo.CurrentCulture);

                switch (operation)
                {
                    case "MC":
                        memory = 0;
                        MessageBox.Show("Память очищена", "MC", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "MR":
                        currentInput = memory.ToString(CultureInfo.CurrentCulture);
                        isNewInput = true;
                        break;
                    case "M+":
                        memory += current;
                        MessageBox.Show($"Добавлено в память: {memory}", "M+", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "M-":
                        memory -= current;
                        MessageBox.Show($"Вычтено из памяти: {memory}", "M-", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка памяти: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddToHistory(string entry)
        {
            history.Add($"{DateTime.Now:HH:mm:ss} - {entry}");
            if (history.Count > 20)
                history.RemoveAt(0);
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
                double num1 = double.Parse(previousInput, CultureInfo.CurrentCulture);
                double num2 = double.Parse(currentInput, CultureInfo.CurrentCulture);
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

                currentInput = result.ToString(CultureInfo.CurrentCulture);
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
                    if (!string.IsNullOrEmpty(previousInput))
                    {
                        double num1 = double.Parse(previousInput, CultureInfo.CurrentCulture);
                        double num2 = double.Parse(currentInput, CultureInfo.CurrentCulture);
                        currentInput = ((num1 * num2) / 100).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        double num = double.Parse(currentInput, CultureInfo.CurrentCulture) / 100;
                        currentInput = num.ToString(CultureInfo.CurrentCulture);
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

        public string GetFormattedDisplay()
        {
            string text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;

            if (text.Length > 15)
            {
                try
                {
                    double num = double.Parse(text, CultureInfo.CurrentCulture);
                    text = num.ToString("E5", CultureInfo.CurrentCulture);
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
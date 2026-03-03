using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace laba_1
{
    public partial class MainWindow : Window
    {
        private string currentInput = "";
        private string previousInput = "";
        private string currentOperation = "";
        private bool isNewInput = true;
        private bool operationPerformed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string content = button.Content.ToString();

            if (IsDigitOrPoint(content))
                HandleDigitOrPoint(content);
            else if (IsOperation(content))
                HandleOperation(content);
            else
                HandleSpecialFunction(content);
        }

        private bool IsDigitOrPoint(string content)
        {
            return "0123456789.,".Contains(content);
        }

        private bool IsOperation(string content)
        {
            return content == "+" || content == "−" || content == "×" || content == "÷" || content == "=";
        }

        private void HandleDigitOrPoint(string content)
        {
            // Определяем десятичный разделитель для текущей культуры (точка или запятая)
            string decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            // Преобразуем введенную точку/запятую в правильный разделитель
            if (content == "." || content == ",")
                content = decimalSeparator;

            // Новое число после операции
            if (isNewInput)
            {
                currentInput = (content == decimalSeparator) ? "0" + decimalSeparator : content;
                isNewInput = false;
            }
            else
            {
                // Запрещаем второй десятичный разделитель
                if (content == decimalSeparator && !currentInput.Contains(decimalSeparator))
                    currentInput += content;
                else if (content != decimalSeparator && currentInput.Length < 15)
                    currentInput += content;
            }

            UpdateDisplay(currentInput);
        }

        private void HandleOperation(string content)
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

        private void HandleSpecialFunction(string content)
        {
            switch (content)
            {
                case "C": ClearAll(); break;
                case "±": ToggleSign(); break;
                case "%": CalculatePercent(); break;
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
                // Используем правильный десятичный разделитель для парсинга
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
                        // Проверка деления на ноль
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
                UpdateDisplay(currentInput);
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
            // Смена знака числа (+/-)
            if (!string.IsNullOrEmpty(currentInput) && currentInput != "0")
            {
                currentInput = currentInput.StartsWith("-") ?
                              currentInput.Substring(1) : "-" + currentInput;
                UpdateDisplay(currentInput);
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
                        // Процент от предыдущего числа
                        double num1 = double.Parse(previousInput, provider);
                        double num2 = double.Parse(currentInput, provider);
                        currentInput = ((num1 * num2) / 100).ToString(provider);
                    }
                    else
                    {
                        // Процент как доля от числа
                        double num = double.Parse(currentInput, provider) / 100;
                        currentInput = num.ToString(provider);
                    }

                    UpdateDisplay(currentInput);
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
            UpdateDisplay("0");
        }

        private void UpdateDisplay(string text)
        {
            // Научная нотация для очень больших чисел
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

            DisplayTextBox.Text = text;
        }
    }
}
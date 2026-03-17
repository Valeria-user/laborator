using System;
using System.Windows.Input;

namespace laba_1
{
    /// <summary>
    /// Обработчик ввода с клавиатуры и кнопок
    /// </summary>
    public class InputHandler
    {
        private CalculatorEngine calculator;
        private Action<string> updateDisplayCallback;

        public InputHandler(CalculatorEngine calculator, Action<string> updateDisplayCallback)
        {
            this.calculator = calculator;
            this.updateDisplayCallback = updateDisplayCallback;
        }

        /// <summary>
        /// Обработка нажатия клавиши
        /// </summary>
        public void HandleKey(KeyEventArgs e)
        {
            string keyContent = GetKeyContent(e);

            if (!string.IsNullOrEmpty(keyContent))
            {
                ProcessInput(keyContent);
            }
            else if (IsSpecialKey(e.Key))
            {
                HandleSpecialKey(e.Key);
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки мыши
        /// </summary>
        public void HandleButton(string content)
        {
            ProcessInput(content);
        }

        private void ProcessInput(string content)
        {
            if (IsDigitOrPoint(content))
            {
                calculator.ProcessDigitOrPoint(content);
                UpdateDisplay();
            }
            else if (IsOperation(content))
            {
                calculator.ProcessOperation(content);
                UpdateDisplay();
            }
            else
            {
                calculator.ProcessSpecialFunction(content);
                UpdateDisplay();
            }
        }

        private string GetKeyContent(KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
                return (e.Key - Key.D0).ToString();

            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                return (e.Key - Key.NumPad0).ToString();

            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.OemComma)
                return ".";

            if (e.Key == Key.Add || (e.Key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.Shift))
                return "+";

            if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                return "−";

            if (e.Key == Key.Multiply)
                return "×";

            if (e.Key == Key.Divide)
                return "÷";

            if (e.Key == Key.Enter || e.Key == Key.Return)
                return "=";

            return null;
        }

        private bool IsDigitOrPoint(string content)
        {
            return "0123456789.,".Contains(content);
        }

        private bool IsOperation(string content)
        {
            return content == "+" || content == "−" || content == "×" || content == "÷" || content == "=";
        }

        private bool IsSpecialKey(Key key)
        {
            return key == Key.Escape || key == Key.Back || key == Key.Delete;
        }

        private void HandleSpecialKey(Key key)
        {
            switch (key)
            {
                case Key.Escape:
                    calculator.ProcessSpecialFunction("C");
                    UpdateDisplay();
                    break;
                case Key.Back:
                case Key.Delete:
                    calculator.ProcessBackspace();
                    UpdateDisplay();
                    break;
            }
        }

        private void UpdateDisplay()
        {
            updateDisplayCallback?.Invoke(calculator.GetFormattedDisplay());
        }
    }
}
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculatorFactory
{
    /// <summary>
    /// Фабрика для создания определений кнопок
    /// </summary>
    public class ButtonFactory
    {
        private List<CalculatorButton> standardButtons;
        private List<CalculatorButton> scientificButtons;

        public ButtonFactory()
        {
            InitializeStandardButtons();
            InitializeScientificButtons();
        }

        private void InitializeStandardButtons()
        {
            standardButtons = new List<CalculatorButton>
            {
                // Ряд 1 (row = 1) - действия
                new ActionButton("BtnC", "C", 1, 0),
                new ActionButton("BtnPlusMinus", "±", 1, 1),
                new ActionButton("BtnPercent", "%", 1, 2),
                new OperationButton("BtnDivide", "÷", 1, 3),

                // Ряд 2 (row = 2) - цифры 7-9 и ×
                new NumberButton("Btn7", "7", 2, 0),
                new NumberButton("Btn8", "8", 2, 1),
                new NumberButton("Btn9", "9", 2, 2),
                new OperationButton("BtnMultiply", "×", 2, 3),

                // Ряд 3 (row = 3) - цифры 4-6 и −
                new NumberButton("Btn4", "4", 3, 0),
                new NumberButton("Btn5", "5", 3, 1),
                new NumberButton("Btn6", "6", 3, 2),
                new OperationButton("BtnSubtract", "−", 3, 3),

                // Ряд 4 (row = 4) - цифры 1-3 и +
                new NumberButton("Btn1", "1", 4, 0),
                new NumberButton("Btn2", "2", 4, 1),
                new NumberButton("Btn3", "3", 4, 2),
                new OperationButton("BtnAdd", "+", 4, 3),

                // Ряд 5 (row = 5) - 0, ., =
                new ActionButton("Btn0", "0", 5, 0, 2),
                new NumberButton("BtnPoint", ".", 5, 2),
                new ActionButton("BtnEquals", "=", 5, 3)
            };
        }

        private void InitializeScientificButtons()
        {
            scientificButtons = new List<CalculatorButton>
            {
                // Научные кнопки (row = 0)
                new ScientificButton("BtnSin", "sin", 0, 0),
                new ScientificButton("BtnCos", "cos", 0, 1),
                new ScientificButton("BtnTan", "tan", 0, 2),
                new ScientificButton("BtnLn", "ln", 0, 3),

                // Научные кнопки (row = 1)
                new ScientificButton("BtnSqrt", "√", 1, 0),
                new ScientificButton("BtnPower", "x²", 1, 1),
                new ScientificButton("BtnExp", "eˣ", 1, 2),
                new ScientificButton("BtnLog", "log", 1, 3),

                // Кнопки памяти (row = 6)
                new MemoryButton("BtnMc", "MC", 6, 0),
                new MemoryButton("BtnMr", "MR", 6, 1),
                new MemoryButton("BtnMPlus", "M+", 6, 2),
                new MemoryButton("BtnMMinus", "M-", 6, 3)
            };
        }

        public void ApplyStyleToButton(Button button, CalculatorButton buttonDef)
        {
            buttonDef.ApplyStyle(button);

            // Устанавливаем позицию в сетке
            Grid.SetRow(button, buttonDef.Row);
            Grid.SetColumn(button, buttonDef.Column);

            if (buttonDef.ColumnSpan > 1)
                Grid.SetColumnSpan(button, buttonDef.ColumnSpan);

            if (buttonDef.RowSpan > 1)
                Grid.SetRowSpan(button, buttonDef.RowSpan);
        }

        public List<CalculatorButton> GetStandardButtons()
        {
            return standardButtons;
        }

        public List<CalculatorButton> GetScientificButtons()
        {
            return scientificButtons;
        }

        public CalculatorButton FindButtonDefinition(string buttonName, bool isScientificMode)
        {
            var list = isScientificMode ? scientificButtons : standardButtons;
            return list.Find(b => b.Name == buttonName);
        }
    }
}
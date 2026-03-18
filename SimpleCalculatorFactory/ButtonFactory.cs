using System.Collections.Generic;

namespace SimpleCalculatorFactory
{
    /// <summary>
    /// Фабрика для создания определений кнопок
    /// </summary>
    public class ButtonFactory
    {
        /// <summary>
        /// Возвращает список определений для стандартного режима
        /// </summary>
        public List<CalculatorButton> GetStandardButtons()
        {
            var buttons = new List<CalculatorButton>();

            // Ряд 0: пусто (для научных кнопок, но в стандартном режиме их нет)

            // Ряд 1: C, ±, %, ÷
            buttons.Add(new ActionButton("BtnC", "C", 1, 0));
            buttons.Add(new ActionButton("BtnPlusMinus", "±", 1, 1));
            buttons.Add(new ActionButton("BtnPercent", "%", 1, 2));
            buttons.Add(new OperationButton("BtnDivide", "÷", 1, 3));

            // Ряд 2: 7, 8, 9, ×
            buttons.Add(new NumberButton("Btn7", "7", 2, 0));
            buttons.Add(new NumberButton("Btn8", "8", 2, 1));
            buttons.Add(new NumberButton("Btn9", "9", 2, 2));
            buttons.Add(new OperationButton("BtnMultiply", "×", 2, 3));

            // Ряд 3: 4, 5, 6, −
            buttons.Add(new NumberButton("Btn4", "4", 3, 0));
            buttons.Add(new NumberButton("Btn5", "5", 3, 1));
            buttons.Add(new NumberButton("Btn6", "6", 3, 2));
            buttons.Add(new OperationButton("BtnSubtract", "−", 3, 3));

            // Ряд 4: 1, 2, 3, +
            buttons.Add(new NumberButton("Btn1", "1", 4, 0));
            buttons.Add(new NumberButton("Btn2", "2", 4, 1));
            buttons.Add(new NumberButton("Btn3", "3", 4, 2));
            buttons.Add(new OperationButton("BtnAdd", "+", 4, 3));

            // Ряд 5: 0 (широкая), ., =
            buttons.Add(new ActionButton("Btn0", "0", 5, 0, 2));
            buttons.Add(new NumberButton("BtnPoint", ".", 5, 2));
            buttons.Add(new ActionButton("BtnEquals", "=", 5, 3));

            return buttons;
        }

        /// <summary>
        /// Возвращает список определений для научного режима
        /// </summary>
        public List<CalculatorButton> GetScientificButtons()
        {
            var buttons = new List<CalculatorButton>();

            // Ряд 0: Научные функции (ПЕРВЫЙ РЯД)
            buttons.Add(new ScientificButton("BtnSin", "sin", 0, 0));
            buttons.Add(new ScientificButton("BtnCos", "cos", 0, 1));
            buttons.Add(new ScientificButton("BtnTan", "tan", 0, 2));
            buttons.Add(new ScientificButton("BtnLn", "ln", 0, 3));

            // Ряд 1: Научные функции (ВТОРОЙ РЯД)
            buttons.Add(new ScientificButton("BtnSqrt", "√", 1, 0));
            buttons.Add(new ScientificButton("BtnPower", "x²", 1, 1));
            buttons.Add(new ScientificButton("BtnExp", "eˣ", 1, 2));
            buttons.Add(new ScientificButton("BtnLog", "log", 1, 3));

            // Ряд 2: C, ±, %, ÷ (СДВИНУТО на 2 ряда вниз)
            buttons.Add(new ActionButton("BtnC", "C", 2, 0));
            buttons.Add(new ActionButton("BtnPlusMinus", "±", 2, 1));
            buttons.Add(new ActionButton("BtnPercent", "%", 2, 2));
            buttons.Add(new OperationButton("BtnDivide", "÷", 2, 3));

            // Ряд 3: 7, 8, 9, ×
            buttons.Add(new NumberButton("Btn7", "7", 3, 0));
            buttons.Add(new NumberButton("Btn8", "8", 3, 1));
            buttons.Add(new NumberButton("Btn9", "9", 3, 2));
            buttons.Add(new OperationButton("BtnMultiply", "×", 3, 3));

            // Ряд 4: 4, 5, 6, −
            buttons.Add(new NumberButton("Btn4", "4", 4, 0));
            buttons.Add(new NumberButton("Btn5", "5", 4, 1));
            buttons.Add(new NumberButton("Btn6", "6", 4, 2));
            buttons.Add(new OperationButton("BtnSubtract", "−", 4, 3));

            // Ряд 5: 1, 2, 3, +
            buttons.Add(new NumberButton("Btn1", "1", 5, 0));
            buttons.Add(new NumberButton("Btn2", "2", 5, 1));
            buttons.Add(new NumberButton("Btn3", "3", 5, 2));
            buttons.Add(new OperationButton("BtnAdd", "+", 5, 3));// Ряд 6: 0 (широкая), ., =
            buttons.Add(new ActionButton("Btn0", "0", 6, 0, 2));
            buttons.Add(new NumberButton("BtnPoint", ".", 6, 2));
            buttons.Add(new ActionButton("BtnEquals", "=", 6, 3));

            // Ряд 7: Кнопки памяти
            buttons.Add(new MemoryButton("BtnMc", "MC", 7, 0));
            buttons.Add(new MemoryButton("BtnMr", "MR", 7, 1));
            buttons.Add(new MemoryButton("BtnMPlus", "M+", 7, 2));
            buttons.Add(new MemoryButton("BtnMMinus", "M-", 7, 3));

            return buttons;
        }
    }
}
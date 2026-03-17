using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleCalculatorFactory
{
    /// <summary>
    /// Базовый класс для всех кнопок калькулятора
    /// </summary>
    public abstract class CalculatorButton
    {
        public string Name { get; protected set; }
        public string Content { get; protected set; }
        public int Row { get; protected set; }
        public int Column { get; protected set; }
        public int RowSpan { get; protected set; } = 1;
        public int ColumnSpan { get; protected set; } = 1;
        public bool IsScientific { get; protected set; } = false;

        public abstract void ApplyStyle(Button button);
        public abstract void HandleClick(CalculatorEngine engine, MainWindow window);
    }

    /// <summary>
    /// Кнопка с цифрой
    /// </summary>
    public class NumberButton : CalculatorButton
    {
        public NumberButton(string name, string content, int row, int col, bool isScientific = false)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.FontSize = 22;
            button.Background = new SolidColorBrush(Colors.White);
            button.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            button.Margin = new Thickness(3);
        }

        public override void HandleClick(CalculatorEngine engine, MainWindow window)
        {
            window.ProcessInput(Content);
        }
    }

    /// <summary>
    /// Кнопка операции (+, -, ×, ÷)
    /// </summary>
    public class OperationButton : CalculatorButton
    {
        public OperationButton(string name, string content, int row, int col, bool isScientific = false)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.FontSize = Content == "−" ? 28 : 24;
            button.FontWeight = FontWeights.Bold;
            button.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            button.Foreground = new SolidColorBrush(Colors.White);
            button.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 136, 0));
            button.Margin = new Thickness(3);
        }

        public override void HandleClick(CalculatorEngine engine, MainWindow window)
        {
            window.ProcessInput(Content);
        }
    }

    /// <summary>
    /// Кнопка действия (C, ±, %, =)
    /// </summary>
    public class ActionButton : CalculatorButton
    {
        public ActionButton(string name, string content, int row, int col, int colSpan = 1, bool isScientific = false)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            ColumnSpan = colSpan;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.FontSize = Content == "C" ? 20 : (Content == "=" ? 26 : 20);
            button.FontWeight = Content == "C" || Content == "=" ? FontWeights.Bold : FontWeights.Normal;
            button.Background = Content == "=" ?
                new SolidColorBrush(Color.FromRgb(255, 165, 0)) :
                new SolidColorBrush(Color.FromRgb(229, 229, 229));
            button.Foreground = Content == "=" ?
                new SolidColorBrush(Colors.White) :
                new SolidColorBrush(Colors.Black);
            button.BorderBrush = Content == "=" ?
                new SolidColorBrush(Color.FromRgb(204, 136, 0)) :
                new SolidColorBrush(Color.FromRgb(204, 204, 204));
            button.Margin = new Thickness(3);
        }

        public override void HandleClick(CalculatorEngine engine, MainWindow window)
        {
            window.ProcessInput(Content);
        }
    }

    /// <summary>
    /// Научная кнопка (sin, cos, tan, и т.д.)
    /// </summary>
    public class ScientificButton : CalculatorButton
    {
        public ScientificButton(string name, string content, int row, int col, bool isScientific = true)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.FontSize = Content == "√" ? 20 : 16;
            button.FontWeight = Content == "√" ? FontWeights.Bold : FontWeights.Normal;
            button.Background = new SolidColorBrush(Color.FromRgb(224, 224, 224));
            button.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            button.Margin = new Thickness(3);
        }

        public override void HandleClick(CalculatorEngine engine, MainWindow window)
        {
            engine.ExecuteScientificFunction(Content);
            window.UpdateDisplay();
        }
    }

    /// <summary>
    /// Кнопка памяти
    /// </summary>
    public class MemoryButton : CalculatorButton
    {
        public MemoryButton(string name, string content, int row, int col, bool isScientific = true)
        {
            Name = name;
            Content = content;
            Row = row;
            Column = col;
            IsScientific = isScientific;
        }

        public override void ApplyStyle(Button button)
        {
            button.Content = Content;
            button.FontSize = 14;
            button.Background = new SolidColorBrush(Color.FromRgb(208, 208, 208));
            button.BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            button.Margin = new Thickness(3);
        }

        public override void HandleClick(CalculatorEngine engine, MainWindow window)
        {
            engine.HandleMemoryOperation(Content);
            window.UpdateDisplay();
        }
    }
}
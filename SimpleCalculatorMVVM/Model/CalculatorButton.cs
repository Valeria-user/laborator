using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleCalculatorMVVM.Models
{

    /// Базовый класс для всех кнопок калькулятора

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
        // HandleClick удалён - в MVVM используется Command
    }


    /// Кнопка с цифрой

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
    }


    /// Кнопка операции (+, -, ×, ÷)

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
    }


    /// Кнопка действия (C, ±, %, =)

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
            if (Content == "↩" || Content == "↪")
            {
                button.Background = new SolidColorBrush(Colors.LightGreen);
                button.FontSize = 24;
            }
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
    }


    /// Научная кнопка (sin, cos, tan, и т.д.)

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
    }


    /// Кнопка памяти

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
    }
}
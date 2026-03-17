using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleCalculatorFactory
{
    public partial class MainWindow : Window
    {
        private CalculatorEngine calculator;
        private ButtonFactory buttonFactory;
        private bool isScientificMode = false;

        public MainWindow()
        {
            InitializeComponent();

            calculator = new CalculatorEngine();
            buttonFactory = new ButtonFactory();

            // Применяем стили из фабрики ко всем кнопкам
            ApplyStylesFromFactory();

            // Подключаем обработчики событий
            ModeSelector.SelectionChanged += ModeSelector_SelectionChanged;

            // Подключаем обработчики для всех кнопок
            AttachClickHandlers();

            // Настройка клавиатуры
            this.KeyDown += MainWindow_KeyDown;
        }

        private void ApplyStylesFromFactory()
        {
            // Применяем стили для стандартных кнопок
            foreach (var buttonDef in buttonFactory.GetStandardButtons())
            {
                var button = FindName(buttonDef.Name) as Button;
                if (button != null)
                {
                    buttonFactory.ApplyStyleToButton(button, buttonDef);
                }
            }

            // Применяем стили для научных кнопок
            foreach (var buttonDef in buttonFactory.GetScientificButtons())
            {
                var button = FindName(buttonDef.Name) as Button;
                if (button != null)
                {
                    buttonFactory.ApplyStyleToButton(button, buttonDef);
                }
            }
        }

        private void AttachClickHandlers()
        {
            // Подключаем обработчики для всех кнопок в ButtonsGrid
            foreach (var item in ButtonsGrid.Children)
            {
                if (item is Button button)
                {
                    button.Click += Button_Click;
                }
            }
        }

        private void ModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isScientificMode = ModeSelector.SelectedIndex == 1;
            UpdateScientificButtonsVisibility();
        }

        private void UpdateScientificButtonsVisibility()
        {
            // Показываем или скрываем научные кнопки
            var visibility = isScientificMode ? Visibility.Visible : Visibility.Collapsed;

            foreach (var buttonDef in buttonFactory.GetScientificButtons())
            {
                var button = FindName(buttonDef.Name) as Button;
                if (button != null)
                {
                    button.Visibility = visibility;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Находим определение кнопки в фабрике
                var buttonDef = buttonFactory.FindButtonDefinition(button.Name, isScientificMode);

                if (buttonDef != null)
                {
                    buttonDef.HandleClick(calculator, this);
                }
                else
                {
                    // Если не нашли в фабрике, обрабатываем по-старому
                    ProcessInput(button.Content.ToString());
                }

                UpdateDisplay();
                this.Focus();
            }
        }

        public void ProcessInput(string content)
        {
            if (IsDigitOrPoint(content))
            {
                calculator.ProcessDigitOrPoint(content);
            }
            else if (IsOperation(content))
            {
                calculator.ProcessOperation(content);
            }
            else
            {
                calculator.ProcessSpecialFunction(content);
            }
        }

        private bool IsDigitOrPoint(string content)
        {
            return "0123456789.,".Contains(content);
        }

        private bool IsOperation(string content)
        {
            return content == "+" || content == "−" || content == "×" || content == "÷" || content == "=";
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Обработка клавиатуры
            string keyContent = GetKeyFromKeyboard(e);

            if (!string.IsNullOrEmpty(keyContent))
            {
                ProcessInput(keyContent);
                UpdateDisplay();
                e.Handled = true;
            }
            else if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                calculator.ProcessBackspace();
                UpdateDisplay();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                calculator.ProcessSpecialFunction("C");
                UpdateDisplay();
                e.Handled = true;
            }
        }

        private string GetKeyFromKeyboard(KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
                return (e.Key - Key.D0).ToString();

            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                return (e.Key - Key.NumPad0).ToString();

            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
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

        public void UpdateDisplay()
        {
            DisplayTextBox.Text = calculator.GetFormattedDisplay();
        }
    }
}
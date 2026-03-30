using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleCalculatorFactory
{
    public partial class MainWindow : Window
    {
        private CalculatorEngine calculator;
        private InputHandler inputHandler;
        private ButtonFactory buttonFactory;
        private bool isScientificMode = false;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация компонентов
            calculator = new CalculatorEngine();

            // ИСПРАВЛЕНИЕ: явно указываем, какой метод передаем
            inputHandler = new InputHandler(calculator, UpdateDisplayFromHandler);

            buttonFactory = new ButtonFactory();

            // Создаем стандартные кнопки при запуске
            CreateButtonsFromFactory(false);

            // Подписываемся на событие изменения режима
            ModeSelector.SelectionChanged += ModeSelector_SelectionChanged;

            // Настройка клавиатуры
            this.KeyDown += MainWindow_KeyDown;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            this.Loaded += (s, e) => this.Focus();
        }

        /// <summary>
        /// Создает кнопки через фабрику
        /// </summary>
        private void CreateButtonsFromFactory(bool scientific)
        {
            // Очищаем старые кнопки
            ButtonsGrid.Children.Clear();

            // Получаем определения кнопок из фабрики
            var buttonDefs = scientific ?
                buttonFactory.GetScientificButtons() :
                buttonFactory.GetStandardButtons();

            // Создаем и настраиваем каждую кнопку
            foreach (var buttonDef in buttonDefs)
            {
                // Создаем WPF кнопку
                Button wpfButton = new Button();

                // Применяем стиль из определения
                buttonDef.ApplyStyle(wpfButton);

                // Сохраняем определение кнопки в Tag для обработки клика
                wpfButton.Tag = buttonDef;

                // Устанавливаем позицию
                Grid.SetRow(wpfButton, buttonDef.Row);
                Grid.SetColumn(wpfButton, buttonDef.Column);

                if (buttonDef.ColumnSpan > 1)
                    Grid.SetColumnSpan(wpfButton, buttonDef.ColumnSpan);

                if (buttonDef.RowSpan > 1)
                    Grid.SetRowSpan(wpfButton, buttonDef.RowSpan);

                // Добавляем обработчик
                wpfButton.Click += Button_Click;

                // Добавляем в сетку
                ButtonsGrid.Children.Add(wpfButton);
            }
        }

        /// <summary>
        /// Обработчик переключения режима
        /// </summary>
        private void ModeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isScientificMode = ModeSelector.SelectedIndex == 1;
            CreateButtonsFromFactory(isScientificMode);
        }

        /// <summary>
        /// Обработчик нажатия на кнопки
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            // Получаем определение кнопки из Tag
            if (button.Tag is CalculatorButton buttonDef)
            {
                // Вызываем обработчик из определения кнопки
                buttonDef.HandleClick(calculator, this);
            }

            UpdateDisplay();
            this.Focus();
        }

        /// <summary>
        /// Метод для обработки ввода (вызывается из классов кнопок)
        /// </summary>
        public void ProcessInput(string content)
        {
            if (inputHandler != null)
            {
                inputHandler.HandleButton(content);
            }
        }/// <summary>
         /// Обновление дисплея (без параметров)
         /// </summary>
        public void UpdateDisplay()
        {
            if (calculator != null)
            {
                DisplayTextBox.Text = calculator.GetFormattedDisplay();
            }
        }

        /// <summary>
        /// Обновление дисплея с параметром (для InputHandler)
        /// </summary>
        private void UpdateDisplayFromHandler(string text)  // ИЗМЕНЕНО: новое имя
        {
            DisplayTextBox.Text = text;
        }

        // === ОБРАБОТЧИКИ КЛАВИАТУРЫ ===

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            inputHandler?.HandleKey(e);
            UpdateDisplay();
            e.Handled = true;
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace laba_1
{
    public partial class MainWindow : Window
    {
        private CalculatorEngine calculator;
        private InputHandler inputHandler;

        public MainWindow()
        {
            InitializeComponent();

            calculator = new CalculatorEngine();
            inputHandler = new InputHandler(calculator, UpdateDisplay);

            // Создаём кнопки из массива
            CreateButtonsFromArray();

            // Настройка обработчиков событий
            this.KeyDown += MainWindow_KeyDown;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            this.Loaded += (s, e) => this.Focus();
        }

        private void CreateButtonsFromArray()
        {
            // Массив с определением всех кнопок
            var buttonDefs = new (string type, string content, int row, int col, int rowSpan, int colSpan)[]
            {
                // Ряд 1 (row = 1)
                ("action", "C",  1, 0, 1, 1),
                ("action", "±",  1, 1, 1, 1),
                ("action", "%",  1, 2, 1, 1),
                ("binOp",  "÷",  1, 3, 1, 1),

                // Ряд 2 (row = 2)
                ("number", "7",  2, 0, 1, 1),
                ("number", "8",  2, 1, 1, 1),
                ("number", "9",  2, 2, 1, 1),
                ("binOp",  "×",  2, 3, 1, 1),

                // Ряд 3 (row = 3)
                ("number", "4",  3, 0, 1, 1),
                ("number", "5",  3, 1, 1, 1),
                ("number", "6",  3, 2, 1, 1),
                ("binOp",  "−",  3, 3, 1, 1),

                // Ряд 4 (row = 4)
                ("number", "1",  4, 0, 1, 1),
                ("number", "2",  4, 1, 1, 1),
                ("number", "3",  4, 2, 1, 1),
                ("binOp",  "+",  4, 3, 1, 1),

                // Ряд 5 (row = 5)
                ("action", "0",  5, 0, 1, 2),  // Кнопка 0 занимает 2 колонки
                ("number", ".",  5, 2, 1, 1),
                ("action", "=",  5, 3, 1, 1)
            };

            // Создаём кнопки по массиву
            foreach (var def in buttonDefs)
            {
                Button button = new Button
                {
                    Content = def.content,
                    Margin = new Thickness(3),
                    FontSize = GetFontSize(def.content),
                    FontWeight = GetFontWeight(def.type)
                };

                // Устанавливаем позицию в сетке
                Grid.SetRow(button, def.row);
                Grid.SetColumn(button, def.col);

                if (def.colSpan > 1)
                    Grid.SetColumnSpan(button, def.colSpan);

                if (def.rowSpan > 1)
                    Grid.SetRowSpan(button, def.rowSpan);

                // Добавляем обработчик
                button.Click += Button_Click;

                // Добавляем в сетку
                MainGrid.Children.Add(button);
            }
        }

        private double GetFontSize(string content)
        {
            // Для операторов делаем шрифт побольше
            if (content == "÷" || content == "×" || content == "−" || content == "+" || content == "=")
                return 24;

            if (content == "C" || content == "±" || content == "%")
                return 20;

            return 22; // для цифр
        }

        private FontWeight GetFontWeight(string type)
        {
            // Операторы делаем жирными
            if (type == "binOp" || type == "action" && (type == "=" || type == "C"))
                return FontWeights.Bold;

            return FontWeights.Normal;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            inputHandler.HandleKey(e);
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            inputHandler.HandleButton(button.Content.ToString());
            this.Focus();
        }

        private void UpdateDisplay(string text)
        {
            DisplayTextBox.Text = text;
        }
    }
}
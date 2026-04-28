using System.Windows;
using System.Windows.Controls;
using SimpleCalculatorMVVM.Models;

namespace SimpleCalculatorMVVM.Views
{
    public partial class MainWindow : Window
    {
        private ButtonFactory _buttonFactory;
        private bool _isScientificMode = false;

        public MainWindow()
        {
            InitializeComponent();

            _buttonFactory = new ButtonFactory();
            CreateButtons();

            ModeSelector.SelectionChanged += (s, e) =>
            {
                _isScientificMode = ModeSelector.SelectedIndex == 1;
                CreateButtons();
            };
        }

        private void CreateButtons()
        {
            ButtonsGrid.Children.Clear();

            var buttonDefs = _isScientificMode ?
                _buttonFactory.GetScientificButtons() :
                _buttonFactory.GetStandardButtons();

            foreach (var buttonDef in buttonDefs)
            {
                Button button = new Button();
                buttonDef.ApplyStyle(button);

                button.Command = ((ViewModels.CalculatorViewModel)DataContext).ButtonCommand;
                button.CommandParameter = buttonDef.Content;

                Grid.SetRow(button, buttonDef.Row);
                Grid.SetColumn(button, buttonDef.Column);

                if (buttonDef.ColumnSpan > 1)
                    Grid.SetColumnSpan(button, buttonDef.ColumnSpan);

                ButtonsGrid.Children.Add(button);
            }
            MessageBox.Show($"Добавлено кнопок: {ButtonsGrid.Children.Count}");
        }
    }
}
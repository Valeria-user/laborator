using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SimpleCalculatorMVVM.Models;
using SimpleCalculatorMVVM.Models.Commands;

namespace SimpleCalculatorMVVM.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorEngine _engine;
        private CommandInvoker _invoker;
        private string _display = "0";

        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged();
            }
        }

        public ICommand ButtonCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public CalculatorViewModel()
        {
            _engine = new CalculatorEngine();
            _invoker = new CommandInvoker();
            ButtonCommand = new RelayCommand(OnButtonClick);
            UndoCommand = new RelayCommand(_ => { _invoker.Undo(); UpdateDisplay(); });
            RedoCommand = new RelayCommand(_ => { _invoker.Redo(); UpdateDisplay(); });
        }

        private void OnButtonClick(object parameter)
        {
            if (parameter is not string content) return;

            ICalculatorCommand command = CreateCommand(content);
            if (command != null)
            {
                _invoker.ExecuteCommand(command);
                UpdateDisplay();
            }
        }

        private ICalculatorCommand CreateCommand(string content)
        {
            // Обработка Undo/Redo (без создания команды в историю)
            if (content == "↩")
            {
                _invoker.Undo();
                UpdateDisplay();
                return null;
            }
            if (content == "↪")
            {
                _invoker.Redo();
                UpdateDisplay();
                return null;
            }

            // Цифры и точка
            if (content.Length == 1 && (char.IsDigit(content[0]) || content == "."))
                return new DigitCommand(_engine, content);

            // Операции
            if (content == "+" || content == "−" || content == "×" || content == "÷")
                return new OperationCommand(_engine, content);
            if (content == "=")
                return new OperationCommand(_engine, "=");

            // Научные функции
            string[] scientific = { "sin", "cos", "tan", "ln", "log", "√", "x²", "eˣ" };
            if (System.Array.Exists(scientific, f => f == content))
                return new ScientificCommand(_engine, content);

            // Память
            string[] memory = { "MC", "MR", "M+", "M-" };
            if (System.Array.Exists(memory, m => m == content))
                return new MemoryCommand(_engine, content);

            // Действия C, ±, %
            if (content == "C" || content == "±" || content == "%")
                return new ActionCommand(_engine, content);

            // Backspace отдельно обрабатывается в MainWindow (клавиша)
            // Но если есть кнопка ⌫, можно добавить сюда
            return null;
        }

        public void ProcessBackspace()
        {
            var command = new BackspaceCommand(_engine);
            _invoker.ExecuteCommand(command);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            Display = _engine.GetFormattedDisplay();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
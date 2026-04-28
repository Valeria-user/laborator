namespace SimpleCalculatorMVVM.Models.Commands
{
    public class ActionCommand : ICalculatorCommand
    {
        private readonly CalculatorEngine _engine;
        private readonly string _action;
        private string _previousInput;
        private string _previousPreviousInput;
        private string _previousOperation;
        private bool _previousOperationPerformed;
        private bool _previousNewInput;

        public ActionCommand(CalculatorEngine engine, string action)
        {
            _engine = engine;
            _action = action;
        }

        public void Execute()
        {
            // Сохраняем полное состояние
            _previousInput = _engine.GetCurrentInput();
            _previousPreviousInput = _engine.GetPreviousInput();
            _previousOperation = _engine.GetCurrentOperation();
            _previousOperationPerformed = _engine.IsOperationPerformed();
            _previousNewInput = _engine.IsNewInput();

            _engine.ProcessSpecialFunction(_action);
        }

        public void Undo()
        {
            _engine.RestoreCurrentInput(_previousInput);
            _engine.SetPreviousInput(_previousPreviousInput);
            _engine.SetCurrentOperation(_previousOperation);
            _engine.SetOperationPerformed(_previousOperationPerformed);
            _engine.SetNewInputFlag(_previousNewInput);
        }
    }
}
namespace SimpleCalculatorMVVM.Models.Commands
{
    public class OperationCommand : ICalculatorCommand
    {
        private readonly CalculatorEngine _engine;
        private readonly string _operation;
        private string _previousInput;
        private string _previousPreviousInput;
        private string _previousOperation;
        private bool _previousOperationPerformed;

        public OperationCommand(CalculatorEngine engine, string operation)
        {
            _engine = engine;
            _operation = operation;
        }

        public void Execute()
        {
            _previousInput = _engine.GetCurrentInput();
            _previousPreviousInput = _engine.GetPreviousInput();
            _previousOperation = _engine.GetCurrentOperation();
            _previousOperationPerformed = _engine.IsOperationPerformed();

            _engine.ProcessOperation(_operation);
        }

        public void Undo()
        {
            _engine.RestoreCurrentInput(_previousInput);
            _engine.SetPreviousInput(_previousPreviousInput);
            _engine.SetCurrentOperation(_previousOperation);
            _engine.SetOperationPerformed(_previousOperationPerformed);
        }
    }
}
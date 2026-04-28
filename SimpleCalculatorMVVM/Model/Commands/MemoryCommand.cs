namespace SimpleCalculatorMVVM.Models.Commands
{
    public class MemoryCommand : ICalculatorCommand
    {
        private readonly CalculatorEngine _engine;
        private readonly string _operation;
        private string _previousState;

        public MemoryCommand(CalculatorEngine engine, string operation)
        {
            _engine = engine;
            _operation = operation;
        }

        public void Execute()
        {
            _previousState = _engine.GetCurrentInput();
            _engine.HandleMemoryOperation(_operation);
        }

        public void Undo()
        {
            // Для памяти отмена сложна, проще восстановить отображаемое число
            _engine.RestoreCurrentInput(_previousState);
        }
    }
}
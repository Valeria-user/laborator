namespace SimpleCalculatorMVVM.Models.Commands
{
    public class BackspaceCommand : ICalculatorCommand
    {
        private readonly CalculatorEngine _engine;
        private string _previousState;

        public BackspaceCommand(CalculatorEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _previousState = _engine.GetCurrentInput();
            _engine.ProcessBackspace();
        }

        public void Undo()
        {
            _engine.RestoreCurrentInput(_previousState);
        }
    }
}
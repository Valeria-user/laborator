using System.Collections.Generic;

namespace SimpleCalculatorMVVM.Models.Commands
{
    public class CommandInvoker
    {
        private readonly Stack<ICalculatorCommand> _undoStack = new Stack<ICalculatorCommand>();
        private readonly Stack<ICalculatorCommand> _redoStack = new Stack<ICalculatorCommand>();

        public void ExecuteCommand(ICalculatorCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
            }
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}
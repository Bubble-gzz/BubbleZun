using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleZun.Utils{
    public class UndoSystem
    {
        int stepID = 0;
        int capacity;
        LinkedList<Operation> historyStack = new LinkedList<Operation>();
        Stack<Operation> redoStack = new Stack<Operation>();
        public UndoSystem(int capacity = 100) {
            this.capacity = capacity;
        }
        public void Register(Operation operation, bool newStep = true)
        {
            if (newStep) stepID++;
            operation.stepID = stepID;
            historyStack.AddLast(operation);
            if (historyStack.Count > capacity) historyStack.RemoveFirst();
            redoStack.Clear();
        }
        public void IncreaseStepID() {
            stepID++;
        }
        public void Undo()
        {
            if (historyStack.Count == 0) return;
            int stepID = historyStack.Last.Value.stepID;
            string description = historyStack.Last.Value.description;
            do
            {
                Operation operation = historyStack.Last.Value;
                historyStack.RemoveLast();
                operation.Undo();
                redoStack.Push(operation);
            } while (historyStack.Count > 0 && historyStack.Last.Value.stepID == stepID);
            Debug.Log("Undo: " + description);
        }
        public void Redo()
        {
            if (redoStack.Count == 0) return;
            int stepID = redoStack.Peek().stepID;
            string description = redoStack.Peek().description;
            do
            {
                Operation operation = redoStack.Pop();
                operation.Redo();
                historyStack.AddLast(operation);
            } while (redoStack.Count > 0 && redoStack.Peek().stepID == stepID);
            Debug.Log("Redo: " + description);
        }

    }
    public abstract class Operation{
        public int stepID;
        public string description;
        public Operation(string description) {
            this.description = description;
        }
        public abstract void Undo();
        public abstract void Redo();
    }
}
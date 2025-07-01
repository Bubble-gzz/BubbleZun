using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            bool mute = false;
            do
            {
                Operation operation = historyStack.Last.Value;
                historyStack.RemoveLast();
                operation.Undo(mute);
                mute = true;
                redoStack.Push(operation);
            } while (historyStack.Count > 0 && historyStack.Last.Value.stepID == stepID);
            //Debug.Log("Undo Group: " + description);
        }
        public void Redo()
        {
            if (redoStack.Count == 0) return;
            int stepID = redoStack.Peek().stepID;
            string description = redoStack.Peek().description;
            bool mute = false;
            do
            {
                Operation operation = redoStack.Pop();
                operation.Redo(mute);
                historyStack.AddLast(operation);
                mute = true;
            } while (redoStack.Count > 0 && redoStack.Peek().stepID == stepID);
            //Debug.Log("Redo Group: " + description);
        }

    }
    public abstract class Operation{
        public int stepID;
        public string description;
        public Operation(string description) {
            this.description = description;
        }
        public abstract void Undo(bool mute = false);
        public abstract void Redo(bool mute = false);
    }
}
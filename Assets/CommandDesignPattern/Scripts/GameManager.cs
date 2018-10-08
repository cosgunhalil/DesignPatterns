using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseObject {

    public Actor Player;
    private Stack<Command> commandStack = new Stack<Command>();

    public override void AwakeObject()
    {
        AppEventManager.Instance.OnCommandCreated += CommandGetted;
        AppEventManager.Instance.UndoButtonClicked += Undo;
        Player.AwakeObject();
    }

    private void OnDestroy()
    {
        AppEventManager.Instance.OnCommandCreated -= CommandGetted;
        AppEventManager.Instance.UndoButtonClicked -= Undo;
    }

    public override void StartObject()
    {
        Player.StartObject();
    }

    private void CommandGetted(Command command)
    {
        AddCommand(command);
        InvokeLastCommand();
    }
    private void InvokeLastCommand()
    {
        var command = commandStack.Peek();
        command.Execute(Player);
    }

    private void AddCommand(Command command)
    {
        commandStack.Push(command);
    }

    public void Undo()
    {
        if (commandStack.Count > 0)
        {
            var command = commandStack.Pop();
            command.Undo(Player);
        }
    }

}

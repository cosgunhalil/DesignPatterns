using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandManager {

    public static List<Command> OldCommands = new List<Command>();

    public static void AddCommand(Command command)
    {
        OldCommands.Add(command);
    }

    public static void Undo()
    {
        if (OldCommands.Count > 0)
        {
            var commandToUndo = OldCommands[OldCommands.Count - 1];
            commandToUndo.Undo(GameManager.Instance.Player);
            OldCommands.Remove(commandToUndo);
        }
    }
}

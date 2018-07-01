using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler {

    public Command MoveRightButton;
    public Command MoveLeftButton;
    public Command JumpButton;

    public InputHandler()
    {
        MoveRightButton = new MoveRightCommand();
        MoveLeftButton = new MoveLeftCommand();
        JumpButton = new JumpCommand();
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeftButton.Execute(GameManager.Instance.Player);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRightButton.Execute(GameManager.Instance.Player);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton.Execute(GameManager.Instance.Player);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            CommandManager.Undo();
        }
    }

}

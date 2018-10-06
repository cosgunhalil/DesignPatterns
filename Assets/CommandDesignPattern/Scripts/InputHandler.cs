using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler {

    private Command MoveUp;
    private Command MoveDown;
    private Command MoveRight;
    private Command MoveLeft;
    private Command Jump;

    public InputHandler()
    {
        MoveUp = new MoveUpCommand();
        MoveDown = new MoveDownCommand();
        MoveRight = new MoveRightCommand();
        MoveLeft = new MoveLeftCommand();
        Jump = new JumpCommand();
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AppEventManager.Instance.CommandCreated(MoveLeft);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AppEventManager.Instance.CommandCreated(MoveRight);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            AppEventManager.Instance.CommandCreated(Jump);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AppEventManager.Instance.CommandCreated(MoveUp);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AppEventManager.Instance.CommandCreated(MoveDown);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            AppEventManager.Instance.SendUndoRequest();
        }
    }

}

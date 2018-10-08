using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEventManager {

    public delegate void CommandCreatedDelegate(Command command);
    public event CommandCreatedDelegate OnCommandCreated;

    public delegate void UndoButtonClickedDelegate();
    public event UndoButtonClickedDelegate UndoButtonClicked;

    private static readonly AppEventManager instance = new AppEventManager();

    static AppEventManager()
    {
    }

    private AppEventManager()
    {
    }

    public static AppEventManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void CommandCreated(Command command)
    {
        if (OnCommandCreated != null)
        {
            OnCommandCreated(command);
        }
    }

    public void SendUndoRequest()
    {
        if (UndoButtonClicked != null)
        {
            UndoButtonClicked();
        }
    }
}

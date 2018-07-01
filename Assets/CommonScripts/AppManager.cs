using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {

    private InputHandler _inputHandler;

    private void Awake()
    {
        GameManager.Instance.AwakeObject();
    }

    private void Start()
    {
        GameManager.Instance.StartObject();
        _inputHandler = new InputHandler();
    }

    public void Update()
    {
        _inputHandler.HandleInput();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour {

    public GameManager gameManager;
    private InputHandler _inputHandler;

    private void Awake()
    {
        gameManager.AwakeObject();
    }

    private void Start()
    {
        gameManager.StartObject();
        _inputHandler = new InputHandler();
    }

    public void Update()
    {
        _inputHandler.HandleInput();
    }

}

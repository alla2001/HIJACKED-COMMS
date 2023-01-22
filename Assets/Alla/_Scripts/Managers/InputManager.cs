using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
   
    public InputMap InputMap;
    public Clicker clicker;
    public MoveTheCamera moveTheCamera;
    private void Awake()
    {
        base.Awake();
        InputMap = new InputMap();
        InputMap.Enable();
    }
}

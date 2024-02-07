using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    void Awake() 
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update() 
    {
        Debug.Log(moveAction.ReadValue<float>());
    }

}

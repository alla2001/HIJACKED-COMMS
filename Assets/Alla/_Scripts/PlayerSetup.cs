using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(Character))]
public class PlayerSetup : NetworkBehaviour
{
    public Character character;
    private void Awake()
    {
        character= GetComponent<Character>();
    }
    private void Start()
    {
        if (netIdentity.isOwned)
        {
            InputManager.instance.clicker.assigendCharacter = character;
            InputManager.instance.moveTheCamera.assosiatedCharacter = character;

        }

    }
}

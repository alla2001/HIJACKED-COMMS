using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(Character))]
public class PlayerSetup : NetworkBehaviour
{
    public Character character;
    public static Character playerCharacter;
    public bool isPlayer;
    public GameObject holok;
    public GameObject adamastor;
    public GameObject characterModel;
    
    public enum Characters
    {
        holok,adamastor
    }
    [SyncVar]
    public Characters nextCharacter=Characters.adamastor;
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
            playerCharacter = character;
            isPlayer = true;

            switch (nextCharacter)
            {
                case Characters.holok:
                    holok.SetActive(true);
                    character.animator=holok.GetComponentInChildren<Animator>();
                    nextCharacter = Characters.adamastor;
                    characterModel = holok;
                    break;
                case Characters.adamastor:
                    adamastor.SetActive(true);
                    character.animator = adamastor.GetComponentInChildren<Animator>();
                    characterModel = adamastor;
                    break;
                default:
                    break;
            }
        }

    }
}

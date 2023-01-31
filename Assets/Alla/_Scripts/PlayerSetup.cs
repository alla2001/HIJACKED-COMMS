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
    public GameObject sasha;
    public GameObject characterModel;
    [SyncVar]
    public GameManager.Characters assosiatedChar = GameManager.Characters.holok;
    private void OnConnectedToServer()
    {
        
    }

    private void Awake()
    {
        character= GetComponent<Character>();
       
    }
  
    public void NextCharacterCommand(GameManager.Characters chara)
    {
        RefrenceManager.gameManager.nextCharacter = chara;
      
    }
  
  
    private void Start()
    {
        if (netIdentity.isOwned)
        {
            InputManager.instance.clicker.assigendCharacter = character;
            InputManager.instance.moveTheCamera.assosiatedCharacter = character;
            playerCharacter = character;
            isPlayer = true;

            switch (
        RefrenceManager.gameManager.nextCharacter)
            {
                case GameManager.Characters.holok:
                    assosiatedChar = GameManager.Characters.holok;
                    holok.SetActive(true);
                    character.animator=holok.GetComponentInChildren<Animator>();
                    NextCharacterCommand(GameManager.Characters.adamastor);
                    characterModel = holok;
                    break;
                case GameManager.Characters.adamastor:
                    assosiatedChar = GameManager.Characters.adamastor;
                    adamastor.SetActive(true);
                    character.animator = adamastor.GetComponentInChildren<Animator>();
                    characterModel = adamastor;
                    NextCharacterCommand(GameManager.Characters.sasha);
                    break;
                case GameManager.Characters.sasha:
                    assosiatedChar = GameManager.Characters.sasha;
                    sasha.SetActive(true);
                    character.animator = sasha.GetComponentInChildren<Animator>();
                    characterModel = sasha;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (
       assosiatedChar)
            {
                case GameManager.Characters.holok:
                  
                    holok.SetActive(true);
                    character.animator = holok.GetComponentInChildren<Animator>();
                 
                    characterModel = holok;
                    break;
                case GameManager.Characters.adamastor:
               
                    adamastor.SetActive(true);
                    character.animator = adamastor.GetComponentInChildren<Animator>();
                    characterModel = adamastor;
                    break;
                case GameManager.Characters.sasha:
               
                    sasha.SetActive(true);
                    character.animator = sasha.GetComponentInChildren<Animator>();
                    characterModel = sasha;
                    break;
                default:
                    break;
            }
        }

    }
}

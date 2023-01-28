using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneBooth : Interactable
{

    public override void Interact()
    {
        print("Telephone booth");
        RadioControler.instance.ChangeSong();
    }
    public override void PreInteract()
    {
        RefrenceManager.musicManager.radioUI.SetActive(true);
    }
}

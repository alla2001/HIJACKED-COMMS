using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneBooth : Interactable
{

    public override void Interact()
    {
        RefrenceManager.musicManager.radioUI.SetActive(true);
    }

}

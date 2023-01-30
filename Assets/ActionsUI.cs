using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionsUI : MonoBehaviour 
{ 
    private void Start()
    {
        RefrenceManager.gameManager.startPlanning += OnPlanning;
        RefrenceManager.gameManager.startPlaying += OnPlay;
    }

    public void OnPlay()
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b,0.15f);
        }
        foreach (Button btn in GetComponentsInChildren<Button>())
        {
            btn.enabled=false;
        }
    }
    public void OnPlanning()
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
        foreach (Button btn in GetComponentsInChildren<Button>())
        {
            btn.enabled = true;
        }
    }
}

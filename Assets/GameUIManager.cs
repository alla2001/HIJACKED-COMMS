using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
{
    public ActionsUI gameBtns;
    public ActionsUI actionBtns;

    public void UnHilightActionBtns()
    {
        actionBtns.OnPlay();
    }
    public void HilightActionBtns()
    {
        actionBtns.OnPlanning();
    }
    public void UnHilightGameBtns()
    {
        gameBtns.OnPlay();
    }
    public void HilightGameBtns()
    {
        gameBtns.OnPlanning();
    }
}

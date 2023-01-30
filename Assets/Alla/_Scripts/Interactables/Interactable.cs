using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : GridObject
{
  
    public static List<Interactable> interactables = new List<Interactable>();
    public static Interactable IsInteractable(Vector2Int pos)
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable.posOnGrid == pos)
            {
                return interactable;
            }
        }
        return null;
    }



    public enum Type
    {
        PhoneBooth
    }

    public Type interactableType;
    public virtual void Interact(Character character)
    {

    }
    public virtual void PreInteract(Character character)
    {

    }
    private void Awake()
    {
        interactables.Add(this);
    }
    private void OnDestroy()
    {
        interactables.Remove(this);
    }
    private void Start()
    {
        base.Start();

       
        GridShaderBinder.gridHilights.Add(new GridHilight { points = { posOnGrid }, color = new Color(0.1f, 1, 0.1f, 1) });

    }
}

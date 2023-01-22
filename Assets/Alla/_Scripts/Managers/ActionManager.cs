using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ActionManager : SingletonMonoBehaviour<ActionManager>
{
	
	private GameAction selectedAction;
	public GameObject actionUIPrefab;
	public Transform actionsListHolder;
	Actions currentActionType;
	public enum Actions
	{
		Move,Shoot
	}
    private void Start()
    {
		RefrenceManager.gameManager.startPlanning += () => 
		{ 
			selectedAction = null;
			
		};
		RefrenceManager.gameManager.startPlaying+= () => 
		{
			if (selectedAction != null) selectedAction.UnHilight(); 
			selectedAction = null; 
		
		
		};

	}
	public void SelectAction(Actions action)
	{
		if (RefrenceManager.gameManager.currentPhase != GameManager.GamePhase.Planning) return;

		currentActionType = action;
		switch (action)
		{

			case Actions.Move:
				selectedAction = new Move();
				break;
				case Actions.Shoot:
				selectedAction = new Shoot();
				break;
		}
		
	}
	public void RemoveAction(GameActionCardUI cardUI)
	{
		UnAssigneAction(cardUI.owner, cardUI.action);
	}
	public void SelectiobActionButton(int action)
    {
	

		SelectAction((Actions)action);
		
	}
	public void HighLightSelectedAction(Character character, Vector2Int selecetedCell)
	{
		if (selectedAction == null) return;
		selectedAction.Hilight(character,selecetedCell);
    }
	public void UnHighLightSelectedAction()
	{
		if (selectedAction == null) return;
		selectedAction.UnHilight();
	}
	public bool AssigneSelectedAction(Character character,Vector2Int selecetedCell)
	{
		Debug.Log("Hilight");
		if (selectedAction == null) return false;
		selectedAction.UnHilight();
		if (RefrenceManager.gameManager.currentPhase != GameManager.GamePhase.Planning) return false;
		if (character.actionPointsLeft <= 0) return false;
		string name ="action";
		if (selectedAction.CanAssigne(character, selecetedCell))
		{
            switch (currentActionType)
            {
                case Actions.Move:
					Move move = selectedAction as Move;
					move.startPosition = character.ghost.posOnGrid;
					move.targetPosition = selecetedCell;
					name = "Move";

					break;
                case Actions.Shoot:
					Shoot shoot = selectedAction as Shoot;
					shoot.targetCell = selecetedCell;
					name = "Shoot";

					break;
                default:
                    break;
            }
            character.AddAction(selectedAction);
		
			GameObject newAction = Instantiate(actionUIPrefab, actionsListHolder);
			newAction.GetComponentInChildren<TextMeshProUGUI>().text = name ;
			
			GameActionCardUI actionCard = newAction.GetComponent<GameActionCardUI>();
			actionCard.action = selectedAction;
			actionCard.owner = character;
			selectedAction = null;

			return true;
		}

		return false;
	}
   
    public bool UnAssigneAction(Character character, GameAction action)
    {
		if (RefrenceManager.gameManager.currentPhase != GameManager.GamePhase.Planning) return false;
		character.RemoveAction(action);
		return true;
    }
}

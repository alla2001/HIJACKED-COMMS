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
	public static List<GameAction> actionHilights = new List<GameAction>();
	public enum Actions
	{
		Move,Shoot,Guard
	}
    private void Start()
    {
		RefrenceManager.gameManager.startPlanning += () => 
		{

			if (selectedAction != null) selectedAction.UnHilight();
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
		if (selectedAction != null) selectedAction.UnHilight();

		currentActionType = action;
		switch (action)
		{

			case Actions.Move:
				selectedAction = new Move();
				
				break;
				case Actions.Shoot:
				selectedAction = new Shoot();
				break;
			case Actions.Guard:
				selectedAction= new Guard();
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
		selectedAction.SelectionHilight(character);
	}
	public void UnHighLightSelectedAction()
	{
		if (selectedAction == null) return;
		selectedAction.UnHilight();
		selectedAction.SelectionUnHilight();
	}
	public bool AssigneSelectedAction(Character character,Vector2Int selecetedCell)
	{
		
		if (selectedAction == null) return false;
		selectedAction.SelectionUnHilight();
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
					
					character.AddActionMove(move);


					break;

                case Actions.Shoot:
					Shoot shoot = selectedAction as Shoot;
					shoot.targetCell = selecetedCell;
					name = "Shoot";
			
					character.AddActionShoot(shoot);
					break;

				case Actions.Guard:

					Guard guard = selectedAction as Guard;
				
					name = "Guard";
					character.AddActionGuard(guard);

                    
						
					
					break;
				default:
                    break;
            }
			character.InitilizeAction(selectedAction);
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
		character.RemoveAction();
		return true;
    }

	public void ClearActions()
    {
		if (RefrenceManager.gameManager.currentPhase != GameManager.GamePhase.Planning) return;
		PlayerSetup.playerCharacter.ClearActions();

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTheCamera : MonoBehaviour
{
	public Camera assosiatedCamera;
	public Character assosiatedCharacter;

	public float moveSpeed;
	[Range(0.01f,0.03f)]
	public float scrollSpeed;
	private int currentActionPoint;
	[HideInInspector]private Vector2Int selectedCell;
	Vector2 moveValue;
	private void Start()
	{

		InputManager.instance.InputMap.Camera.Scroll.performed += (i) => { assosiatedCamera.transform.Translate(0, 0, i.ReadValue<Vector2>().y * scrollSpeed); };

		InputManager.instance.InputMap.Camera.LockCamera.performed += (i) =>
		{
			assosiatedCamera.transform.position = new Vector3(assosiatedCamera.transform.position.x, PlayersManager.instance.defautlCameraHeight, assosiatedCamera.transform.position.z);
		};
		
	}
    private void Update()
    {
		moveValue =
		InputManager.instance.InputMap.Camera.Move.ReadValue<Vector2>();
		assosiatedCamera.transform.Translate(moveSpeed*moveValue.x*Time.deltaTime,0, moveSpeed*moveValue.y* Time.deltaTime,Space.World);
		
		
    }
    public void AddAction()
	{
		if (currentActionPoint - 1 > 0)
		{
			ActionManager.instance.AssigneSelectedAction(assosiatedCharacter,selectedCell);
		}
		currentActionPoint--;
	}
	public void RemoveAction(GameAction action)
	{
		if (currentActionPoint - 1 > 0)
		{

		}
		currentActionPoint++;
	}
}
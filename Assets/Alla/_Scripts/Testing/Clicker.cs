using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Clicker : MonoBehaviour
{
	public LayerMask groundLayer;
	public Character assigendCharacter;
	private Vector3 _hitpoint;

	private bool pingMode;
	private void Start()
	{

		InputManager.instance.InputMap.Camera.Click.performed += (i) =>
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 10000f, groundLayer))
			{
				Vector2Int pos;

				if(GridManager.instance.WorldToGrid(hit.point,out pos))
				{
					if (pingMode)
					{
						RefrenceManager.notificationManager.PingCell(pos, Color.red);

                    }
                    else
                    {
						ActionManager.instance.AssigneSelectedAction(assigendCharacter, pos);
					}
				
				}
			
			
				//assigendCharacter.MoveTo(hit.point);
				_hitpoint = hit.point;
			}
		};
	}

	private void Update()
	{
		pingMode = InputManager.instance.InputMap.Camera.PingMode.ReadValue<float>()>0;
	}
	Vector2Int lastpos;
	private void FixedUpdate()
    {
		RaycastHit hit;
		
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 10000f, groundLayer))
		{
			Vector2Int pos;
			if (GridManager.instance.WorldToGrid(hit.point, out pos))
			{
			
				GridShaderBinder.instance.cursorPos = pos;
				ActionManager.instance.HighLightSelectedAction(assigendCharacter, pos);
				

            }
            else
            {
				GridShaderBinder.instance.cursorPos = new Vector2Int(-1, -1);
				ActionManager.instance.UnHighLightSelectedAction();
			}
           
			_hitpoint = hit.point;
			return;
		}
		GridShaderBinder.instance.cursorPos = new Vector2Int(-1, -1);
		ActionManager.instance.UnHighLightSelectedAction();
	}

    private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(_hitpoint, 0.34f);
	}
}
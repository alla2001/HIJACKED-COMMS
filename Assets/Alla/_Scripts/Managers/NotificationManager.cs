using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;	
public class NotificationManager : NetworkBehaviour
{
    private void Awake()
    {
		if (RefrenceManager.notificationManager == null)
			RefrenceManager.notificationManager = this;
		else Destroy(null);
    }
    [SerializeField]private GameObject notificationPrefab;
	[SerializeField] private GameObject pingPrefab;
	[Command(requiresAuthority = false)]
	public void NotfyOnCell(Vector2Int cell, string text, Color color)
	{
		GameObject notification = Instantiate(notificationPrefab, GridManager.instance.GridToWorld(cell), Quaternion.identity);
		notification.GetComponent<NotificationUI>().SetText(text, color);

	}
	[Command(requiresAuthority =false)]
	public void PingCell(Vector2Int cell, Color color)
	{

        if (isServer)
        {
			PingCellServer(cell, color);

		}

	}
	[ClientRpc]
	private void PingCellServer(Vector2Int cell, Color color)
    {
		GameObject ping = Instantiate(pingPrefab, GridManager.instance.GridToWorld(cell), Quaternion.identity);
	}
}

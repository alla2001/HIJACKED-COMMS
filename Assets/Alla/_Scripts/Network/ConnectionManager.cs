using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
public class ConnectionManager : SingletonMonoBehaviour<ConnectionManager>
{
    public TMP_InputField inputField;
    public NetworkManager networkManager;
    public void Connect()
    {
        networkManager.networkAddress = inputField.text;
        networkManager.StartClient();
    }
}

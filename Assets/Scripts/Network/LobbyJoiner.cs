using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LobbyJoiner : MonoBehaviour
{
    [SerializeField] 
    private TMP_InputField m_ipAddressInputField;

    public UnityEvent OnAttemptedJoin;
    
    public void JoinLobby()
    {
        if(NetworkClient.active)
            return;
        
        OnAttemptedJoin?.Invoke();
        
        StartCoroutine(JoinLobbyAsync());
    }

    public IEnumerator JoinLobbyAsync()
    {
        yield return 0;
        
        NetworkManagerCustom networkManager = NetworkManagerCustom.Instance;

        string ip = m_ipAddressInputField.text;

        networkManager.networkAddress = ip;

        networkManager.StartClient();
    }
}

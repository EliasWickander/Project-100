using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerCustom : NetworkManager
{
    public static NetworkManagerCustom Instance => singleton as NetworkManagerCustom;
    
    [Scene]
    [SerializeField]
    private string m_menuScene;

    [Header("Lobby Room")] 
    [SerializeField]
    private LobbyRoomPlayer m_roomPlayerPrefab;

    public static event Action OnClientConnectionAttemptEvent;
    public static event Action OnClientConnectedEvent;
    public static event Action OnClientDisconnectedEvent;
    public static event Action<TransportError> OnClientErrorEvent;
    
    public override void OnStartServer()
    {
        base.OnStartServer();

        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameObject[] spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        
        foreach(GameObject prefab in spawnablePrefabs)
            NetworkClient.RegisterPrefab(prefab);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
        OnClientConnectedEvent?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        
        OnClientDisconnectedEvent?.Invoke();
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);
        
        OnClientErrorEvent?.Invoke(error);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != m_menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == m_menuScene)
        {
            LobbyRoomPlayer roomPlayerInstance = Instantiate(m_roomPlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }
    
    public void JoinLobby()
    {
        if(NetworkClient.active)
            return;
        
        OnClientConnectionAttemptEvent?.Invoke();
        
        StartCoroutine(JoinLobbyAsync());
    }

    public void JoinLobby(string ip)
    {
        if(NetworkClient.active)
            return;

        networkAddress = ip;
        OnClientConnectionAttemptEvent?.Invoke();

        StartCoroutine(JoinLobbyAsync());
    }
    
    //Make sure to call StartClient after one frame so event has time to trigger
    private IEnumerator JoinLobbyAsync()
    {
        yield return new WaitForSeconds(0.1f);

        StartClient();
    }
}
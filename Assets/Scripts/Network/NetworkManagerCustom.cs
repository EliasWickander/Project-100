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

    public int m_minPlayers = 2;
    
    [Scene]
    [SerializeField]
    private string m_menuScene;

    [Header("Lobby Room")] 
    [SerializeField]
    private LobbyRoomPlayer m_roomPlayerPrefab;

    public OnStartHostAttemptEvent m_onStartHostAttemptEvent;
    public OnStartHostEvent m_onStartHostEvent;
    public OnClientConnectionAttemptEvent m_onClientConnectionAttemptEvent;
    public OnClientConnectedEvent m_onClientConnectedEvent;
    public OnClientDisconnectedEvent m_onClientDisconnectedEvent;
    public OnClientErrorEvent m_onClientErrorEvent;

    private List<LobbyRoomPlayer> m_roomPlayers = new List<LobbyRoomPlayer>();

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

        if(m_onClientConnectedEvent)
            m_onClientConnectedEvent.Raise();;
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        if(m_onClientDisconnectedEvent)
            m_onClientDisconnectedEvent.Raise();
    }

    public override void OnClientError(TransportError error, string reason)
    {
        base.OnClientError(error, reason);

        if(m_onClientErrorEvent)
            m_onClientErrorEvent.Raise(error);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != m_menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(conn.identity == null)
            return;

        Debug.Log("disconnect");
        LobbyRoomPlayer player = conn.identity.GetComponent<LobbyRoomPlayer>();

        m_roomPlayers.Remove(player);
        
        base.OnServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().path == m_menuScene)
        {
            LobbyRoomPlayer roomPlayerInstance = Instantiate(m_roomPlayerPrefab);

            roomPlayerInstance.IsLeader = m_roomPlayers.Count == 0;
            
            Debug.Log("add " + m_roomPlayers.Count);
            m_roomPlayers.Add(roomPlayerInstance);
            
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnStopServer()
    {
        m_roomPlayers.Clear();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        
        if(m_onStartHostEvent)
            m_onStartHostEvent.Raise();
    }

    public void HostLobby()
    {
        if(NetworkServer.active || NetworkClient.active)
            return;

        if(m_onStartHostAttemptEvent)
            m_onStartHostAttemptEvent.Raise();

        StartCoroutine(HostLobbyAsync());
    }

    public IEnumerator HostLobbyAsync()
    {
        yield return new WaitForSeconds(0.1f);

        StartHost();
    }
    
    public void JoinLobby()
    {
        if(NetworkClient.active)
            return;
        
        m_onClientConnectionAttemptEvent.Raise();
        
        StartCoroutine(JoinLobbyAsync());
    }

    public void JoinLobby(string ip)
    {
        if(NetworkClient.active)
            return;

        networkAddress = ip;
        
        if(m_onClientConnectionAttemptEvent)
            m_onClientConnectionAttemptEvent.Raise();

        StartCoroutine(JoinLobbyAsync());
    }
    
    //Make sure to call StartClient after one frame so event has time to trigger
    private IEnumerator JoinLobbyAsync()
    {
        yield return new WaitForSeconds(0.1f);

        StartClient();
    }
}
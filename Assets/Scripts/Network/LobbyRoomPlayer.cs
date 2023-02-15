using Mirror;

public class LobbyRoomPlayer : NetworkBehaviour
{
    public LobbyLeaderSetEvent m_lobbyLeaderSetEvent;
    private bool m_isLeader = false;
    public bool IsLeader
    {
        get
        {
            return m_isLeader;
        }
        set
        {
            m_isLeader = value;
            
            if(m_lobbyLeaderSetEvent)
                m_lobbyLeaderSetEvent.Raise(value);
        }
    }
}
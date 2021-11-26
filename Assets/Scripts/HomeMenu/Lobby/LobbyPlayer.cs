using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    private Lobby lobby = this.gameobject.getComponent<Lobby>();
    public override void OnStartClient()
    {

    }

}

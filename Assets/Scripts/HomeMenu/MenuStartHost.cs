using Mirror;
using UnityEngine;

public class MenuStartHost : MonoBehaviour
{
    public void StartHost()
    {
        Debug.Log("[MENU] Starting host...");
        NetworkManager.singleton.StartHost();
    }
}

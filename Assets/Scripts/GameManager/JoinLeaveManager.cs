using Mirror;
using UnityEngine;

public class JoinLeaveManager : MonoBehaviour
{
    private NetworkManager networkManager;

    public void Join(string ip, string username)
    {
        networkManager = GetComponent<NetworkManager>();

        Debug.Log("[JoinLeaveManager] Trying to join server: " + ip + " as " + username);

        networkManager.StartClient();
        networkManager.networkAddress = ip;
        Debug.Log("[JoinLeaveManager] " + username + " joined the server: " + ip);
    }
}

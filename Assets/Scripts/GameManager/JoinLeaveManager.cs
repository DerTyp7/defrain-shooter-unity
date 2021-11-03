using Mirror;
using UnityEngine;

public class JoinLeaveManager : MonoBehaviour
{
    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    public void Join(string ip, string username)
    {
        

        Debug.Log("[JoinLeaveManager] Trying to join server: " + ip + " as " + username);

        networkManager.StartClient();
        networkManager.networkAddress = ip;
        Debug.Log("[JoinLeaveManager] " + username + " joined the server: " + ip);
    }

    public void Host()
    {
        networkManager.StartHost();
    }
}

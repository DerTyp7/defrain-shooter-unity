using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuStartClient : MonoBehaviour
{
    [SerializeField] private TMP_InputField IpInput;
    public void StartClient()
    {
        Debug.Log("[MENU] Starting client...");
        
        NetworkManager.singleton.networkAddress = IpInput.text;
        NetworkManager.singleton.StartClient();
    }
}

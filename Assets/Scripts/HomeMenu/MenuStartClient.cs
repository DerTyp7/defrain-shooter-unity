using Mirror;
using UnityEngine;
using TMPro;

public class MenuStartClient : MonoBehaviour
{
    [SerializeField] private TMP_InputField IpInput;
    [SerializeField] private TMP_InputField UsernameInput;
    public void StartClient()
    {
        if(UsernameInput.text != "")
        {
            Debug.Log("[MENU] Starting client...");

            GameObject.FindGameObjectWithTag("VariableSaver").GetComponent<VariableSaver>().username = UsernameInput.text;

            NetworkManager.singleton.networkAddress = IpInput.text;
            if(IpInput.text != "")
            {
                NetworkManager.singleton.StartClient();
            }
        }
    }
}

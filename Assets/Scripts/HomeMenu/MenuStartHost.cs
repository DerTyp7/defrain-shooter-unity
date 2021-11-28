using Mirror;
using UnityEngine;
using TMPro;

public class MenuStartHost : MonoBehaviour
{
    [SerializeField] private TMP_InputField UsernameInput;
    public void StartHost()
    {
        if (UsernameInput.text != null)
        {
            Debug.Log("[MENU] Starting host...");
            GameObject.FindGameObjectWithTag("VariableSaver").GetComponent<VariableSaver>().username = UsernameInput.text;
            NetworkManager.singleton.StartHost();
        }
    }
}

using Mirror;
using UnityEngine.UI;
using UnityEngine;

public class HostBtnScript : MonoBehaviour
{
    private GameObject GameManager;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HostServer);
        GameManager = GameObject.Find("GameManager");
    }

    public void HostServer()
    {
        GameManager.GetComponent<NetworkManager>().StartHost();
    }
}

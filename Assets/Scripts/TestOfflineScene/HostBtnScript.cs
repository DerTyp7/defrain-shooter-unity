using Mirror;
using UnityEngine.UI;
using UnityEngine;

public class HostBtnScript : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HostServer);
    }

    public void HostServer()
    {
        NetworkClient.ConnectHost();
    }
}

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
        GameObject.Find("GameManager").GetComponent<JoinLeaveManager>().Host();
    }
}

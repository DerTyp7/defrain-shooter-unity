using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinBtnScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputIp;
    [SerializeField] private TMP_InputField inputUsername;
    private GameObject GameManager;
    private NetworkManager networkManager;
    public GameObject localPlayer;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(JoinServer);
        GameManager = GameObject.Find("GameManager");
        networkManager = GameManager.GetComponent<NetworkManager>();
    }

    public void JoinServer()
    {
        networkManager.StartClient();
        networkManager.networkAddress = inputIp.text;
        
        
    }
}

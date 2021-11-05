using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinBtnScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputIp;
    [SerializeField] private TMP_InputField inputUsername;

    private JoinLeaveManager joinLeaveManager;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(JoinServer);

        joinLeaveManager = GameObject.Find("GameManager").GetComponent<JoinLeaveManager>();
    }

    public void JoinServer()
    {
        NetworkClient.Connect(inputIp.text);
        joinLeaveManager.Join(inputIp.text, inputUsername.text);        
    }
}

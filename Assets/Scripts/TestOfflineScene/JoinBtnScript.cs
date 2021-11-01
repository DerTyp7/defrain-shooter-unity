using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinBtnScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputIp;
    [SerializeField] private TMP_InputField inputUsername;
<<<<<<< Updated upstream
=======
    private JoinLeaveManager joinLeaveManager;
>>>>>>> Stashed changes

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(JoinServer);
<<<<<<< Updated upstream
=======
        joinLeaveManager = GameObject.Find("GameManager").GetComponent<JoinLeaveManager>();
>>>>>>> Stashed changes
    }

    public void JoinServer()
    {
<<<<<<< Updated upstream
        NetworkClient.Connect(inputIp.text);
=======
        joinLeaveManager.Join(inputIp.text, inputUsername.text);        
>>>>>>> Stashed changes
    }
}

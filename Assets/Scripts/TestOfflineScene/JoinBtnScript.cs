using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinBtnScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputIp;
    [SerializeField] private TMP_InputField inputUsername;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(JoinServer);
    }

    public void JoinServer()
    {
        NetworkClient.Connect(inputIp.text);
    }
}

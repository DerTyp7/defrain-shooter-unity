using UnityEngine;

public class VariableSaver : MonoBehaviour
{
    public string username;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

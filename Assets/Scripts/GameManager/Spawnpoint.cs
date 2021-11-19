using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField] public int teamID = 0;
    [SerializeField] public bool available = true;
    [SerializeField] public bool blocked = false;
    public Vector3 position;
    private void Awake()
    {
        position = this.transform.position;
    }
    void Start()
    {
        
    }
    IEnumerator disableEnum(float time) 
    {
        available = false;
        yield return new WaitForSeconds(time);
        available = true;
    }
    public void disableForSeconds(float time) 
    {
        StartCoroutine(disableEnum(time));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

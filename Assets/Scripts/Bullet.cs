using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int seconds = 30;
    private void Start()
    {
        StartCoroutine("dest"); 
    }
    IEnumerator dest() 
    {
       yield return new WaitForSeconds(seconds);
        Destroy(this);
    }
}

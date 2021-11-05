using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Reload : NetworkBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetButton("Reload"))
            {
                
            }
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerColor : NetworkBehaviour
{
    [SyncVar(hook = "SetColor")]
    public Color color;
    public Renderer renderer;

    void SetColor(Color oldColor, Color newColor)
    {
        if (color == null)
        {
            color = renderer.material.color;
        }

        renderer.material.color = newColor;
        color = newColor;

    }

    public override void OnStartClient()
    {
        renderer.material.color = color;
    }
}

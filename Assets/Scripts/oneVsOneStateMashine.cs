using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneVsOneStateMashine : MonoBehaviour
{
    public int state = 0;
    public const int TOTALSTATES = 4;
    void StateMashineRound() 
    {
        switch (state)
        {
            case 0:
                //respawn players
                break;
            case 1:
                //Let the players walk and kill each other
                //if one team gets killed    go to next state
                break;
            case 2:
                //stop player movement
                break;
            case 3:
                //show score board
                break;
            default:
                break;

        }
    }

    void switchState() 
    {
        state++;
        if (state >= TOTALSTATES)
        {
            state = 0;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    private string name;
    private int score;

    public Team(string _name, int _score)
    {
        name = _name;
        score = _score;

        Debug.Log(name + " Team Created!");
    }

    public string GetTeamName()
    {
        return name;
    }
}

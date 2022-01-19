using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun 
{
    // All types of guns
    public enum types
    {
        Rifle, Pistole, Melee, Grenade
    }
    // Type of gun
    private types type;
    // Name of gun
    private string name;
    // Damage of gun
    private float damage;
    // Strength of throw
    private float dropForce;

    //private Animator gunAnimator;
    //private Transform gunRightREF;
    //private Transform gunLeftREF;

    // Constructor
    public Gun()
    {
        this.type = types.Rifle;
        this.name = "Weapon";
        this.damage = 0f;
        this.dropForce = 10f;
    }
    public Gun(types type, string name, float damage, float dropforce)
    {
        this.type = type;
        this.name = name;
        this.damage = damage;
        this.dropForce = dropforce;
    }

    // Getter
    private types Type { get => type; }
    private string Name { get => name; }
    private float Damage { get => damage; }
    public float DropForce { get => dropForce; }
}

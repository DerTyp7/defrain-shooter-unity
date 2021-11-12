using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum weaponKinds
    {
        Rifle, Pistole, Knife, Granade
    }
    [SerializeField] weaponKinds weaponKind;
    [SerializeField] bool active = false;
    [SerializeField] int damage = 0;
    [SerializeField] float firerate = 0;
    [SerializeField] int roundsPerShot = 1;
    [SerializeField] float recoilStrength = 0;
    [SerializeField] int currentAmmunition = 0;
    [SerializeField] int magazinSize = 0;
    [SerializeField] int totalAmmunition = 0;
    [SerializeField] GameObject bulletExit;
    [SerializeField] bool allowAction = true;

    public bool Active { get => active; set => active = value; }
    public weaponKinds WeaponKind { get => weaponKind; set => weaponKind = value; }
    public int Damage { get => damage; set => damage = value; }
    public float Firerate { get => firerate; set => firerate = value; }
    public int RoundsPerShot { get => roundsPerShot; set => roundsPerShot = value; }
    public float RecoilStrength { get => recoilStrength; set => recoilStrength = value; }
    public int CurrentAmmunition { get => currentAmmunition; set => currentAmmunition = value; }
    public int MagazinSize { get => magazinSize; set => magazinSize = value; }
    public int TotalAmmunition { get => totalAmmunition; set => totalAmmunition = value; }
    public GameObject BulletExit { get => bulletExit; set => bulletExit = value; }
    public bool AllowAction { get => allowAction; set => allowAction = value; }

    private void Start()
    {
        CurrentAmmunition = MagazinSize;
    }

}

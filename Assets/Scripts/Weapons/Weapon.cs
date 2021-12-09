using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum weaponKinds
    {
        Rifle, Pistole, Knife, Grenade
    }
    [Header("Weapon Info")]
    [SerializeField] weaponKinds weaponKind;
    [SerializeField] float dropForce = 10f;
    [SerializeField] int damage = 0;
    [SerializeField] float firerate = 0;
    [SerializeField] float recoilStrength = 0;
    [SerializeField] int currentAmmunition = 0;
    [SerializeField] int magazinSize = 0;
    [SerializeField] int totalAmmunition = 0;
    [SerializeField] GameObject bulletExit;
    [SerializeField] bool allowAction = true;
    [Header("")]
    [SerializeField] Animator weaponAnimator;
    [SerializeField] Transform gunRightREF;
    [SerializeField] Transform gunLeftREF;
    //[Header("Grenade")]
    private bool hasBeenThrown = false;

    public weaponKinds WeaponKind { get => weaponKind; }
    public float DropForce { get => dropForce; set => dropForce = value; }
    public int Damage { get => damage; set => damage = value; }
    public float Firerate { get => firerate; set => firerate = value; }
    public float RecoilStrength { get => recoilStrength; set => recoilStrength = value; }
    public int CurrentAmmunition { get => currentAmmunition; set => currentAmmunition = value; }
    public int MagazinSize { get => magazinSize; set => magazinSize = value; }
    public int TotalAmmunition { get => totalAmmunition; set => totalAmmunition = value; }
    public GameObject BulletExit { get => bulletExit; }
    public bool AllowAction { get => allowAction; set => allowAction = value; }
    public Animator WeaponAnimator { get => weaponAnimator; }
    public Transform GunLeftREF { get => gunLeftREF; }
    public Transform GunRightREF { get => gunRightREF; }
    public bool HasBeenThrown { get => hasBeenThrown; set => hasBeenThrown = value; }

    private void Start() {
        CurrentAmmunition = MagazinSize;
    }

}

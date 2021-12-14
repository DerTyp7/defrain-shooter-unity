using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class Shoot : NetworkBehaviour
{

    [SerializeField] WeaponManager weaponManager; // For throwing grenade
    [SerializeField] GameObject muzzle;
    [SerializeField] ProcedualAnimationController shootAnim;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] Camera mCamera;
    [SerializeField] bool limitAmmunition = true;
    [Header("Debug")]
    [SerializeField] bool showBullethole = true;
    [SerializeField] float bulletholeRadius = 0.2f;

    private Weapon weapon;
    private RaycastHit crosshairHitPoint;
    private Vector3 _pointDirection;
    private Quaternion _lookRotation;
    private Vector3 hitpos;
    private RaycastHit hit;
    private Ray ray;
    private bool updateCanvas = true;
    private int curAmmo = 1, totalAmmo = 1;

    public int CurAmmo { get => curAmmo; set => curAmmo = value; } // For DebugCanvas
    public int TotalAmmo { get => totalAmmo; set => totalAmmo = value; } // For DebugCanvas

    private void Start() {
        if (isServer) { 
            weapon = weaponHolder.GetComponent<Weapon>();
        }
        if (isLocalPlayer) {
            weapon = weaponHolder.GetComponent<Weapon>();
            curAmmo = weapon.CurrentAmmunition;
            totalAmmo = weapon.TotalAmmunition;
        }
    }

    private void Update() {
        if (isLocalPlayer) {
            if (updateCanvas) {
                curAmmo = weapon.CurrentAmmunition;
                totalAmmo = weapon.TotalAmmunition;
                updateCanvas = false;
            }
            if (Input.GetButtonDown("Fire")) { 
                updateCanvas = true;
                // If current weapon kind is a rifle or pistole
                string weaponKindString = weapon.WeaponKind.ToString();
                if(weaponKindString == "Rifle" || weaponKindString == "Pistole") {
                    if (weapon.AllowAction && weapon.CurrentAmmunition > 0) {
                        shootAnim.Recoil(0.1f);
                    }
                    // Shoot Weapon
                    CmdFireBullet(); 
                } // If current weapon kind is grenade
                else if(weaponKindString == "Grenade"){
                    // Throw Grenade
                    throwGrenade(); 
                } // If current weapon kind is kinfe
                else { 
                    // Throw hands (punch)
                }       
            }
            if (Input.GetButtonDown("Reload")) {
                updateCanvas = true;
                CmdReloadWeapon();
            }            
        }
    }

    private void throwGrenade() {
        Debug.Log("ThrowGrenade!");
        // Throws grenade with dropForce
        weapon.HasBeenThrown = true;
        // 3 -> Grenade index
        weaponManager.dropWeapon(weapon.DropForce, 3);
        weaponManager.switchWeapon(1);
    }

    [Command]
    private void CmdReloadWeapon() {
        if (weapon.AllowAction && limitAmmunition) {
            reloadWeapon(weapon);
        }
    }

    // [Command]
    // This code will be executed on the Server.
    private void CmdFireBullet() {
        ray = new Ray(mCamera.transform.position, mCamera.transform.forward); // Raycast from Camera
        if (Physics.Raycast(ray, out crosshairHitPoint, 5000f)) { // Check if Raycast is beyond 5000
            hitpos = crosshairHitPoint.point; // If hitpoint is under 5000
        } else {
            hitpos = mCamera.transform.position + mCamera.transform.forward * 5000; 
        }
        _pointDirection = hitpos - muzzle.transform.position;
        _lookRotation = Quaternion.LookRotation(_pointDirection);
        shootAnim.rotationMod[1] = Quaternion.RotateTowards(weaponHolder.transform.rotation, _lookRotation, 1f); // Point weapon to raycast hitpoint from camera
        
        if (weapon.AllowAction) { // If not reloading etc.
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit) && weapon.CurrentAmmunition > 0) { // Raycast from Bullet Exit Point to camera raycast 
                if (showBullethole) { bulletHole(GameObject.CreatePrimitive(PrimitiveType.Sphere), hit); } // Creates bullethole where raycast hits
                if (hit.transform.gameObject.GetComponent<Rigidbody>())
                {
                    hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(mCamera.transform.forward * weapon.HitForce);
                }
                if (hit.transform.gameObject.GetComponent<Player>() != null) { // If hit object is a player
                    Debug.Log("-->HIT PLAYER: " + hit.transform.name);
                    hit.transform.gameObject.GetComponent<Player>().RemoveHealth(weapon.Damage); 
                }
            }
            if (limitAmmunition) {
                subtractAmmunition(weapon); // Subtract Ammunition 
            }
            StartCoroutine(fireRate());
        }
    }

    void bulletHole(GameObject holeObject, RaycastHit hit) // Nur zum testen da
    {
        holeObject.transform.localScale = new Vector3(bulletholeRadius, bulletholeRadius, bulletholeRadius);
        holeObject.transform.position = hit.point;
    }


    IEnumerator fireRate() {
        weapon.AllowAction = false;
        yield return new WaitForSeconds(60f/weapon.Firerate); // Waits for firerate seconds
        weapon.AllowAction = true;
    }

    private bool subtractAmmunition(Weapon weapon) { // Subtracts Ammunition from weapon 
        if (weapon.CurrentAmmunition > 0) {
            weapon.CurrentAmmunition -= 1;
            Debug.Log(weapon.CurrentAmmunition + " / " + weapon.TotalAmmunition);
            return true;
        }
        return false;
    }

    public bool setWeapon(GameObject newWeapon) {
        Debug.Log("Switch weapon to: " + newWeapon.transform.name);
        weapon = newWeapon.GetComponent<Weapon>();
        curAmmo = weapon.CurrentAmmunition;
        totalAmmo = weapon.TotalAmmunition;
        muzzle = weapon.BulletExit;
        return true;
    }
    private bool reloadWeapon(Weapon weapon) {  // Reloads Ammunition from weapon 
        if (weapon.AllowAction && weapon.TotalAmmunition > 0) {
            weapon.AllowAction = false;
            int dif = weapon.MagazinSize - weapon.CurrentAmmunition;
            if (weapon.TotalAmmunition >= dif) {
                weapon.CurrentAmmunition += dif;
                weapon.TotalAmmunition -= dif;
            }
            else {
                weapon.CurrentAmmunition += weapon.TotalAmmunition;
                weapon.TotalAmmunition = 0;
            }
            weapon.AllowAction = true;
            Debug.Log("Reloaded");
            return true;
        }
        return false;
    }


}

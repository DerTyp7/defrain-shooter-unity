using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shoot : NetworkBehaviour
{
    [SerializeField] GameObject muzzle;
    [SerializeField] ShootAnimation shootAnim;
    [SerializeField] GameObject gunHoldPos;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject GunRotation;
    

    Ammunition ammunition;
    private Weapon weapon;
    private RaycastHit crosshairHitPoint;
    private Camera mCamera;
    private Vector3 _pointDirection;
    private Quaternion _lookRotation;
    private void Start()
    {
        
        if (isLocalPlayer)
        {
            mCamera = Camera.main;
            weapon = weaponHolder.GetComponent<Weapon>();
            shootAnim.OnSwitchWeapon(weapon.Firerate);
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {

            if (Input.GetButton("Fire"))
            {
                CmdFireBullet();
            }
            if (Input.GetButton("Reload"))
            {
                CmdReloadWeapon();
            }
        }
    }


    [Command]
    private void CmdReloadWeapon()
    {
        if (GetComponent<Ammunition>() != null && weapon.AllowAction)
        {
            GetComponent<Ammunition>().reloadWeapon(weapon);
        }
    }

   [Command]
    // This code will be executed on the Server.
    private void CmdFireBullet()
    {
        RaycastHit hit;
        Ray ray = new Ray(mCamera.transform.position, mCamera.transform.forward);
        Physics.Raycast(ray, out crosshairHitPoint, 5000f);
        
        if (crosshairHitPoint.distance != 0 && crosshairHitPoint.distance < 2) // Turning Weapon to shooting point
        {
            _pointDirection = crosshairHitPoint.point - muzzle.transform.position;
            _lookRotation = Quaternion.LookRotation(_pointDirection);
            GunRotation.transform.rotation = Quaternion.RotateTowards(GunRotation.transform.rotation, _lookRotation, 1f);
        }

        if (weapon.AllowAction) // shooting
        {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit) && weapon.CurrentAmmunition > 0)
            {
                Debug.DrawLine(muzzle.transform.position, hit.point);
                Debug.Log("Geshooted BITCH");
                if (hit.transform.gameObject.GetComponent<Player>() != null)
                {
                    Debug.Log("GETROFFEN------------------");
                    hit.transform.gameObject.GetComponent<Player>().RemoveHealth(20);
                }
            }
            if (GetComponent<Ammunition>() != null)
            {
                GetComponent<Ammunition>().subtractAmmunition(weapon);
            }
            StartCoroutine(fireRate());
        }
        
    }
    [Client]
    void shootAnimation()
    {
        shootAnim.recoil(gunHoldPos, 0.1f);
    }
    IEnumerator fireRate()
    {
        weapon.AllowAction = false;
        yield return new WaitForSeconds(60f/weapon.Firerate);
        weapon.AllowAction = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shoot : NetworkBehaviour
{
    [SerializeField] GameObject muzzle;
    [SerializeField] GameObject weaponHolder;
    private Weapon weapon;

    Ammunition ammunition;
    private RaycastHit crosshairHitPoint;
    private Camera mCamera;
    private Vector3 _pointDirection;
    private Quaternion _lookRotation;
    private void Start()
    {
        mCamera = Camera.main;
        weapon = weaponHolder.GetComponent<Weapon>();
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
        Physics.Raycast(mCamera.transform.position, mCamera.transform.forward, out crosshairHitPoint);
        Debug.DrawLine(mCamera.transform.position, crosshairHitPoint.point);
        _pointDirection = crosshairHitPoint.point - muzzle.transform.position;
        _lookRotation = Quaternion.LookRotation(_pointDirection);
        weapon.transform.rotation = Quaternion.RotateTowards(weapon.transform.rotation, _lookRotation, 1f);
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out RaycastHit hit) && weapon.CurrentAmmunition > 0)
        {
            Debug.DrawLine(muzzle.transform.position, hit.point);
        }

        if (weapon.AllowAction)
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
    
    IEnumerator fireRate()
    {
        weapon.AllowAction = false;
        yield return new WaitForSeconds(weapon.Firerate);
        weapon.AllowAction = true;
    }

}

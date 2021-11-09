using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shoot : NetworkBehaviour
{
    [SerializeField] GameObject muzzle;
    [SerializeField] ShootAnimation shootAnim;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject GunRotation;
    

    Ammunition ammunition;
    private Weapon weapon;
    private RaycastHit crosshairHitPoint;
    private Camera mCamera;
    private Vector3 _pointDirection;
    private Quaternion _lookRotation;
    private Vector3 hitpos;
    private RaycastHit hit;
    private Ray ray;
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
        Debug.Log("Test");
        if (isLocalPlayer)
        {
            if (Input.GetButtonDown("Fire"))
            {
                CmdFireBullet();
            }
            if (Input.GetButtonDown("Reload"))
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
        ray = new Ray(mCamera.transform.position, mCamera.transform.forward);
        if(Physics.Raycast(ray, out crosshairHitPoint, 5000f)){

            hitpos = crosshairHitPoint.point;
        }
        else
        {
            hitpos = mCamera.transform.position + mCamera.transform.forward * 5000;
        }
        _pointDirection = hitpos - muzzle.transform.position;
        _lookRotation = Quaternion.LookRotation(_pointDirection);
        GunRotation.transform.rotation = Quaternion.RotateTowards(GunRotation.transform.rotation, _lookRotation, 1f);

        if (weapon.AllowAction)
        {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit) && weapon.CurrentAmmunition > 0)
            {
                shootAnimation();
                Debug.DrawLine(muzzle.transform.position, hit.point);
                Debug.Log(hit.transform.name);
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
        shootAnim.recoil(0.1f);
    }
    IEnumerator fireRate()
    {
        weapon.AllowAction = false;
        yield return new WaitForSeconds(60f/weapon.Firerate);
        weapon.AllowAction = true;
    }

}

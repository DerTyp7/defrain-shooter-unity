using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] bool active = false;
    [SerializeField] float damage = 0;
    [SerializeField] int firerate = 0;
    [SerializeField] float recoilStrength = 0;
    [SerializeField] int currentAmmunition = 0;
    [SerializeField] int totalAmmunition = 0;
    [SerializeField] ParticleSystem flash;
    [SerializeField] ParticleSystem smoke;
    [SerializeField] GameObject bulletExit;
    [SerializeField] GameObject camera;

    public bool Active { get => active; set => active = value; }

    private void Start()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit))
        {
            transform.rotation = Quaternion.Euler(0f, hit.point.y, 0f);
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            fire();
        }
    }
    private void fire()
    {
        flash.Play();
        smoke.Play();
        RaycastHit hit; 
        
        if(Physics.Raycast(bulletExit.transform.position,bulletExit.transform.forward, out hit))
        {
            Debug.DrawLine(bulletExit.transform.position, hit.point);
            /*if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * 80);
            }
            Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));*/
        }
    }
}

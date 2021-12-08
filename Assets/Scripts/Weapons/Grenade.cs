using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] GameObject explodeParticle;
    [SerializeField] Weapon weapon; 
    private float countdown;

    void Start() {
        countdown = weapon.Timer;
    }

    void Update() {
        // If grenade has been thrown and countdown is over 0 and grenade has not exploded yet
        if (weapon.HasBeenThrown && !weapon.HasExploded) {
            // Decrease timer by 1 second
            countdown -= Time.deltaTime;
            // If countdown get to 0... BOOM!:
            if(countdown <= 0) {
                // Lets grenade explode
                Explode();
            }
        }
    }


    /* - Spawn explosion particles and add force to nearby objects - */
    private void Explode() {
        // Spawns explosion particle
        GameObject spawnedExplosion = Instantiate(explodeParticle, transform.position, transform.rotation);
        // Destroys explosion particle after on second
        Destroy(spawnedExplosion, 1);

        // Gets all collider that are in a sphere around the grenade
        Collider[] colliders = Physics.OverlapSphere(transform.position, weapon.GrenadeRadius);
        // Iterate over all colliders found in radius
        foreach(Collider nearbyObject in colliders) {
            // Check if nearby object is a Player
            if (nearbyObject.transform.gameObject.GetComponent<Player>()) {
                // Remove health from player
                nearbyObject.transform.gameObject.GetComponent<Player>().RemoveHealth(weapon.Damage);
            } else {
                // Get Rigidbody from nearby object and...
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                // if rigidbody exists...
                if (rb != null) {
                    // adds force to nearby objects
                    rb.AddExplosionForce(weapon.ExplosionForce, transform.position, weapon.GrenadeRadius);
                }
            }
        }
        weapon.HasExploded = true;
        // Destroys grenade
        Destroy(gameObject);
    }
}

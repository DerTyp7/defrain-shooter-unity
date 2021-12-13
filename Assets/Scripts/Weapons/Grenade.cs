using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [Header("Grenade Info")]
    [SerializeField] float timer = 2f;
    [SerializeField] float explosionForce = 500f;
    [SerializeField] float grenadeRadius = 3f;
    [SerializeField] bool hasExploded = false;
    [Tooltip("After how many seconds the explosion Gameobject gets deleted!")]
    [SerializeField] float lengthOfExplosion = 1;
    private float countdown;

    [SerializeField] float cameraShakeRadius = 6f;

    [SerializeField] GameObject explodeParticle;
    
    [SerializeField] Weapon weapon;

    [Header("Debug")]
    [SerializeField] bool showExplosion = true;

    void Start() {
        countdown = timer;
    }

    void Update() {
        // If grenade has been thrown and countdown is over 0 and grenade has not exploded yet
        if (weapon.HasBeenThrown && !hasExploded) {
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
        if (showExplosion) {
            // Spawns explosion particle
            GameObject spawnedExplosion = Instantiate(explodeParticle, transform.position, transform.rotation);
            // Destroys explosion particle after on second
            Destroy(spawnedExplosion, lengthOfExplosion);
        }
        StartCoroutine(cameraShake());
        // Coroutine for adding explosion force to nearby objects
        StartCoroutine(addExplosionForce());
        
        // Destroys grenade
        Destroy(gameObject);
    }

    IEnumerator cameraShake() {
        // Gets all collider that are in a sphere around the grenade
        Collider[] colliders = Physics.OverlapSphere(transform.position, cameraShakeRadius);
        // Iterate over all colliders found in radius
        foreach (Collider nearbyObject in colliders) {
            // Check if nearby object is a Player and if Collider is not a CharacterController (can be changed to CapsuleCollider)
            if (nearbyObject.GetComponent<Player>() && nearbyObject.GetType() != typeof(UnityEngine.CharacterController)) {
                // Starts camera shake on player
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                nearbyObject.GetComponent<ProcedualAnimationController>().cameraShake();
            }
        }
        yield return null;
    }

    IEnumerator addExplosionForce() {
        // Gets all collider that are in a sphere around the grenade
        Collider[] colliders = Physics.OverlapSphere(transform.position, grenadeRadius);
        // Iterate over all colliders found in radius
        foreach (Collider nearbyObject in colliders) {
            // Check if nearby object is a Player and if Collider is not a CharacterController (can be changed to CapsuleCollider)
            if (nearbyObject.GetComponent<Player>() && nearbyObject.GetType() != typeof(UnityEngine.CharacterController)) {
                // Remove health from player
                nearbyObject.GetComponent<Player>().RemoveHealth(weapon.Damage);
            } else {
                // Get Rigidbody from nearby object and...
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                // if rigidbody exists...
                if (rb != null) {
                    // adds force to nearby objects
                    rb.AddExplosionForce(explosionForce, transform.position, grenadeRadius);
                }
            }
        }
        hasExploded = true;
        yield return null;
    }
}

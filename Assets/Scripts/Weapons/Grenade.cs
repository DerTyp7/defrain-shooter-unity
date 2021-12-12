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
    [Header("Camera Shake Info")] // NOT WOKRING BECAUSE THE CAMERA IS FIXED IN PLACE
    [SerializeField] bool cameraShakeActive = true;
    [SerializeField] float cameraShakeRadius = 6f;
    [SerializeField] float cameraShakeDuration = 1f;
    [SerializeField] AnimationCurve cameraShakeCurve;

    [Header("Explosion GameObject")]
    [SerializeField] GameObject explodeParticle;
    
    [Header("Scripts")]
    [SerializeField] Weapon weapon;

    [Header("Debug")]
    [SerializeField] bool showExplosion = true;

    // To change it from other scripts
    public bool CameraShakeActive { get => cameraShakeActive; set => cameraShakeActive = value; }

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

        if (cameraShakeActive) {
            // Coroutine for camera shake to nearby Players
            StartCoroutine(cameraShake());
        }
        // Coroutine for adding explosion force to nearby objects
        StartCoroutine(addExplosionForce());
        
        // Destroys grenade
        Destroy(gameObject);
    }

    IEnumerator cameraShake() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, cameraShakeRadius);
        foreach(Collider nearbyObject in colliders){
            if (nearbyObject.GetComponent<Player>() && nearbyObject.GetType() != typeof(UnityEngine.CharacterController)) {
                // Start coroutine that shakes the camera
                StartCoroutine(shaking(nearbyObject));
            }
        }
        yield return null;
    }

    IEnumerator shaking(Collider obj) {
        // Getting neck from player
        GameObject neck = obj.GetComponent<Player>().PlayerNeck;
        Vector3 startPos = neck.transform.position;
        float elapsedTime = 0f;
        while(elapsedTime < cameraShakeDuration) {
            elapsedTime += Time.deltaTime;
            float strength = cameraShakeCurve.Evaluate(elapsedTime / cameraShakeDuration);
            neck.transform.position = startPos + Random.insideUnitSphere * strength;
        }
        neck.transform.position = startPos;
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

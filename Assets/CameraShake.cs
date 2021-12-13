using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Header("Camera Shake Info")]
    [SerializeField] bool cameraShakeActive = true;
    [SerializeField] Camera objectToMove;
    [SerializeField] float cameraShakeRadius = 6f;
    [SerializeField] float cameraShakeDuration = 1f;
    [SerializeField] AnimationCurve cameraShakeCurve;



    void cameraShake() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, cameraShakeRadius);
        foreach (Collider nearbyObject in colliders) {
            if (nearbyObject.GetComponent<Player>() && nearbyObject.GetType() != typeof(UnityEngine.CharacterController)) {
                // Start coroutine that shakes the camera
                StartCoroutine(shaking(nearbyObject));
            }
        }
    }

    IEnumerator shaking(Collider obj) {
        float elapsedTime = 0f;
        while (elapsedTime < cameraShakeDuration) {
            elapsedTime += Time.deltaTime;
            float strength = cameraShakeCurve.Evaluate(elapsedTime / cameraShakeDuration);
            objectToMove.transform.localPosition = objectToMove.transform.localPosition + Random.insideUnitSphere * strength;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
    }
}

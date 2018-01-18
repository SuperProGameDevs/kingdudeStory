using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [SerializeField] private GameObject observable;
    [SerializeField] private float dampTime = 0.15f;

    private Vector3 currentVelocity = Vector3.zero;
    private Camera cam;

    // Initiate anything on THIS object (starts before any Start call on any object)
    void Awake() {
        cam = this.GetComponent<Camera>();
        cam.transparencySortMode = TransparencySortMode.Orthographic;
    }

    private void FixedUpdate() 
    {
        if (observable) {
            // Smooth camera code
            Vector3 vpObsPosition = cam.WorldToViewportPoint(observable.transform.position);
            Vector3 camDelta = observable.transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, vpObsPosition.z));
            Vector3 destination = this.transform.position + camDelta;
            this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref currentVelocity, dampTime);
        }
    }
}

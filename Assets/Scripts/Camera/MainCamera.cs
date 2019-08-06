using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;

    void Start()
    {
        offset = this.transform.position - target.position;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetCamPos = target.position + offset;
            this.transform.position = Vector3.Lerp(this.transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}

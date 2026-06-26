using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    private void FixedUpdate()
    {
        if (cam == null) return;

        // Face camera
        transform.forward = cam.transform.forward;
    }
    void LateUpdate()
    {
        
    }
}
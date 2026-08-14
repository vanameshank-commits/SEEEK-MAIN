using UnityEngine;

public class PointATrigger : MonoBehaviour
{
    public FenceController fence;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            if (fence != null)
            {
                fence.OpenFromPointA();
            }

            Debug.Log("Point A activated.");
        }
    }
}
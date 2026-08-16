using UnityEngine;

public class MannequinStopTrigger : MonoBehaviour
{
    [Header("Mannequin AI")]
    public MannequinAI mannequinAI;

    private void OnTriggerEnter(Collider other)
    {
        MannequinAI ai =
            other.GetComponentInParent<MannequinAI>();

        if (ai == null)
            return;

        if (ai != mannequinAI)
            return;

        mannequinAI.StopChasing();

        Debug.Log("MANNEQUIN ENTERED STOP TRIGGER!");
    }
}
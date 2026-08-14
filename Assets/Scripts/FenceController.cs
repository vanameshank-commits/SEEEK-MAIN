using UnityEngine;
using System.Collections;

public class FenceController : MonoBehaviour
{
    [Header("Fence Positions")]
    public float closedY = -1f;
    public float openY = 1.727f;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private bool isMoving = false;

    // =========================================================
    // OPEN FROM POINT A
    // =========================================================

    public void OpenFromPointA()
    {
        if (isMoving)
            return;

        StopAllCoroutines();
        StartCoroutine(MoveFence(openY));
    }

    // =========================================================
    // DROP FENCE
    // =========================================================

    public void DropFence()
    {
        if (isMoving)
            return;

        StopAllCoroutines();
        StartCoroutine(DropAndReturn());
    }

    // =========================================================
    // MOVE TO POSITION
    // =========================================================

    IEnumerator MoveFence(float targetY)
    {
        isMoving = true;

        Vector3 targetPosition =
            new Vector3(
                transform.position.x,
                targetY,
                transform.position.z
            );

        while (Vector3.Distance(
            transform.position,
            targetPosition) > 0.01f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );

            yield return null;
        }

        transform.position = targetPosition;

        isMoving = false;
    }

    // =========================================================
    // DROP THEN COME BACK UP
    // =========================================================

    IEnumerator DropAndReturn()
    {
        isMoving = true;

        // Go DOWN
        Vector3 downPosition =
            new Vector3(
                transform.position.x,
                closedY,
                transform.position.z
            );

        while (Vector3.Distance(
            transform.position,
            downPosition) > 0.01f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    downPosition,
                    moveSpeed * Time.deltaTime
                );

            yield return null;
        }

        transform.position = downPosition;

        // Small delay
        yield return new WaitForSeconds(0.5f);

        // Come back UP
        Vector3 upPosition =
            new Vector3(
                transform.position.x,
                openY,
                transform.position.z
            );

        while (Vector3.Distance(
            transform.position,
            upPosition) > 0.01f)
        {
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    upPosition,
                    moveSpeed * Time.deltaTime
                );

            yield return null;
        }

        transform.position = upPosition;

        isMoving = false;
    }
}
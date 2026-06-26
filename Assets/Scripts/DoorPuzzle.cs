using UnityEngine;

public class DoorPuzzle : MonoBehaviour
{
    public Collider ringZone;
    public Collider sphereZone;
    public Collider coneZone;
    public Collider cubeZone;

    public GameObject ring;
    public GameObject sphere;
    public GameObject cone;
    public GameObject cube;

    public Transform door;

    bool opened = false;

    Quaternion closedRot;
    Quaternion openRot;

    void Start()
    {
        closedRot =
            door.rotation;

        openRot =
            Quaternion.Euler(
                door.eulerAngles.x,
                door.eulerAngles.y - 90,
                door.eulerAngles.z
            );
    }

    void Update()
    {
        bool ringOK =
            ringZone.bounds.Contains(
                ring.transform.position
            );

        bool sphereOK =
            sphereZone.bounds.Contains(
                sphere.transform.position
            );

        bool coneOK =
            coneZone.bounds.Contains(
                cone.transform.position
            );

        bool cubeOK =
            cubeZone.bounds.Contains(
                cube.transform.position
            );


        Debug.Log("Ring = " + ringOK);
        Debug.Log("Sphere = " + sphereOK);
        Debug.Log("Cone = " + coneOK);
        Debug.Log("Cube = " + cubeOK);

        bool solved =
            ringOK
            &&
            sphereOK
            &&
            coneOK
            &&
            cubeOK;

        if (
            solved
            &&
            !opened
        )
        {
            opened = true;

            door.rotation =
                openRot;

            Debug.Log(
                "DOOR OPEN"
            );
        }

        if (
            !solved
            &&
            opened
        )
        {
            opened = false;

            door.rotation =
                closedRot;

            Debug.Log(
                "DOOR CLOSED"
            );
        }
    }
}
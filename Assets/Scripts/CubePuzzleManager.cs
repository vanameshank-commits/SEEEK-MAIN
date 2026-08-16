using UnityEngine;
using TMPro;
using System.Collections;

public class CubePuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class CubeData
    {
        public GameObject cube;

        [Header("6 Letter Objects")]
        public TMP_Text[] letters = new TMP_Text[6];
    }

    // =========================================================
    // CUBES
    // =========================================================

    [Header("Cubes")]
    public CubeData[] cubes = new CubeData[4];


    // =========================================================
    // PUZZLE
    // =========================================================

    [Header("Puzzle")]
    public string answer = "CUBE";

    [Tooltip("Drag the player's actual Camera here.")]
    public Camera playerCamera;


    // =========================================================
    // DOOR 1
    // =========================================================

    [Header("Door 1")]
    public Transform door1;
    public Vector3 door1OpenPosition;
    public Vector3 door1OpenRotation;


    // =========================================================
    // DOOR 2
    // =========================================================

    [Header("Door 2")]
    public Transform door2;
    public Vector3 door2OpenPosition;
    public Vector3 door2OpenRotation;


    // =========================================================
    // DOOR MOVEMENT
    // =========================================================

    [Header("Door Movement")]
    public float doorMoveSpeed = 2f;
    public float doorRotateSpeed = 180f;


    // =========================================================
    // DOOR SOUND
    // =========================================================

    [Header("Door Sound")]
    public AudioSource doorAudioSource;
    public AudioClip doorOpenSound;


    private bool puzzleSolved = false;
    private bool checking = false;


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (puzzleSolved || checking)
            return;

        if (AllCubesPlaced())
        {
            StartCoroutine(CheckPuzzle());
        }
    }


    // =========================================================
    // CHECK ALL CUBES
    // =========================================================

    bool AllCubesPlaced()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i] == null ||
                cubes[i].cube == null)
            {
                return false;
            }

            if (!cubes[i].cube.CompareTag("PlacedCube"))
            {
                return false;
            }
        }

        return true;
    }


    // =========================================================
    // CHECK PUZZLE
    // =========================================================

    IEnumerator CheckPuzzle()
    {
        checking = true;

        // Give Unity one frame to update
        // the cube transforms after rotation.
        yield return null;

        string currentAnswer = "";

        for (int i = 0; i < cubes.Length; i++)
        {
            currentAnswer +=
                GetCameraFacingLetter(cubes[i]);
        }

        Debug.Log(
            "================================="
        );

        Debug.Log(
            "CURRENT CUBE ANSWER = " +
            currentAnswer
        );

        Debug.Log(
            "================================="
        );


        if (currentAnswer == answer)
        {
            puzzleSolved = true;

            Debug.Log(
                "PUZZLE SOLVED! CUBE"
            );

            StartCoroutine(OpenDoors());
        }

        checking = false;
    }


    // =========================================================
    // GET FACE FACING CAMERA
    // =========================================================

    string GetCameraFacingLetter(CubeData cubeData)
    {
        if (playerCamera == null)
        {
            Debug.LogError(
                "CubePuzzleManager: Player Camera is NOT assigned!"
            );

            return "";
        }

        if (cubeData.cube == null)
            return "";


        Transform cubeTransform =
            cubeData.cube.transform;


        // -----------------------------------------------------
        // Convert CAMERA position into the CUBE'S local space.
        // -----------------------------------------------------

        Vector3 cameraLocalPosition =
            cubeTransform.InverseTransformPoint(
                playerCamera.transform.position
            );


        // Direction from cube center toward camera
        Vector3 cameraDirection =
            cameraLocalPosition.normalized;


        float bestDot = -999f;

        TMP_Text bestLetter = null;

        Vector3 bestLocalPosition = Vector3.zero;


        // -----------------------------------------------------
        // Check all 6 letter objects
        // -----------------------------------------------------

        for (int i = 0;
             i < cubeData.letters.Length;
             i++)
        {
            if (cubeData.letters[i] == null)
                continue;


            // Letter position in cube local space
            Vector3 letterLocalPosition =
                cubeTransform.InverseTransformPoint(
                    cubeData.letters[i].transform.position
                );


            // Direction from cube center toward letter
            Vector3 faceDirection =
                letterLocalPosition.normalized;


            // Compare letter-face direction
            // with direction toward camera
            float dot =
                Vector3.Dot(
                    faceDirection,
                    cameraDirection
                );


            if (dot > bestDot)
            {
                bestDot = dot;

                bestLetter =
                    cubeData.letters[i];

                bestLocalPosition =
                    letterLocalPosition;
            }
        }


        if (bestLetter == null)
            return "";


        string letter =
            bestLetter.text
                .Trim()
                .ToUpper();


        Debug.Log(
            cubeData.cube.name +
            " -> PLAYER FACING LETTER = " +
            letter +
            " | Dot = " +
            bestDot.ToString("F2") +
            " | Local Face = " +
            bestLocalPosition
        );


        return letter;
    }


    // =========================================================
    // OPEN DOORS
    // =========================================================

    IEnumerator OpenDoors()
    {
        // Play door sound once when doors start opening
        if (doorAudioSource != null &&
            doorOpenSound != null)
        {
            doorAudioSource.PlayOneShot(
                doorOpenSound
            );
        }


        bool door1Done =
            door1 == null;

        bool door2Done =
            door2 == null;


        while (!door1Done ||
               !door2Done)
        {
            // =================================================
            // DOOR 1
            // =================================================

            if (door1 != null)
            {
                door1.position =
                    Vector3.MoveTowards(
                        door1.position,
                        door1OpenPosition,
                        doorMoveSpeed *
                        Time.deltaTime
                    );


                door1.rotation =
                    Quaternion.RotateTowards(
                        door1.rotation,
                        Quaternion.Euler(
                            door1OpenRotation
                        ),
                        doorRotateSpeed *
                        Time.deltaTime
                    );


                bool positionDone =
                    Vector3.Distance(
                        door1.position,
                        door1OpenPosition
                    ) < 0.01f;


                bool rotationDone =
                    Quaternion.Angle(
                        door1.rotation,
                        Quaternion.Euler(
                            door1OpenRotation
                        )
                    ) < 0.5f;


                door1Done =
                    positionDone &&
                    rotationDone;
            }


            // =================================================
            // DOOR 2
            // =================================================

            if (door2 != null)
            {
                door2.position =
                    Vector3.MoveTowards(
                        door2.position,
                        door2OpenPosition,
                        doorMoveSpeed *
                        Time.deltaTime
                    );


                door2.rotation =
                    Quaternion.RotateTowards(
                        door2.rotation,
                        Quaternion.Euler(
                            door2OpenRotation
                        ),
                        doorRotateSpeed *
                        Time.deltaTime
                    );


                bool positionDone =
                    Vector3.Distance(
                        door2.position,
                        door2OpenPosition
                    ) < 0.01f;


                bool rotationDone =
                    Quaternion.Angle(
                        door2.rotation,
                        Quaternion.Euler(
                            door2OpenRotation
                        )
                    ) < 0.5f;


                door2Done =
                    positionDone &&
                    rotationDone;
            }


            yield return null;
        }


        // =====================================================
        // FORCE FINAL EXACT POSITIONS
        // =====================================================

        if (door1 != null)
        {
            door1.position =
                door1OpenPosition;

            door1.rotation =
                Quaternion.Euler(
                    door1OpenRotation
                );
        }


        if (door2 != null)
        {
            door2.position =
                door2OpenPosition;

            door2.rotation =
                Quaternion.Euler(
                    door2OpenRotation
                );
        }


        Debug.Log(
            "BOTH DOORS OPENED!"
        );
    }
}
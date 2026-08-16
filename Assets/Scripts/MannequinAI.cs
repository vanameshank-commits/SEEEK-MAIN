using UnityEngine;
using UnityEngine.AI;

public class MannequinAI : MonoBehaviour
{
    [Header("PLAYER")]
    public Transform player;
    public Camera playerCamera;
    public PlayerMovement playerController;
    public MouseLook mouseLook;

    [Header("MANNEQUIN")]
    public float runSpeed = 4f;
    public float rotationSpeed = 360f;
    public float attackDistance = 2.5f;

    [Header("RESTART")]
    public GameObject restartPanel;

    [Header("SPAWN POINTS")]
    public Transform mannequinSpawnPoint;
    public Transform playerSpawnPoint;

    [Header("CHASE")]
    public bool chaseActivated = false;

    [Header("CHASE LIMIT")]
    public Transform mannequinChaseLimit;
    public float chaseLimitRadius = 0.5f;
    public bool useChaseLimit = false;

    [Header("NAVMESH")]
    public float playerSampleDistance = 5f;

    private NavMeshAgent agent;
    private Animator animator;

    private bool attacking = false;

    private NavMeshPath chasePath;
    private int currentCorner = 0;


    // =========================================================
    // START
    // =========================================================
    public void StopChasing()
    {
        chaseActivated = false;

        agent.isStopped = true;
        agent.ResetPath();

        animator.SetBool("Running", false);
        animator.Play("Idle");

        Debug.Log("MANNEQUIN STOPPED BY TRIGGER!");
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        chasePath = new NavMeshPath();

        // -----------------------------------------------------
        // IMPORTANT NAVMESH SETTINGS
        // -----------------------------------------------------

        agent.speed = runSpeed;
        agent.acceleration = 100f;
        agent.angularSpeed = 720f;
        agent.stoppingDistance = 0f;
        agent.autoBraking = false;

        agent.updatePosition = true;
        agent.updateRotation = false;

        agent.isStopped = true;

        // We are manually moving the mannequin.
        // NavMeshAgent is mainly being used for path calculation.
        agent.obstacleAvoidanceType =
            ObstacleAvoidanceType.NoObstacleAvoidance;


        // -----------------------------------------------------
        // RESTART PANEL
        // -----------------------------------------------------

        if (restartPanel != null)
            restartPanel.SetActive(false);


        // -----------------------------------------------------
        // CREATE SPAWN POINT IF EMPTY
        // -----------------------------------------------------

        if (mannequinSpawnPoint == null)
        {
            GameObject spawn =
                new GameObject("Mannequin Spawn Point");

            spawn.transform.position =
                transform.position;

            spawn.transform.rotation =
                transform.rotation;

            mannequinSpawnPoint =
                spawn.transform;
        }


        if (playerSpawnPoint == null && player != null)
        {
            GameObject spawn =
                new GameObject("Player Spawn Point");

            spawn.transform.position =
                player.position;

            spawn.transform.rotation =
                player.rotation;

            playerSpawnPoint =
                spawn.transform;
        }


        if (animator != null)
            animator.SetBool("Running", false);


        Debug.Log(
            "MANNEQUIN READY | NAVMESH = " +
            agent.isOnNavMesh
        );
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (attacking)
            return;


        if (!chaseActivated)
        {
            StopMannequin();
            return;
        }


        if (player == null)
            return;


        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "MANNEQUIN IS NOT ON NAVMESH!"
            );

            StopMannequin();
            return;
        }


        // =====================================================
        // PLAYER LOOKING AT MANNEQUIN
        // =====================================================

        if (PlayerIsLookingAtMe())
        {
            StopMannequin();
            return;
        }


        // =====================================================
        // ATTACK
        // =====================================================

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );


        if (distance <= attackDistance)
        {
            AttackPlayer();
            return;
        }


        // =====================================================
        // CHASE LIMIT
        // =====================================================

        if (useChaseLimit &&
            mannequinChaseLimit != null)
        {
            float limitDistance =
                Vector3.Distance(
                    transform.position,
                    mannequinChaseLimit.position
                );

            if (limitDistance <= chaseLimitRadius)
            {
                StopMannequin();
                return;
            }
        }


        // =====================================================
        // FIND PLAYER NAVMESH POSITION
        // =====================================================

        NavMeshHit playerHit;

        bool playerFound =
            NavMesh.SamplePosition(
                player.position,
                out playerHit,
                playerSampleDistance,
                NavMesh.AllAreas
            );


        if (!playerFound)
        {
            StopMannequin();

            Debug.LogWarning(
                "PLAYER NAVMESH POSITION NOT FOUND!"
            );

            return;
        }


        // =====================================================
        // CALCULATE PATH
        // =====================================================

        bool pathFound =
            NavMesh.CalculatePath(
                transform.position,
                playerHit.position,
                NavMesh.AllAreas,
                chasePath
            );


        if (!pathFound ||
            chasePath.status ==
            NavMeshPathStatus.PathInvalid)
        {
            StopMannequin();

            Debug.LogWarning(
                "MANNEQUIN COULD NOT CALCULATE PATH!"
            );

            return;
        }


        if (chasePath.corners.Length < 2)
        {
            StopMannequin();
            return;
        }


        // =====================================================
        // FIND THE CORRECT PATH CORNER
        // =====================================================

        currentCorner = 1;

        float closestDistance = Mathf.Infinity;


        for (int i = 1;
             i < chasePath.corners.Length;
             i++)
        {
            float d =
                Vector3.Distance(
                    transform.position,
                    chasePath.corners[i]
                );

            if (d < closestDistance)
            {
                closestDistance = d;
                currentCorner = i;
            }
        }


        Vector3 targetPoint =
            chasePath.corners[currentCorner];


        // Keep mannequin on terrain height
        targetPoint.y =
            transform.position.y;


        Vector3 direction =
            targetPoint -
            transform.position;

        direction.y = 0f;


        // =====================================================
        // REACHED CORNER
        // =====================================================

        if (direction.magnitude < 0.35f)
        {
            if (currentCorner <
                chasePath.corners.Length - 1)
            {
                currentCorner++;
            }

            return;
        }


        // =====================================================
        // MOVE
        // =====================================================

        Vector3 moveDirection =
            direction.normalized;


        // -----------------------------------------------------
        // MANUALLY MOVE USING NAVMESH AGENT
        // -----------------------------------------------------

        Vector3 movement =
            moveDirection *
            runSpeed *
            Time.deltaTime;


        agent.isStopped = false;

        agent.Move(movement);


        // =====================================================
        // ROTATE
        // =====================================================

        Quaternion targetRotation =
            Quaternion.LookRotation(
                moveDirection
            );


        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );


        // =====================================================
        // RUN ANIMATION
        // =====================================================

        if (animator != null)
        {
            animator.SetBool(
                "Running",
                true
            );
        }
    }


    // =========================================================
    // STOP
    // =========================================================

    void StopMannequin()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (animator != null)
        {
            animator.SetBool(
                "Running",
                false
            );
        }
    }


    // =========================================================
    // START CHASING
    // =========================================================

    public void StartChasing()
    {
        if (attacking)
            return;

        chaseActivated = true;

        Debug.Log(
            "================================"
        );

        Debug.Log(
            "MANNEQUIN CHASE ACTIVATED!"
        );

        Debug.Log(
            "ON NAVMESH = " +
            agent.isOnNavMesh
        );

        Debug.Log(
            "================================"
        );
    }


    // =========================================================
    // PLAYER LOOKING
    // =========================================================

    bool PlayerIsLookingAtMe()
    {
        if (playerCamera == null)
            return false;

        // Get all renderers belonging to the mannequin
        Renderer[] renderers =
            GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return false;

        // Calculate the center of the mannequin
        Bounds bounds =
            renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(
                renderers[i].bounds
            );
        }

        Vector3 mannequinCenter =
            bounds.center;


        // Direction from camera to mannequin
        Vector3 direction =
            mannequinCenter -
            playerCamera.transform.position;

        float distance =
            direction.magnitude;

        if (distance <= 0.01f)
            return false;

        direction.Normalize();


        // How directly the player is looking at mannequin
        float dot =
            Vector3.Dot(
                playerCamera.transform.forward,
                direction
            );


        // If mannequin is behind player
        if (dot < 0.65f)
            return false;


        // Convert mannequin position to screen
        Vector3 viewport =
            playerCamera.WorldToViewportPoint(
                mannequinCenter
            );


        // Mannequin must be in front of camera
        if (viewport.z <= 0)
            return false;


        // ---------------------------------------------------------
        // LOOKING AREA
        // ---------------------------------------------------------

        // Center of screen = 0.5 , 0.5
        //
        // These values control how accurately
        // player needs to look at mannequin.
        //

        float horizontalDistance =
            Mathf.Abs(
                viewport.x - 0.5f
            );

        float verticalDistance =
            Mathf.Abs(
                viewport.y - 0.5f
            );


        // Player is looking at mannequin
        if (horizontalDistance < 0.22f &&
            verticalDistance < 0.25f)
        {
            return true;
        }


        return false;
    }

    // =========================================================
    // ATTACK
    // =========================================================

    void AttackPlayer()
    {
        if (attacking)
            return;


        attacking = true;


        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }


        if (animator != null)
        {
            animator.SetBool(
                "Running",
                false
            );

            animator.SetTrigger(
                "Attack"
            );
        }


        Debug.Log(
            "MANNEQUIN ATTACKED PLAYER!"
        );


        if (restartPanel != null)
            restartPanel.SetActive(true);


        if (mouseLook != null)
            mouseLook.canLook = false;


        if (playerController != null)
            playerController.enabled = false;


        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }


    // =========================================================
    // RESTART BUTTON
    // =========================================================

    public void RestartMannequinPuzzle()
    {
        Debug.Log(
            "RESTARTING MANNEQUIN PUZZLE..."
        );


        if (restartPanel != null)
            restartPanel.SetActive(false);


        ResetPlayer();

        ResetMannequin();


        if (playerController != null)
            playerController.enabled = true;


        if (mouseLook != null)
            mouseLook.canLook = true;


        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;


        Debug.Log(
            "MANNEQUIN PUZZLE RESTARTED!"
        );
    }


    // =========================================================
    // RESET PLAYER
    // =========================================================

    void ResetPlayer()
    {
        if (player == null)
            return;


        CharacterController controller =
            player.GetComponent<CharacterController>();


        if (controller != null)
            controller.enabled = false;


        Rigidbody rb =
            player.GetComponent<Rigidbody>();


        if (rb != null)
        {
            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;
        }


        if (playerSpawnPoint != null)
        {
            player.position =
                playerSpawnPoint.position;

            player.rotation =
                playerSpawnPoint.rotation;
        }


        if (controller != null)
            controller.enabled = true;
    }


    // =========================================================
    // RESET MANNEQUIN
    // =========================================================

    public void ResetMannequin()
    {
        attacking = false;


        if (agent == null)
            agent =
                GetComponent<NavMeshAgent>();


        if (animator == null)
            animator =
                GetComponent<Animator>();


        agent.isStopped = true;
        agent.ResetPath();


        // -----------------------------------------------------
        // RETURN TO SPAWN
        // -----------------------------------------------------

        if (mannequinSpawnPoint != null)
        {
            NavMeshHit hit;


            if (NavMesh.SamplePosition(
                mannequinSpawnPoint.position,
                out hit,
                5f,
                NavMesh.AllAreas
            ))
            {
                agent.Warp(
                    hit.position
                );
            }
            else
            {
                transform.position =
                    mannequinSpawnPoint.position;
            }


            transform.rotation =
                mannequinSpawnPoint.rotation;
        }


        // -----------------------------------------------------
        // RESET PATH
        // -----------------------------------------------------

        currentCorner = 0;


        // -----------------------------------------------------
        // CHASE STAYS ACTIVE AFTER RESTART
        // -----------------------------------------------------

        chaseActivated = true;


        // -----------------------------------------------------
        // RESET ANIMATION
        // -----------------------------------------------------

        if (animator != null)
        {
            animator.ResetTrigger(
                "Attack"
            );

            animator.SetBool(
                "Running",
                false
            );

            animator.Play(
                "Idle"
            );
        }


        Debug.Log(
            "MANNEQUIN RETURNED TO SPAWN."
        );
    }
}   
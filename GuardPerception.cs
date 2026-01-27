using UnityEngine;

public class GuardPerception : MonoBehaviour
{
    [SerializeReference] private Transform pointA, pointB, currentTarget;
    [SerializeField] private float moveSpeed = 100.0f;
    private enum GuardState { Patrol, Chase, Search }
    [SerializeField] private GuardState guardState = GuardState.Patrol;
    [SerializeField] private float searchTimer = 0f;
    
    
    [SerializeField] public Transform player;
    [SerializeField] public float viewDistance = 10f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTarget = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        //switch case to check with current state the guard will move 
        switch (guardState)
        {
            case GuardState.Patrol:
                DoPatrol();
                break;
            case GuardState.Chase:
                DoChase();
                break;
            case GuardState.Search:
                DoSearch();
                break;
        }

        checkPlayerDistance();
    }


    void checkPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= viewDistance)
        {
            checkLineOfSight();
        }
        else if (guardState == GuardState.Chase)
        {
            guardState = GuardState.Search;
            searchTimer = 0f;
        }

    }

    void checkLineOfSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit hit;

        // Shoot a ray from the Guard to the Player
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance)) {
            if (hit.transform.CompareTag("Player")) {
                // The ray hit the Player directly!
                Debug.DrawRay(transform.position, directionToPlayer * hit.distance, Color.red);
                // Debug.Log("I SEE YOU!");
                guardState = GuardState.Chase;
            } else {
                // The ray hit a wall or something else
                Debug.DrawRay(transform.position, directionToPlayer * hit.distance, Color.white);
                // Debug.Log("Something is in the way.");
                if (guardState == GuardState.Chase)
                {
                  guardState = GuardState.Search;
                  searchTimer = 0f;
                }
            }
        }
    }
    
    void DoPatrol()
    {
        // Checking if points are existing
        if (pointA == null || pointB == null) return;

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        // Rotate your char and look at the target
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Change to the other target when arrived
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.2f) {
            if (currentTarget == pointA) {
                currentTarget = pointB;
            } else {
                currentTarget = pointA;
            }
        
            Debug.Log("Arrived! Heading to the other point.");
        }
    }
    
    void DoChase()
    {
        //logic for moving towards player
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Rotate to face the player
        Vector3 direction = (player.position - transform.position).normalized;
        float rotationSpeed = 15f;
        if (direction != Vector3.zero) {
            
            // We use a slightly faster rotation for chasing so the guard looks "alert"
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
        Debug.Log("I'm coming for you!");
    }
    
    void DoSearch()
    { 
        float maxSearchTime = 5f;  
      searchTimer += Time.deltaTime;
      Debug.Log("Searching... " + (maxSearchTime - searchTimer).ToString("F1"));

      // if timer is more than maxSearchTime return to patrol state
      if (searchTimer >= 5f)
      {
          guardState = GuardState.Patrol;   
      }
    }
}

using UnityEngine;

public class NPCPathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float reachDistance = 0.3f;

    private int currentWaypointIndex = 0;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // If we're far enough, move
        if (Vector3.Distance(transform.position, target.position) > reachDistance)
        {
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            if (animator != null)
                animator.SetBool("isWalking", false);
        }
    }
}

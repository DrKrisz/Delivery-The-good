using UnityEngine;

public class HeadBobSimple : MonoBehaviour
{
    public Transform playerTransform;       // Drag your player object
    public float walkBobSpeed = 6f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 12f;
    public float runBobAmount = 0.1f;

    private float timer = 0f;
    private Vector3 startLocalPos;
    private Vector3 lastPosition;

    void Start()
    {
        startLocalPos = transform.localPosition;
        lastPosition = playerTransform.position;
    }

    void Update()
    {
        // Calculate player movement speed manually
        Vector3 movement = playerTransform.position - lastPosition;
        movement.y = 0; // ignore vertical movement
        float speed = movement.magnitude / Time.deltaTime;

        lastPosition = playerTransform.position;

        bool isMoving = speed > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            float bobSpeed = isRunning ? runBobSpeed : walkBobSpeed;
            float bobAmount = isRunning ? runBobAmount : walkBobAmount;

            timer += Time.deltaTime * bobSpeed;
            float bobY = Mathf.Sin(timer) * bobAmount;

            transform.localPosition = startLocalPos + new Vector3(0f, bobY, 0f);
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPos, Time.deltaTime * 6f);
        }
    }
}

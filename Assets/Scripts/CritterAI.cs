using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CritterAI : MonoBehaviour
{
    public enum State { Idle, Follow, Send, Work, Return, Scared }

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float orbitRadius = 1.2f;
    public float followDistance = 0.8f; // min distance to player (center)

    [Header("Orbit")]
    public int orbitIndex = 0; // assigned by manager or computed
    public int maxOrbitSlots = 8;

    private State state = State.Idle;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 currentTargetPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnWhistled(Transform playerTransform)
    {
        player = playerTransform;
        state = State.Follow;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                // TBD: small idle behavior or stay in place
                break;
            case State.Follow:
                UpdateOrbitPosition();
                MoveTo(currentTargetPos);
                break;
            // other states here (later!)
        }
    }

    void UpdateOrbitPosition()
    {
        if (player == null) return;

        // compute orbit angle based on index and total slots
        float angle = (360f / maxOrbitSlots) * orbitIndex;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        currentTargetPos = (Vector2)player.position + offset;

        // maintain minimum followDistance (so critters don't hug center)
        Vector2 dirToPlayer = (Vector2)player.position - (Vector2)transform.position;

        if (dirToPlayer.magnitude < followDistance)
        {
            // push target outward slightly
            currentTargetPos = (Vector2)player.position - dirToPlayer.normalized * followDistance;
        }
    }

    void MoveTo(Vector2 target)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        // (later) set facing / animation parameters 
    }
}

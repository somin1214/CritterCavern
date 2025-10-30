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

    // method to help critters avoid overlapping each other
    void AvoidNeighbors()
    {
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Critter"));
        Vector2 repulse = Vector2.zero;

        foreach (var n in neighbors)
        {
            if (n.gameObject == gameObject) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)n.transform.position;
            float dist = diff.magnitude;

            if (dist > 0.01f)
            {
                repulse += diff.normalized / dist; // more push when closer
            }
        }

        if (repulse.sqrMagnitude > 0.01f)
        {
            Vector2 newPos = rb.position + repulse * 0.02f; // tweak small value
            rb.MovePosition(Vector2.MoveTowards(rb.position, newPos, moveSpeed * Time.fixedDeltaTime));
        }
    }
}

using UnityEngine;
using System.Collections;

/*
---------------------------------------
CritterAI.cs
 Handles critter following, orbiting, and basic task logic.
--------------------------------------- 
*/

[RequireComponent(typeof(Rigidbody2D))]
public class CritterAI : MonoBehaviour
{
    public enum State { Idle, Follow, Send, Work, Return, Scared }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float orbitRadius = 1.2f;
    [SerializeField] private float followDistance = 0.8f;

    [Header("Orbit")]
    [SerializeField] public int orbitIndex = 0;
    [SerializeField] public int maxOrbitSlots = 8;

    private State state = State.Idle;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 currentTargetPos;
    private Vector2 targetPos;
    private WorkTarget currentTarget; 

    private Animator anim;
    private bool isWorking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnWhistled(Transform playerTransform)
    {
        player = playerTransform;
        state = State.Follow;
    }

    void Update()
    {
        if (state == State.Work)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.1f && !isWorking)
            {
                StartCoroutine(DoWork());
            }
        }
    }

    public void SetWorkTarget(Vector2 target)
    {
        targetPos = target;
        state = State.Work;
        anim?.SetBool("isWorking", false);
        isWorking = false;

        // detect if there’s a WorkTarget
        RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero, 0f, LayerMask.GetMask("Workable"));
        if (hit.collider)
        {
            currentTarget = hit.collider.GetComponent<WorkTarget>();
        }
    }

    IEnumerator DoWork()
    {
        isWorking = true;
        anim?.SetBool("isWorking", true);
    
        if (currentTarget != null)
        {
            float timer = 0f;
            while (timer < 2f)
            {
                yield return new WaitForSeconds(1f);
                currentTarget.AddWork(1f); // contribute to object
                timer += 1f;
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
    
        anim?.SetBool("isWorking", false);
        isWorking = false;
        state = State.Follow;
    }


    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Follow:
                UpdateOrbitPosition();
                MoveTo(currentTargetPos);
                AvoidNeighbors();
                break;
        }
    }

    void UpdateOrbitPosition()
    {
        if (player == null) return;

        float angle = (360f / maxOrbitSlots) * orbitIndex;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        currentTargetPos = (Vector2)player.position + offset;

        Vector2 dirToPlayer = (Vector2)player.position - (Vector2)transform.position;
        if (dirToPlayer.magnitude < followDistance)
        {
            currentTargetPos = (Vector2)player.position - dirToPlayer.normalized * followDistance;
        }
    }

    void MoveTo(Vector2 target)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

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
                repulse += diff.normalized / dist;
            }
        }

        if (repulse.sqrMagnitude > 0.01f)
        {
            Vector2 newPos = rb.position + repulse * 0.02f;
            rb.MovePosition(Vector2.MoveTowards(rb.position, newPos, moveSpeed * Time.fixedDeltaTime));
        }
    }
}

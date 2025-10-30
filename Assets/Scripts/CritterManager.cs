using System.Collections.Generic;
using UnityEngine;

public class CritterManager : MonoBehaviour
{
    public static CritterManager Instance { get; private set; }
    public List<CritterAI> critters = new List<CritterAI>();
    public int maxOrbitSlots = 8;

    void Awake()
    {
        Instance = this;
    }

    public void Register(CritterAI c)
    {
        if(!critters.Contains(c)) critters.Add(c);
        ReassignOrbitIndices();
    }

    public void Unregister(CritterAI c)
    {
        if(critters.Contains(c)) critters.Remove(c);
        ReassignOrbitIndices();
    }

    void ReassignOrbitIndices()
    {
        for (int i = 0; i < critters.Count; i++)
        {
            critters[i].orbitIndex = i;
            critters[i].maxOrbitSlots = Mathf.Max(maxOrbitSlots, critters.Count);
        }
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

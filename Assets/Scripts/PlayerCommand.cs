using System.Collections.Generic;
using UnityEngine;

/*
---------------------------------------
PlayerCommand.cs
 Handles player commands and interactions with critters.
--------------------------------------- 
*/
public class PlayerCommand : MonoBehaviour
{
    [Header("Whistle Settings")]
    public float whistleRadius = 3f;
    public LayerMask critterLayer;
    public Color gizmoColor = new Color(0f, 0.8f, 1f, 0.2f);

    [Header("Command Settings")]
    public LayerMask interactableLayer; // objects to work on

    private List<CritterAI> crittersInRange = new List<CritterAI>();

    void Update()
    {
        HandleWhistle();
        HandleRightClickSend();
    }

    void HandleWhistle()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, whistleRadius, critterLayer);

            crittersInRange.Clear();
            foreach (var hit in hits)
            {
                CritterAI ai = hit.GetComponent<CritterAI>();
                if (ai != null)
                {
                    ai.OnWhistled(transform);
                    crittersInRange.Add(ai);
                }
            }
        }
    }

    void HandleRightClickSend()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero, 0f, interactableLayer);

            if (hit.collider != null)
            {
                Vector2 targetPos = hit.point;

                // Send all following critters to work
                foreach (var critter in crittersInRange)
                {
                    critter.SetWorkTarget(targetPos);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, whistleRadius);
    }
}

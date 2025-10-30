using System.Collections.Generic;
using UnityEngine;

/*
---------------------------------------
PlayerWhistle.cs
Handles player whistle input and notifies critters within range.
--------------------------------------- 
*/
public class PlayerWhistle : MonoBehaviour
{
    [Header("Whistle Settings")]
    public float whistleRadius = 3.5f;
    [SerializeField] private float baseSpriteRadius = 1.0f; // fix: Added missing variable
    public LayerMask critterLayer;
    public bool whistleActive = false;

    [Header("Visuals")]
    public GameObject whistleVisual; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Whistle();
        }
    }

    private void Whistle()
    {
        whistleActive = true;

        if (whistleVisual != null)
        {
            whistleVisual.SetActive(true);

            // Scale the visual circle relative to the whistle radius
            whistleVisual.transform.localScale = Vector3.one * (whistleRadius / baseSpriteRadius);
        }

        // Detect critters in radius
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, whistleRadius, critterLayer);
        foreach (var c in cols)
        {
            var crit = c.GetComponent<CritterAI>();
            if (crit != null)
                crit.OnWhistled(this.transform);
        }

        // Deactivate whistle after short delay
        Invoke(nameof(StopWhistle), 0.3f);
    }

    private void StopWhistle()
    {
        whistleActive = false;
        if (whistleVisual != null)
            whistleVisual.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.15f);
        Gizmos.DrawSphere(transform.position, whistleRadius);

        Gizmos.color = new Color(0f, 0.8f, 1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, whistleRadius);
    }
}

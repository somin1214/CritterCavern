using System.Collections.Generic;
using UnityEngine;

public class PlayerWhistle : MonoBehaviour
{
    public float whistleRadius = 3.5f;
    public LayerMask critterLayer;
    public bool whistleActive = false;
    public GameObject whistleVisual; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Whistle();
        }
    }

    void Whistle()
    {
        whistleActive = true;
        whistleVisual.SetActive(true);
        whistleVisual.transform.localScale = Vector3.one * (whistleRadius / baseSpriteRadius);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, whistleRadius, critterLayer);
        foreach (var c in cols)
        {
            var crit = c.GetComponent<CritterAI>();
            if (crit != null) crit.OnWhistled(this.transform);
        }
        // (maybe) start a coroutine to turn whistleActive off after short time
        Invoke(nameof(StopWhistle), 0.3f);
    }

    void StopWhistle()
    {
        whistleActive = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.15f);
        Gizmos.DrawSphere(transform.position, whistleRadius);
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, whistleRadius);
    }
}

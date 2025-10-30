using UnityEngine;
using System.Collections;

public class WorkTarget : MonoBehaviour
{
    [Header("Work Settings")]
    public float requiredWork = 5f;      // total "work units" to complete
    public float workPerSecond = 1f;     // how much 1 critter contributes per second
    public bool destroyOnComplete = true;

    private float currentWork = 0f;
    private bool isComplete = false;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public IEnumerator WorkRoutine(CritterAI critter)
    {
        while (!isComplete)
        {
            yield return new WaitForSeconds(1f);
            AddWork(workPerSecond);
        }
    }

    public void AddWork(float amount)
    {
        if (isComplete) return;

        currentWork += amount;

        // (later) shake a bit
        if (sr != null)
        {
            float progress = currentWork / requiredWork;
            sr.color = Color.Lerp(Color.white, Color.gray, progress);
        }

        if (currentWork >= requiredWork)
        {
            CompleteWork();
        }
    }

    void CompleteWork()
    {
        isComplete = true;
        Debug.Log($"{name} - Work complete!");

        if (destroyOnComplete)
        {
            Destroy(gameObject);
        }
        else
        {
            sr.color = Color.green; // mark as done
        }
    }
}

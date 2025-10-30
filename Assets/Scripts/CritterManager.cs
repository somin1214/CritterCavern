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
}

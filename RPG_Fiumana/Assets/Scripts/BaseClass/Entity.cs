using System.Collections.Generic;
using UnityEngine;


public class Entity : MapObject
{
    public Dictionary<int, int> entityCoordinates;
    public Perk startingStats;
    public Vector2Int coords;

    private void Start()
    {
        //Debug.Log("Starting Stats");
        AttachPerk(startingStats);
        GameplayManager.Instance.RegisterEntity(this); 
    }

    private void Update()
    {
        foreach (Perk pk in stats.attachedPerks)
        {
            pk.OnUpdate();
        }
    }
}


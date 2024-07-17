using System.Collections.Generic;
using UnityEngine;


public class Entity : MapObject
{
    public Dictionary<int, int> entityCoordinates;
    public Perk startingStats;
    public Vector2Int coords;
    public bool canDrop = false;
    public List<GameObject> droppableObjects = new();

    private void Awake()
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

        if(canDrop)
        {
            DropOnDefeat();
        }
    }

    private void DropOnDefeat()
    {
        Debug.Log(statsList[3].value);

        if(statsList[3].value <= 0)
        {
            int randomObjectIndex = Random.Range(0, droppableObjects.Count);
            GameObject randomObject = droppableObjects[randomObjectIndex];

            Instantiate(randomObject, gameObject.transform.position, Quaternion.identity);
            Debug.Log("Enemy defeated!");
            Destroy(gameObject);
        }
    }
}


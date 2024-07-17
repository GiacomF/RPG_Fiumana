using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    #region SINGLETON
    private static GameplayManager _instance;
    public static GameplayManager Instance
    {
        get
        {
            if(_instance)
                return _instance;

            _instance = GameObject.FindFirstObjectByType<GameplayManager>();
            return _instance;
        }
    }
    #endregion

    public List<Entity> entities= new List<Entity>();
    public Dictionary<Vector2Int, Cell> coordsToTiles = new Dictionary<Vector2Int, Cell>();
    public GameObject Map;
    public float slerpInterpolationTime;
    public int activeEntityInt;
    public float upOffset;
    public int entityLayer = 0;

    private void Start()
    {
        entityLayer = Physics.DefaultRaycastLayers & ~(1 << LayerMask.NameToLayer("IgnoreRaycast"));
        activeEntityInt = 0;

        for (int i = 0; i < Map.transform.childCount; i++)
        {
            Cell thisCellScript = Map.transform.GetChild(i).gameObject.GetComponent<Cell>();
            Vector2Int thisCellCoords = thisCellScript.tileSpecs.coords;

            coordsToTiles.Add(thisCellCoords, thisCellScript);
        }

        foreach (Entity entity in entities)
        {
            Cell currentCell = coordsToTiles[entity.coords];
            entity.stats.accumulatedMovementCost = currentCell.tileSpecs.traverseCost;
            AttachTerrainPerks(currentCell, entity);
        }

        //Debug.Log(coordsToTiles.Count);
    }

    public void Update()
    {
        InputManager.Instance.leftClick.performed += OnLeftMouseClick;
    }

    public void RegisterEntity(Entity entity)
    {
        entities.Add(entity);
        entity.transform.position = new Vector3(entity.coords.x, upOffset, entity.coords.y);
    }

    public void MoveEntity(Vector2Int destination)
    {
        Entity currentlyActiveEntity = GetCurrentlyActiveEntity();
        //Debug.Log("Number of entities: " + entities.Count);
        if(coordsToTiles.TryGetValue(destination, out Cell cell) == true)
        {
            switch (cell.tileSpecs.type)
            {
                case Cell.TerrainType.Water:
                return;
            }

            Cell currentCell = coordsToTiles[currentlyActiveEntity.coords];
            DetachCurrentTerrainPerks(currentCell, currentlyActiveEntity);

            GameObject character = currentlyActiveEntity.gameObject;
            character.transform.position = Vector3.Slerp(currentlyActiveEntity.gameObject.transform.position, new Vector3(destination.x, upOffset, destination.y), slerpInterpolationTime);
            currentlyActiveEntity.coords = destination;
            Debug.Log("Target Cell traverse cost : " + cell.tileSpecs.traverseCost);
            currentlyActiveEntity.stats.ApplyMovementCost(cell.tileSpecs.traverseCost);

            AttachTerrainPerks(cell, currentlyActiveEntity);
        }
    }

    private Entity GetCurrentlyActiveEntity()
    {
        if(activeEntityInt > entities.Count - 1)
        {
            activeEntityInt = 0;
        }

        Entity currentlyActiveEntity;
        currentlyActiveEntity = entities[activeEntityInt];

        return currentlyActiveEntity;
    }

    public void OnLeftMouseClick(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            //Debug.Log("Left click");
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            float actionsLeft = GetCurrentlyActiveEntity().statsList[0].value;


            if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, entityLayer))
            {
                Cell cellInfo = hitInfo.collider.gameObject.GetComponent<Cell>();
                
                Entity occupyingEntity = GetEntityAtCoords(cellInfo.tileSpecs.coords);
                Debug.Log(occupyingEntity);

                if (actionsLeft == 0)
                {
                    Debug.Log("Not enough action points!");
                }

                else if (cellInfo.tileSpecs.traverseCost > actionsLeft)
                {
                    Debug.Log("Not enough action points to move there!");
                }
                
                else if (occupyingEntity != null)
                {
                    Attack(occupyingEntity);
                }
                
                else
                {
                    MoveEntity(cellInfo.tileSpecs.coords);
                }
            }
        }
    }

    private void Attack(Entity entityToAttack)
    {
        entityToAttack.stats.ApplyHealthLoss(CalculateHealthLoss(entityToAttack));
        entityToAttack.ChangeStats(); 
        Debug.Log("Attack happened + health loss : " + entityToAttack.stats.accumulatedHealthLoss);
    }

    private float CalculateHealthLoss(Entity entityToAttack)
    {
        Entity currentlyActiveEntity = GetCurrentlyActiveEntity();
        float healthLossAmount = currentlyActiveEntity.statsList[1].value - entityToAttack.statsList[2].value;

        return healthLossAmount;
    }

    private Entity GetEntityAtCoords(Vector2Int coordsToCheck)
    {
        foreach (Entity entity in entities)
        {
            if (entity.coords == coordsToCheck)
            {
                return entity; 
            }
        }

        return null; 
    }

    private void AttachTerrainPerks(Cell cell, Entity entity)
    {
        Cell.TerrainSpecs thisTileSpecs = cell.tileSpecs;
        if(thisTileSpecs.terrainsPerks.Count == 0)
            return;
        foreach (Perk perk in thisTileSpecs.terrainsPerks)
        {
            //Debug.Log(perk.perkModifiers.Count);
            entity.AttachPerk(perk);
        }
    }

    private void DetachCurrentTerrainPerks(Cell cell, Entity entity)
    {
        Cell.TerrainSpecs thisTileSpecs = cell.tileSpecs;
        if(thisTileSpecs.terrainsPerks.Count == 0)
            return;
            //Debug.Log(thisTileSpecs.terrainsPerks.Count);
        foreach (Perk perk in thisTileSpecs.terrainsPerks)
        {
            //Debug.Log(perk.perkModifiers.Count);
            entity.DetachPerk(perk);
        }
    }

    private void StartTurn()
    {
        activeEntityInt += 1;
        Entity currentlyActiveEntity = GetCurrentlyActiveEntity();
        currentlyActiveEntity.stats.accumulatedMovementCost = 0;
        currentlyActiveEntity.ChangeStats();
    }
}

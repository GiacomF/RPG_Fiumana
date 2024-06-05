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
    public Dictionary<Vector2Int, Cell> coordsToTiles= new Dictionary<Vector2Int, Cell>();
    public GameObject Map;
    public float slerpInterpolationTime;
    public int activeEntityInt;
    public float upOffset;

    private void Start()
    {
        activeEntityInt = 2;

        for (int i = 0; i < Map.transform.childCount; i++)
        {
            Cell thisCellScript = Map.transform.GetChild(i).gameObject.GetComponent<Cell>();
            Vector2Int thisCellCoords = thisCellScript.tileSpecs.coords;

            coordsToTiles.Add(thisCellCoords, thisCellScript);
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
        //Debug.Log("Number of entities: " + entities.Count);
        if(coordsToTiles.TryGetValue(destination, out Cell cell) == true)
        {
            switch (cell.tileSpecs.type)
            {
                case Cell.TerrainType.Water:
                return;
            }

            GameObject character = GetCurrentlyActiveEntity().gameObject;
            character.transform.position = Vector3.Slerp(GetCurrentlyActiveEntity().gameObject.transform.position, new Vector3(destination.x, upOffset, destination.y), slerpInterpolationTime);
            AttachTerrainPerks(cell);
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

            if(Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Cell cellInfo = hitInfo.collider.gameObject.GetComponent<Cell>();
                Debug.Log(cellInfo.tileSpecs.coords);
                MoveEntity(cellInfo.tileSpecs.coords);
            }
        }
    }

    private void AttachTerrainPerks(Cell cell)
    {
        Entity entity = GetCurrentlyActiveEntity();
        Cell.TerrainSpecs thisTileSpecs = cell.tileSpecs;
        if(thisTileSpecs.terrainsPerks.Count == 0)
            return;
            Debug.Log(thisTileSpecs.terrainsPerks.Count);
        foreach (Perk perk in thisTileSpecs.terrainsPerks)
        {
            Debug.Log(perk.perkModifiers.Count);
            entity.AttachPerk(perk);
        }
    }
}

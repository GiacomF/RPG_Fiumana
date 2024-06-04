using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public TerrainSpecs[] terrainSpecs;
    public TerrainSpecs tileSpecs;

    [Serializable]
    public enum TerrainType
    {
        Plain,
        Swamp,
        Water,
        Hill
    }

    [Serializable]
    public struct TerrainSpecs
    {
        public Vector2Int coords;
        public TerrainType type;
        public int traverseCost;
        public List<Perk> terrainsPerks;
        public GameObject terrainPrefab;
    }

    private void Awake()
    {
        int randomTile = UnityEngine.Random.Range(0, terrainSpecs.Length - 1);
        tileSpecs = terrainSpecs[randomTile];
    }

    public void GenerateVisual(Vector3 position, Transform transform)
    {
        Instantiate(tileSpecs.terrainPrefab, position, Quaternion.identity, transform);
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainTypes", menuName = "ScriptableObjects/AllTerrains")]
public class TerrainTypes : ScriptableObject
{
    public Cell.TerrainSpecs[] allTerrainSpecs;
}

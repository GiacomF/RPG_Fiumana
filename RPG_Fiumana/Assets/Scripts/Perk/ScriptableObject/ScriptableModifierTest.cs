using UnityEngine;
using static PerkStructure;

[CreateAssetMenu(fileName = "PerkValuesScriptableObject", menuName = "Perks/Perk_SO")]
public class PerkValuesScriptableObject : ScriptableObject
{                 
    [SerializeField] public Modifier[] allModifiers;
}

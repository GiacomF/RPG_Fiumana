using UnityEngine;

//Questo è il perk vero e proprio, dove i metodi e i parametri generali della PerkStructure vengono definiti e concretizzati per potersi adattare ad ogni possibile utilizzo
public class Perk : PerkStructure
{
    //Lo scriptable object dal quale vengono presi i Modifier 
    public PerkValuesScriptableObject perkModifiersArray;
    public PerkValuesScriptableObject updatePerkModifiersArray;

    private void Awake()
    {
        //Aggiungiamo alla lista i Modifier ricavati dallo scriptable object
        foreach (Modifier mod in perkModifiersArray.allModifiers)
        {
            perkModifiers.Add(mod);
            //Debug.Log(mod.statToModify + " Modification Added");
        }

        foreach (Modifier mod in updatePerkModifiers)
        {
            updatePerkModifiers.Add(mod);
        }
    }
}

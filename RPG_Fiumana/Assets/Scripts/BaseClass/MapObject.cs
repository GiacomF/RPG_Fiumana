using System;
using System.Collections.Generic;
using UnityEngine;
using static StatsEngine;
using static PerkStructure;

#region Generic Map Object Script

public class MapObject : MonoBehaviour, IPerkable
{
    public StatsEngine stats = new StatsEngine();

    [Serializable]
    public struct Stat
    {
        public string name;
        public float value;

        public Stat(string name, float value)
        {
            this.name = name;
            this.value = value;
        }
    }

    [SerializeField] List<Stat> statsList = new List<Stat>();
    [SerializeField] List<Perk> currentlyAttachedPerks;

    public void AttachPerk(Perk perkToAttach)
    {
        stats.AttachPerk(perkToAttach);
        ChangeStats();
    }

    public void DetachPerk(Perk perkToDetach)
    {
        stats.DetachPerk(perkToDetach);
        ChangeStats();
    }

    public float GetStatValue(StatsEngine.RPGStat stat)
    {
        return stats.Get(stat);
    }

    public void ChangeStats()
    {
        statsList.Clear();
        foreach(KeyValuePair<RPGStat, float> kvp in stats.statistics)
        {
            statsList.Add(new Stat(kvp.Key.ToString(), GetStatValue(kvp.Key)));
        }
        currentlyAttachedPerks = stats.attachedPerks;
    }
}

#endregion
#region Stat Engine

//Creiamo e definiamo cos'è uno StatsEngine
public class StatsEngine
{   //#region tutti i valori contenuti nello StatsEngine
    //L'enum ci definisce tutte le potenziali statistiche, è utile per associare un valore mnemonico ad uno numerico
    [Serializable]
    public enum RPGStat
    {
        Actions,
        Damage,
        Protection,
        maxHealth
    }

    public const int defaultValue = 1;
    public Dictionary<RPGStat, float> statistics { get; private set; } = new Dictionary<RPGStat, float>();
    //Creaimo una nuova lista di perk. Definiamo cos'è un perk più in basso nello script
    //I perk contenuti in questo StatsEngine serviranno dopo nel metodo UpdateStats()
    public List<Perk> attachedPerks = new List<Perk>();
    private const int minStatValue = 0;
    private const int maxStatValue = 100;
    //#endregion

    //Chiamato quando qualunque cosa ci equipaggia un perk
    public void AttachPerk(Perk perkToAttach)
    {
        if(attachedPerks.Contains(perkToAttach))
        return;
        //Debug.Log(perkToAttach);
        //Aggiunge il perk alla lista attachedPerks     
        attachedPerks.Add(perkToAttach);
        //Setta quale complesso di statistiche andrà a modificare
        perkToAttach.SetParentStatsEngine(this);
        //Chiama il metodo OnAttachPerck()
        perkToAttach.OnAttachPerk();
        //Debug.Log(attachedPerks.Count);
        UpdateStats();
    }
    //Chiamato quando qualunque cosa ci disequipaggia un perk
    public void DetachPerk(Perk perkToDetach)
    {
        perkToDetach.OnDetachPerk();
        attachedPerks.Remove(perkToDetach);
        //Debug.Log(attachedPerks.Count);
        UpdateStats();
    }

    public void UpdateStats()
    {
        // Resetta le statistiche ai valori di default per ricalcolare correttamente
        foreach (RPGStat stat in Enum.GetValues(typeof(RPGStat)))
        {
            statistics[stat] = defaultValue;
        }

        // Prima esegue le addizioni
        foreach (Perk perk in attachedPerks)
        {
            foreach (Modifier mod in perk.perkModifiers)
            {
                if (mod.type == ModifierType.Add)
                {
                    statistics[mod.statToModify] += mod.value;
                }
            }
        }

        // Poi esegue le moltiplicazioni
        foreach (Perk perk in attachedPerks)
        {
            foreach (Modifier mod in perk.perkModifiers)
            {
                if (mod.type == ModifierType.Multiply)
                {
                    statistics[mod.statToModify] *= 1 + mod.value;
                }
            }
        }

        // Limita i valori delle statistiche
        foreach (RPGStat stat in Enum.GetValues(typeof(RPGStat)))
        {
            statistics[stat] = Mathf.Clamp(statistics[stat], minStatValue, maxStatValue);
        }
    }
    
    public float Get(StatsEngine.RPGStat stat)
    {
        return statistics[stat];
    }
}

#endregion

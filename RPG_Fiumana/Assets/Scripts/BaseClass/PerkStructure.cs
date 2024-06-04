using System;
using System.Collections.Generic;
using UnityEngine;
using static StatsEngine;
//Questo script è la struttura generica dei Perk, base di qualunque cambio di statistiche nel gioco, tutte le modifiche passano da qui
//Contiene la struttura dei modifier, il tipo ModifierType, il set di statistiche da modificare, la lista dei Modifier contenuti nel perk, metodi per assegnare lo statsEngine e aggiungere le modifiche, nonchè quelli per dare effetti situazionali ai Perk (all'equipaggiamneto, nel tempo o alla perdita) 
public class PerkStructure : MonoBehaviour
{
    //Definiamo se il modificatore è di tipo somma o moltiplicazione
    [Serializable]
    public enum ModifierType
    {
        Add,
        Multiply
    }

    [Serializable]
    public struct Modifier
    {
        //Quale statistica viene modificata
        public RPGStat statToModify;
        //In che modo
        public ModifierType type;
        //Con quale valore
        public float value;

        public Modifier(RPGStat statToModify, ModifierType type, float value)
        {
            this.statToModify = statToModify;
            this.type = type;
            this.value = value;
        }
    }

    protected StatsEngine parentStats;
    public List<Modifier> perkModifiers;
    public List<Modifier> updatePerkModifiers;

    protected void AddModifierToList(Modifier mod)
    {
        perkModifiers.Add(mod);
    }

    public void SetParentStatsEngine(StatsEngine parent)
    {
        parentStats = parent;
    }

    // Called every time the PerkStructure is attached to a map object
    public virtual void OnAttachPerk()
    {

    }

    // Called every time the PerkStructure is detached from a map object
    public virtual void OnDetachPerk()
    {

    }

    // Called every game time unit
    public virtual void OnUpdate()
    {

    }
}

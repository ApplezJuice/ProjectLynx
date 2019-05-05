using UnityEngine;

//public enum SpellType { Ranged }
//public enum SpellDiscipline { Frost, Fire, Arcane, Shadow}

[System.Serializable]
public class Spell
{
    public Texture icon;
    public string name;
    public string description;
    public int id;
    public GameObject spellPrefab;
    public SpellType type;
    public SpellDiscipline spellDisc;
    public float castTime;
    public float cooldown;
    public float baseDamage;

    public Spell(Spell d)
    {
        icon = d.icon;
        name = d.name;
        description = d.description;
        id = d.id;
        spellPrefab = d.spellPrefab;
        type = d.type;
        castTime = d.castTime;
        cooldown = d.cooldown;
        spellDisc = d.spellDisc;


    }
}

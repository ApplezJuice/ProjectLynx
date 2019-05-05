using System.Collections.Generic;
using UnityEngine;

public enum SpellType { Ranged }
public enum SpellDiscipline { Frost, Fire, Arcane, Shadow }

[CreateAssetMenu(menuName = "Spell Config")]
public class SpellN : ScriptableObject
{
    [SerializeField] Sprite icon = null;
    [SerializeField] string spellName = null;
    [SerializeField] string description = null;
    [SerializeField] int id = 0;
    [SerializeField] GameObject spellPrefab = null;
    [SerializeField] SpellType type = SpellType.Ranged;
    [SerializeField] SpellDiscipline spellDisc = SpellDiscipline.Frost;
    [SerializeField] float castTime = 0f;
    [SerializeField] float cooldown = 0f;
    [SerializeField] float baseDamage = 0f;
    [SerializeField] float spellSpeed = 0f;
    [SerializeField] Color spellBarColor = Color.black;

    public Sprite getIcon() { return icon; }
    public string getSpellName() { return spellName; }
    public string getSpellDescription() { return description; }
    public int getSpellID() { return id; }
    public GameObject getSpellPrefab() { return spellPrefab; }
    public SpellType getSpellType() { return type; }
    public SpellDiscipline getSpellDiscipline() { return spellDisc; }
    public float getSpellCastTime() { return castTime; }
    public float getSpellCooldown() { return cooldown; }
    public float getSpellBaseDamage() { return baseDamage; }
    public float getSpellSpeed() { return spellSpeed; }
    public Color getSpellBarColor() { return spellBarColor; }
}

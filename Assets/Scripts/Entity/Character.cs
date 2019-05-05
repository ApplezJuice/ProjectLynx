using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class Character : EntityBase
{
    public UIManager uiManager;

    public bool hpNeedsUpdateing = true;
    public bool manaNeedsUpdating = true;
    public bool isCasting = false;
    public bool spellCasted = false;

    [SerializeField]
    //public GameObject rangedSpellPrefab;
    public RangedSpell currentSpellCasted;

    // Spell System
    //public Spell[] allSpells;
    //public Spell[] playerSpells;
    public SpellBook playerSpellBook;
    public HashSet<int> spellOnCD;

    public Character (string _entityName, float _maxHP, float _maxMana, float _atkPower, int _level, float _atkSpeed, float _dodge) : base (_entityName, 100f, _maxMana, _atkPower, _level, _atkSpeed, _dodge)
    {

    }
    

    public void Start()
    {
        // add spells
        //playerSpells[0].id = allSpells[0].id;
        //playerSpells[0].name = allSpells[0].name;
        //playerSpells[0].description = allSpells[0].description;
        //playerSpells[0].icon = allSpells[0].icon;
        //playerSpells[0].type = allSpells[0].type;
        //playerSpells[0].spellPrefab = allSpells[0].spellPrefab;
        //playerSpells[0].cooldown = allSpells[0].cooldown;
        //playerSpells[0].castTime = allSpells[0].castTime;
        //playerSpells[0].spellDisc = allSpells[0].spellDisc;
        //playerSpells[0].baseDamage = allSpells[0].baseDamage;
        spellOnCD = new HashSet<int>();
        SetAdditionalStats();
        //maxHP = 100f;
        curHP = 100f;
        hp.BaseValue = 100f;

        curMana = 90f;
        mana.BaseValue = 90f;
        hpNeedsUpdateing = true;
        manaNeedsUpdating = true;

        TextMeshPro playerName = GetComponentInChildren<TextMeshPro>();
        playerName.SetText(entityName);
    }

    public void Update()
    {

        // DEBUG TESTING
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
            hpNeedsUpdateing = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            HealSelf(10);
            hpNeedsUpdateing = true;
        }
        // Mana
        if (Input.GetKeyDown(KeyCode.L))
        {
            UseMana(10);
            manaNeedsUpdating = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetMana(10);
            manaNeedsUpdating = true;
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            StatModifier strMod = new StatModifier(10f, StatModType.Flat);
            Strength.AddModifier(strMod);
            uiManager.charSheetNeedsUpdating = true;
        }
        // END DEBUG TESTING

    }

    public void RangedAttack(int id)
    {
        Vector3 SpawnSpellLoc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject clone;
        //clone = Instantiate(playerSpells[id].spellPrefab, SpawnSpellLoc, Quaternion.identity);
        clone = Instantiate(playerSpellBook.playerOwnedSpells[id].getSpellPrefab(), SpawnSpellLoc, Quaternion.identity);

    }

    public void SetAdditionalStats()
    {
        Strength.BaseValue = 10;
        StatModifier mod1 = new StatModifier(10, StatModType.Flat);
        Strength.AddModifier(mod1);
        //print(Strength.Value);

        dodge.BaseValue = 10;
        //print(dodge.Value);
        dodge.AddModifier(mod1);
        //print(dodge.Value);
    }

    public void UsedSpell(int id)
    {
        //print(spellOnCD.Contains(id));
        if (spellOnCD.Contains(id) == false)
        {
            //print("Used spell 1");
            isCasting = true;
            //if (playerSpells[id].type == SpellType.Ranged)
            if (playerSpellBook.playerOwnedSpells[id].getSpellType() == SpellType.Ranged)
            {
                //currentSpellCasted = rangedSpellPrefab.GetComponent<RangedSpell>();
                currentSpellCasted = playerSpellBook.playerOwnedSpells[id].getSpellPrefab().GetComponent<RangedSpell>();
                currentSpellCasted.castTime = playerSpellBook.playerOwnedSpells[id].getSpellCastTime();
                currentSpellCasted.needPlayerDir = true;
                uiManager.CastingBar(id);
                RangedAttack(id);

                StartCoroutine(SpellOnCD(id));
            }
        }
    }

    public IEnumerator SpellOnCD(int id)
    {
        spellOnCD.Add(id);
        float cdTimer = 0.0f;

        while (cdTimer <= (playerSpellBook.playerOwnedSpells[id].getSpellCooldown() + playerSpellBook.playerOwnedSpells[id].getSpellCastTime()) )
        {
            cdTimer += Time.deltaTime;
            yield return null;
        }

        SpellOffCD(id);
    }

    private void SpellOffCD(int id)
    {
        StopCoroutine(SpellOnCD(id));
        //playerSpellBook.playerOwnedSpells[id].spellOnCD = false;
        spellOnCD.Remove(id);
    }

    public float getCurHP() { return curHP; }
    public float getMaxHP() { return hp.Value; }

    public float getCurMana() { return curMana; }
    public float getMaxMana() { return mana.Value; }

    public bool getManaUpdate() { return manaNeedsUpdating; }
    public bool getHPUpdate() { return hpNeedsUpdateing; }

}
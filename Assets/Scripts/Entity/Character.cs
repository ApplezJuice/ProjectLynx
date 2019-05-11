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

    public float healthTickRate;
    public float manaTickRate;

    public bool healthTickOn = false;
    public bool ManaTickOn = false;

    [SerializeField]
    //public GameObject rangedSpellPrefab;
    public RangedSpell currentSpellCasted;

    // Spell System
    //public Spell[] allSpells;
    //public Spell[] playerSpells;
    public SpellBook playerSpellBook;
    public HashSet<int> spellOnCD;

    public Character (string _entityName, float _maxHP, float _maxMana, float _atkPower, int _level, float _atkSpeed, float _dodge) 
        : base (_entityName, 100f, _maxMana, _atkPower, _level, _atkSpeed, _dodge)
    {

    }
    

    public void Start()
    {
        Init();
       
        StartCoroutine(HealOverTime());
        StartCoroutine(ManaOverTime());
    }

    public void Update()
    {

        

    }

    private void Init()
    {
        // add spells
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

        healthTickOn = true;
        ManaTickOn = true;

        SetBaseStats();
    }

    private void SetBaseStats()
    {
        Strength.BaseValue = 10f;
        Dexterity.BaseValue = 8f;
        Stamina.BaseValue = 10f;
        Intellect.BaseValue = 7f;
        dodge.BaseValue = 11f;
        CritChance.BaseValue = 7f;

        //resists
        FrostResist.BaseValue = 5f;
        FireResist.BaseValue = 6f;
        ArcaneResist.BaseValue = 5f;
        PoisonResist.BaseValue = 7f;
        ShadowResist.BaseValue = 3f;
    }

    public void RangedAttack(int id)
    {
        Vector3 SpawnSpellLoc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject clone;
        //clone = Instantiate(playerSpells[id].spellPrefab, SpawnSpellLoc, Quaternion.identity);
        clone = Instantiate(playerSpellBook.playerOwnedSpells[id].getSpellPrefab(), SpawnSpellLoc, Quaternion.identity);
        clone.GetComponent<RangedSpell>().spellID = id;

    }

    public void SetAdditionalStats()
    {
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

    IEnumerator HealOverTime()
    {
        while (healthTickOn)
        {
            if (curHP < hp.Value)
            {
                // need to play test this variable to get a good number to heal over time
                // 5% over 5 seconds
                if (curHP + ((healthTickRate/100) * hp.Value) >= hp.Value)
                {
                    curHP = hp.Value;
                    hpNeedsUpdateing = true;
                    //print(healthTickRate);
                }
                else
                {
                    curHP += ((healthTickRate / 100) * hp.Value);
                    hpNeedsUpdateing = true;
                    //print(healthTickRate);
                }

            }
            
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator ManaOverTime()
    {
        while (ManaTickOn)
        {
            if (curMana < mana.Value)
            {
                if (curMana + ((manaTickRate / 100) * mana.Value) >= mana.Value)
                {
                    curMana = mana.Value;
                    manaNeedsUpdating = true;
                    //print(healthTickRate);
                }
                else
                {
                    curMana += ((manaTickRate / 100) * mana.Value);
                    manaNeedsUpdating = true;
                    //print(healthTickRate);
                }

            }

            yield return new WaitForSeconds(5f);
        }
    }

    public float getCurHP() { return curHP; }
    public float getMaxHP() { return hp.Value; }

    public float getCurMana() { return curMana; }
    public float getMaxMana() { return mana.Value; }

    public bool getManaUpdate() { return manaNeedsUpdating; }
    public bool getHPUpdate() { return hpNeedsUpdateing; }

}
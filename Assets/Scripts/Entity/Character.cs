using UnityEngine;
using TMPro;


public class Character : EntityBase
{
    public UIManager uiManager;

    public bool hpNeedsUpdateing = true;
    public bool manaNeedsUpdating = true;
    public bool isCasting = false;
    public bool spellCasted = false;

    [SerializeField]
    public GameObject rangedSpellPrefab;

    public Character (string _entityName, float _maxHP, float _maxMana, float _atkPower, int _level, float _atkSpeed, float _dodge) : base (_entityName, 100f, _maxMana, _atkPower, _level, _atkSpeed, _dodge)
    {

    }
    

    public void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isCasting = true;
            RangedAttack();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StatModifier strMod = new StatModifier(10f, StatModType.Flat);
            Strength.AddModifier(strMod);
            uiManager.charSheetNeedsUpdating = true;
        }
        // END DEBUG TESTING

    }

    void RangedAttack()
    {
        Vector3 SpawnSpellLoc = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject clone;
        clone = Instantiate(rangedSpellPrefab, SpawnSpellLoc, Quaternion.identity);

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

    public float getCurHP() { return curHP; }
    public float getMaxHP() { return hp.Value; }

    public float getCurMana() { return curMana; }
    public float getMaxMana() { return mana.Value; }

    public bool getManaUpdate() { return manaNeedsUpdating; }
    public bool getHPUpdate() { return hpNeedsUpdateing; }

}
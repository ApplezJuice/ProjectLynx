using UnityEngine;
using TMPro;


public class EnemyTest : EntityBase
{
    public TextMeshPro enemyName;
    public EnemyTest(string _entityName, float _maxHP, float _maxMana, float _atkPower, int _level, float _atkSpeed, float _dodge) : base(_entityName, 100f, _maxMana, _atkPower, _level, _atkSpeed, _dodge)
    {

    }

    public void Start()
    {
        SetAdditionalStats();
        maxHP = 200f;
        curHP = 200f;
        hp.BaseValue = 200f;

        enemyName.SetText(entityName);

    }

    public void Update()
    {

    }

    public void SetAdditionalStats()
    {
        Strength.BaseValue = 50;
        StatModifier mod1 = new StatModifier(10, StatModType.Flat);
        Strength.AddModifier(mod1);
        //print(Strength.Value);

        dodge.BaseValue = 70;
        //print("Enemy: " + dodge.Value);
        dodge.AddModifier(mod1);
        //print("Enemy: " + dodge.Value);
        //print("Enemy: " + hp.Value);
    }

    public float getCurHP() { return curHP; }
    public float getMaxHP() { return maxHP; }

}
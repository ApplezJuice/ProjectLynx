using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public string entityName;
    public int level;

    protected float curHP;
    protected float maxHP;
    protected float curMana;
    protected float maxMana;

    public bool isDead = false;
    public bool isDamageable = true;

    //public float baseAttackPower;

    public CharacterStat attackPower;
    public CharacterStat attackSpeed;
    public CharacterStat hp;
    public CharacterStat mana;


    // base stats
    public CharacterStat Strength;
    public CharacterStat Dexterity;
    public CharacterStat Intellect;
    public CharacterStat Stamina;
    public CharacterStat CritChance;
    public CharacterStat dodge;

    // resists
    public CharacterStat FrostResist;
    public CharacterStat FireResist;
    public CharacterStat PoisonResist;
    public CharacterStat ArcaneResist;
    public CharacterStat ShadowResist;

    // all params provided
    public EntityBase(string _entityName, float _maxHP, float _maxMana, float _atkPower, int _level, float _atkSpeed, float _dodge)
    {
        entityName = _entityName;
        level = _level;
        maxHP = _maxHP;
        maxMana = _maxMana;

        attackPower.BaseValue = _atkPower;
        attackSpeed.BaseValue = _atkSpeed;
        dodge.BaseValue = _dodge;
        //hp.BaseValue = maxHP;

        //curHP = maxHP;
        curHP = hp.Value;
        curMana = mana.Value;
        //curAttackPower = _atkPower;
    }

    // no level - atk speed - dodge
    public EntityBase(string _entityName, float _maxHP, float _maxMana, float _atkPower) : this(_entityName, _maxHP, _maxMana, _atkPower, 1, 10f, 10f) { }

    // no level provided
    public EntityBase(string _entityName, float _maxHP, float _maxMana, float _atkPower, float _atkSpeed, float _dodge) : this(_entityName, _maxHP, _maxMana, _atkPower, 1, _atkSpeed, _dodge) { }


    public virtual void TakeDamage(float dmg)
    {
        if (isDamageable)
        {
            if (curHP - dmg <= 0)
            {
                Die();
            }
            else
            {
                curHP = Mathf.Max(curHP - dmg, 0);
            }
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public virtual void Reserect()
    {
        if (isDead)
        {
            isDead = false;
            curHP = (int)maxHP / 10;
        }
    }

    public virtual void HealSelf(float amnt)
    {
        if (curHP + amnt >= hp.Value)
        {
            curHP = hp.Value;
        }
        else
        {
            curHP += amnt;
        }
    }

    public virtual void UseMana(float amnt)
    {
        curMana = Mathf.Max(curMana - amnt, 0);
    }

    public virtual void GetMana(float amnt)
    {
        if (curMana + amnt >= mana.Value)
        {
            curMana = mana.Value;
        }
        else
        {
            curMana += amnt;
        }
    }
}

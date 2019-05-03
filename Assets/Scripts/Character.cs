using UnityEngine;


public class Character : MonoBehaviour
{
    public CharacterStat Strength;
    public Health healthComponent;


    public void Start()
    {
        healthComponent = GetComponent<Health>();
        StatModifier mod1 = new StatModifier(10, StatModType.Flat);
        Strength.AddModifier(mod1);
        print(Strength.Value);
    }

    public void Update()
    {
        // DEBUG TESTING
        if (Input.GetKeyDown(KeyCode.K))
        {
            healthComponent.TakeDamage(10f);
            print(healthComponent.health);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            healthComponent.HealSelf(10f);
            print(healthComponent.health);
        }
    }

}
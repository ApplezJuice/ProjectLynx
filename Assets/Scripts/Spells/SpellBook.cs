using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    [SerializeField]
    public List<SpellN> playerOwnedSpells;
    [SerializeField]
    public List<SpellN> allSpells;

    public SpellN CastSpell(int index)
    {
        if (playerOwnedSpells[index])
        {
            return playerOwnedSpells[index];
        }
        return null;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public UIManager uiManager;
    public bool xpNeedsUpdateing;
    public bool lvlNeedsUpdating;
    int curXP;
    int curLvl;
    public GameObject levelPrefab;

    Dictionary<int, int> levelDictionary = new Dictionary<int, int>();

    public int getCurXP() { return curXP; }
    public int getCurLvl() { return curLvl; }
    public int getMaxXP()
    {
        return levelDictionary[curLvl];
    }
    private void Awake()
    {
        curLvl = 1;
        curXP = 0;
        int baseStartXP = 98;
        float xpFormula;
        float prevXP;

        levelDictionary.Add(1, 98);

        for (int i = 2; i < 61; i++)
        {
            prevXP = levelDictionary[i - 1];
            xpFormula = baseStartXP + (prevXP * .1f + prevXP);
            levelDictionary.Add(i, (int)xpFormula);

            //print(levelDictionary[i]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        xpNeedsUpdateing = true;
        lvlNeedsUpdating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GainXP(10);
        }

    }

    void GainXP(int xpAmnt)
    {
        if ((curXP + xpAmnt) >= getMaxXP())
        {
            //curXP = getMaxXP();
            if (curLvl < 60)
            {
                curXP = (curXP + xpAmnt) - levelDictionary[curLvl];
                curLvl++;
                uiManager.charSheetNeedsUpdating = true;
                lvlNeedsUpdating = true;
                LevelUp();
            }
        }
        else
        {
            curXP += xpAmnt;
            
        }
        xpNeedsUpdateing = true;
    }

    void LevelUp()
    {
        Vector3 LevelSpawnLocation = new Vector3(this.transform.position.x, this.transform.position.y - .5f, this.transform.position.z);
        GameObject level;
        //clone = Instantiate(playerSpells[id].spellPrefab, SpawnSpellLoc, Quaternion.identity);
        level = Instantiate(levelPrefab, LevelSpawnLocation, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class handleLevel : MonoBehaviour
{
    public LevelSystem charLevelSystem;
    public TextMeshProUGUI lvlText;

    // Start is called before the first frame update
    void Start()
    {
        charLevelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
        lvlText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charLevelSystem.lvlNeedsUpdating)
        {
            UpdateLvlText();
        }
    }

    void UpdateLvlText()
    {
        lvlText.SetText(charLevelSystem.getCurLvl().ToString());
        charLevelSystem.lvlNeedsUpdating = false;
    }
}

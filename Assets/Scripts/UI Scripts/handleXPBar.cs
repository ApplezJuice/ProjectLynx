using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class handleXPBar : MonoBehaviour
{
    private Image content;
    private float currentFill;

    [SerializeField]
    private LevelSystem myCharXP;

    [SerializeField]
    private float lerpSpeed = 2f;

    public TextMeshProUGUI xpText;

    void Start()
    {
        myCharXP = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
        xpText = GetComponentInChildren<TextMeshProUGUI>();
        content = GetComponent<Image>();
        xpText.SetText("{0}/{1}", myCharXP.getCurXP(), myCharXP.getMaxXP());
    }

    // Update is called once per frame
    void Update()
    {
        if (myCharXP.getCurXP() == 0)
        {
            currentFill = 0;
        }
        else
        {
            currentFill = (float)myCharXP.getCurXP() / myCharXP.getMaxXP();
        }

        if (myCharXP.xpNeedsUpdateing)
        {
            UpdateXPBar();
        }
    }

    public void UpdateXPBar()
    {
        xpText.SetText("{0}/{1}", myCharXP.getCurXP(), myCharXP.getMaxXP());
        content.fillAmount = currentFill;
        myCharXP.xpNeedsUpdateing = false;
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CurrentMana : MonoBehaviour
{
    private Image content;
    private float currentFill;

    [SerializeField]
    private Character myChar;

    [SerializeField]
    private float lerpSpeed = 2f;

    public TextMeshProUGUI manaText;

    void Start()
    {
        manaText = GetComponentInChildren<TextMeshProUGUI>();
        content = GetComponent<Image>();
        manaText.SetText("{0}/{1}", myChar.getCurMana(),myChar.getMaxMana());
    }

    // Update is called once per frame
    void Update()
    {
        currentFill = myChar.getCurMana() / myChar.getMaxMana();

        if (myChar.manaNeedsUpdating)
        {
            UpdateManaBar();
        }

        if (Math.Round(currentFill,4) != Math.Round(content.fillAmount,4))
        {

            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void UpdateManaBar()
    {
        manaText.SetText("{0}/{1}", myChar.getCurMana(), myChar.getMaxMana());
        myChar.manaNeedsUpdating = false;
    }
}

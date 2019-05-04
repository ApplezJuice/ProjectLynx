using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CurrentHP : MonoBehaviour
{
    private Image content;
    private float currentFill;

    [SerializeField]
    private Character myChar;

    [SerializeField]
    private float lerpSpeed = 2f;

    public TextMeshProUGUI hpText;

    void Start()
    {
        hpText = GetComponentInChildren<TextMeshProUGUI>();
        content = GetComponent<Image>();
        hpText.SetText("{0}/{1}", myChar.getCurHP(),myChar.getMaxHP());
    }

    // Update is called once per frame
    void Update()
    {
        currentFill = myChar.getCurHP() / myChar.getMaxHP();

        if (myChar.hpNeedsUpdateing)
        {
            UpdateHPBar();
        }

        if (Math.Round(currentFill,4) != Math.Round(content.fillAmount,4))
        {

            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void UpdateHPBar()
    {
        hpText.SetText("{0}/{1}", myChar.getCurHP(), myChar.getMaxHP());
        myChar.hpNeedsUpdateing = false;
    }
}

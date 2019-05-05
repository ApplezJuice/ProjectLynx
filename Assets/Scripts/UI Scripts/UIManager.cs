using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image healthBar;
    public Image manaBar;
    public Image xpBar;

    public float healthBarCurrentFill;
    public float manaBarCurrentFill;
    public float xpBarCurrentFill;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI lvlText;
    public TextMeshProUGUI xpText;

    // Char Sheet
    public GameObject charSheet;
    public TextMeshProUGUI charSheetPlayerName;
    public TextMeshProUGUI charSheetHealth;
    public TextMeshProUGUI charSheetStr;
    public TextMeshProUGUI charSheetLvl;

    private bool showingCharSheet = false;
    public bool charSheetNeedsUpdating;

    public LevelSystem charLevelSystem;
    public Character myChar;
    public RangedSpell myCharRangedSpell;

    private float lerpSpeed = 2f;
    private float currentXPFill;
    private float currentManaFill;
    private float currentHPFill;

    // cast bar
    public SpellBook playerSpellBook;
    public Image castingBar;
    public TextMeshProUGUI castBarSpellTitle;
    public TextMeshProUGUI spellCastTime;
    public Image spellIcon;
    public CanvasGroup spellCastBarCanvasGroup;

    // action bar
    public Button[] actionButtons;
    private KeyCode action1, action2, action3;

    // Start is called before the first frame update
    void Start()
    {
        myCharRangedSpell = GameObject.FindGameObjectWithTag("Player").GetComponent<RangedSpell>();
        playerSpellBook = GameObject.FindGameObjectWithTag("Player").GetComponent<SpellBook>();

        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;

        hpText.SetText("{0}/{1}", myChar.getCurHP(), myChar.getMaxHP());
        manaText.SetText("{0}/{1}", myChar.getCurMana(), myChar.getMaxMana());
        xpText.SetText("{0}/{1}", charLevelSystem.getCurXP(), charLevelSystem.getMaxXP());

        charSheetNeedsUpdating = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentManaFill = myChar.getCurMana() / myChar.getMaxMana();
        currentHPFill = myChar.getCurHP() / myChar.getMaxHP();

        // handle action bars
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
            //myCharRangedSpell.needPlayerDir = true;
            //myChar.isCasting = true;
            //myChar.RangedAttack();
        }
        if (Input.GetKeyDown(action2))
        {
            //myChar.UsedSpell(myChar.playerSpells[0].id);
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(action3))
        {
            //myChar.UsedSpell(myChar.playerSpells[0].id);
            ActionButtonOnClick(0);
        }

        // character sheet

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (showingCharSheet)
            {
                charSheet.SetActive(false);
                showingCharSheet = false;
            }
            else
            {
                charSheet.SetActive(true);
                showingCharSheet = true;
            }
        }

        // handle char sheet updates
        if (charSheetNeedsUpdating)
        {
            UpdateCharSheet();
        }

        // handle xp bar
        if (charLevelSystem.getCurXP() == 0)
        {
            currentXPFill = 0;
        }
        else
        {
            currentXPFill = (float)charLevelSystem.getCurXP() / charLevelSystem.getMaxXP();
        }

        if (charLevelSystem.xpNeedsUpdateing)
        {
            UpdateXPBar();
        }

        // handle lvl number
        if (charLevelSystem.lvlNeedsUpdating)
        {
            UpdateLvlText();
        }

        // handle mana bar
        if (myChar.manaNeedsUpdating)
        {
            UpdateManaBar();
        }

        if (Math.Round(currentManaFill, 4) != Math.Round(manaBar.fillAmount, 4))
        {

            manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, currentManaFill, Time.deltaTime * lerpSpeed);
        }

        // handle hp bar
        if (myChar.hpNeedsUpdateing)
        {
            UpdateHPBar();
        }

        if (Math.Round(currentHPFill, 4) != Math.Round(healthBar.fillAmount, 4))
        {

            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHPFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void UpdateCharSheet()
    {
        charSheetPlayerName.SetText(myChar.entityName);
        charSheetHealth.SetText("{0}", myChar.getMaxHP());
        charSheetStr.SetText("{0}", myChar.Strength.Value);
        charSheetLvl.SetText("{0}", charLevelSystem.getCurLvl());
    }

    public void UpdateHPBar()
    {
        hpText.SetText("{0}/{1}", myChar.getCurHP(), myChar.getMaxHP());
        myChar.hpNeedsUpdateing = false;
    }

    public void UpdateManaBar()
    {
        manaText.SetText("{0}/{1}", myChar.getCurMana(), myChar.getMaxMana());
        myChar.manaNeedsUpdating = false;
    }

    void UpdateLvlText()
    {
        lvlText.SetText(charLevelSystem.getCurLvl().ToString());
        charLevelSystem.lvlNeedsUpdating = false;
    }

    public void UpdateXPBar()
    {
        xpText.SetText("{0}/{1}", charLevelSystem.getCurXP(), charLevelSystem.getMaxXP());
        xpBar.fillAmount = currentXPFill;
        charLevelSystem.xpNeedsUpdateing = false;
    }

    public void ActionButtonOnClick(int btnIndex)
    {
        actionButtons[btnIndex].onClick.Invoke();
    }

    public void CastingBar(int id)
    {
        castingBar.fillAmount = 0;
        castingBar.color = new Color(playerSpellBook.playerOwnedSpells[id].getSpellBarColor().r,
                                     playerSpellBook.playerOwnedSpells[id].getSpellBarColor().g,
                                     playerSpellBook.playerOwnedSpells[id].getSpellBarColor().b,1f);
        castBarSpellTitle.SetText(playerSpellBook.playerOwnedSpells[id].getSpellName());
        spellIcon.sprite = playerSpellBook.playerOwnedSpells[id].getIcon();
        //print(playerSpellBook.playerOwnedSpells[id].getSpellBarColor());

        StartCoroutine(Progress(id));
        StartCoroutine(FadeBar());
    }

    private IEnumerator Progress(int id)
    {
        float timeLeft = Time.deltaTime;
        float rate = 1.0f / playerSpellBook.playerOwnedSpells[id].getSpellCastTime();
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;
            timeLeft += Time.deltaTime;
            spellCastTime.SetText((playerSpellBook.playerOwnedSpells[id].getSpellCastTime() - timeLeft).ToString("F1"));
            if ((playerSpellBook.playerOwnedSpells[id].getSpellCastTime() - timeLeft) <= 0f)
            {
                spellCastTime.SetText("0.0");
            }
            yield return null;
        }
        StopCasting(id);

    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.5f;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            spellCastBarCanvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    private void StopCasting(int id)
    {
        StopCoroutine(FadeBar());
        StopCoroutine(Progress(id));
        spellCastBarCanvasGroup.alpha = 0;
    }
}

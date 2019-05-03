using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentHP : MonoBehaviour
{
    private Image content;
    private float currentFill;
    private float currentValue;
    private Health currentHP;

    [SerializeField]
    private float lerpSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
        currentHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        currentFill = currentHP.health / 100;
        if (content.fillAmount != currentFill)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
        //print(mainChar.HP.Value / 10);
    }
}

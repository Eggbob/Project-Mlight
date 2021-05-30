using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{

    [SerializeField]
    private Image imageCoolDown;

    [SerializeField]
    private TMP_Text textCoolDown;
    private Button btn;

    private bool isCooldown = false;
    private float coolDownTime = 10f;
    private float timer = 0f;

    void Start()
    {
        btn = this.GetComponent<Button>();
        textCoolDown.gameObject.SetActive(false);
        imageCoolDown.fillAmount = 0.0f;
    }


    void Update()
    {       
        if(isCooldown)
        {
            ApplyCoolDown();
        }
    }

    void ApplyCoolDown()
    {
        timer -= Time.deltaTime;

        if(timer < 0.0f)
        {
            isCooldown = false;
            textCoolDown.gameObject.SetActive(false);
            imageCoolDown.fillAmount = 0.0f;
            btn.interactable = true;
        }
        else
        {
            textCoolDown.text = Mathf.RoundToInt(timer).ToString();
            imageCoolDown.fillAmount = timer / coolDownTime;
            btn.interactable = false;
        }
    }

    public void UseSpell(float coolTime)
    {
        if(!isCooldown)
        {
            isCooldown = true;
            coolDownTime = coolTime;
            timer = coolDownTime;
            textCoolDown.gameObject.SetActive(true);
        }
    }
}

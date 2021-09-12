using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RegistUserManager : MonoBehaviour
{

    public Text titleTxt;

    public InputField nameInput;
    public InputField idInput;
    public InputField passInput;
    public InputField conPassInput;

    public Button createBtn;


    private void Start()
    {
        createBtn.onClick.AddListener(() => RegistUser());
    }


    //유저 생성
    private void RegistUser()
    {
        if(string.IsNullOrEmpty(nameInput.text) || string.IsNullOrEmpty(idInput.text) ||
            string.IsNullOrEmpty(passInput.text) || string.IsNullOrEmpty(conPassInput.text))
        {
            titleTxt.text = "모든 항목을 입력해 주세요!";
            return;
        }

        if(!passInput.text.Trim().Equals(conPassInput.text.Trim()))
        {
            titleTxt.text = "비밀번호가 다릅니다!";
            return;
        }


        Action<bool> registerCallBack = (hasuser) =>
        {
            if (!hasuser)
            {
                titleTxt.text = "이미 존재하는 아이디 입니다!";
                return;
            }
            else
            {
                this.gameObject.SetActive(false);
                return;
            }
        };

        StartCoroutine(WebManager.Instance.RegisterUser(idInput.text, passInput.text, nameInput.text,
            registerCallBack));

    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public Text titleTxt;

    public InputField idField;
    public InputField passField;

    public Button loginBtn;


    private void Start()
    {
        loginBtn.onClick.AddListener(() =>
        {
            LoginRoutine();
        });
    }

    //로그인 처리과정
    private void LoginRoutine()
    {
        if(string.IsNullOrEmpty(idField.text) || string.IsNullOrEmpty(passField.text))
        {
            titleTxt.text = "모든 항목을 입력해 주세요!";
            return;
        }


        Action<bool> loginCallBack = (hasUser) =>
        {
            if(!hasUser)
            {
                titleTxt.text = "존재하지 않는 사용자 입니다!";
                return;
            }
            else
            {
                TitleManager.Instance.LogIn(true);
                gameObject.SetActive(false);
            }
        };


        StartCoroutine(WebManager.Instance.LoginUser(idField.text, passField.text, loginCallBack));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private Button loginBtn;

    [SerializeField]
    private Button logOutBtn;

    [SerializeField]
    private Button exitBtn;

    private void Start()
    {
        loginBtn.onClick.AddListener(() => LogIn());
        logOutBtn.onClick.AddListener(() => LogOut());

        startBtn.onClick.AddListener(() => LoadScene());
        exitBtn.onClick.AddListener(() => ExitGame());

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        LogIn();

    }

    //게임시작
    private void LoadScene()
    {
        LoadingManager.LoadScene("MainScene");
    }

    //게임 종료
   private void ExitGame()
   {
        Application.Quit();
   }

    //로그인하기
    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                startBtn.gameObject.SetActive(true);
                loginBtn.gameObject.SetActive(false);
                logOutBtn.gameObject.SetActive(true);
            }
            else
            {
                loginBtn.gameObject.SetActive(true);
                startBtn.gameObject.SetActive(false);
                logOutBtn.gameObject.SetActive(false);
            }
        });

    }

    //로그아웃 하기
    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();

        loginBtn.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(false);
        logOutBtn.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private static TitleManager instance;

    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private Button loginBtn;

    [SerializeField]
    private Button localLoginBtn;

    [SerializeField]
    private Button logOutBtn;

    [SerializeField]
    private Button exitBtn;



    public static TitleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TitleManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TitleBgmManager.Instance.PlayBgm("Title");

        loginBtn.onClick.AddListener(() =>
        {
            Social.localUser.Authenticate((bool success) =>
            {
                LogIn(success);
            });
        });

        logOutBtn.onClick.AddListener(() =>
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
            LogOut();
        });
      

        startBtn.onClick.AddListener(() => LoadScene());
        exitBtn.onClick.AddListener(() => ExitGame());

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

    }

    //게임시작
    private void LoadScene()
    {
        TitleBgmManager.Instance.StopBgm();
        LoadingManager.LoadScene("MainScene");
    }

    //게임 종료
   private void ExitGame()
   {
        Application.Quit();
   }

    //로그인하기
    public void LogIn(bool success)
    {    
        if (success)
        {
            startBtn.gameObject.SetActive(true);
            loginBtn.gameObject.SetActive(false);
            logOutBtn.gameObject.SetActive(true);
            localLoginBtn.gameObject.SetActive(false);
        }
        else
        {
            loginBtn.gameObject.SetActive(true);
            startBtn.gameObject.SetActive(false);
            logOutBtn.gameObject.SetActive(false);
            localLoginBtn.gameObject.SetActive(true);
        }
    }

    //로그아웃 하기
    public void LogOut()
    {
        loginBtn.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(false);
        logOutBtn.gameObject.SetActive(false);
        localLoginBtn.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button loginBtn;

    [SerializeField]
    private Button exitBtn;

    private void Start()
    {
        loginBtn.onClick.AddListener(() => LoadScene());
        exitBtn.onClick.AddListener(() => ExitGame());
    }

    private void LoadScene()
    {
        LoadingManager.LoadScene("MainScene");
    }

   private void ExitGame()
   {
        Application.Quit();
   }
}

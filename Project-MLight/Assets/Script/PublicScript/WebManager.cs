using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : MonoBehaviour
{
    private static WebManager instance;

    public static WebManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<WebManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public IEnumerator RegisterUser(string id, string password, string userName, Action<bool> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginID", id);
        form.AddField("loginPass", password);
        form.AddField("userName", userName);

        using (UnityWebRequest www = UnityWebRequest.Post("http://dhm04155.dothome.co.kr/RegisterUserInform.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text.Equals("TRUE"))
                {
                    callback(true);
                }
                else if (www.downloadHandler.text.Equals("FALSE"))
                {
                    callback(false);
                }


                Debug.Log(www.downloadHandler.text);
            }

        }
    }


    public IEnumerator LoginUser(string id, string password, Action<bool> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginID", id);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://dhm04155.dothome.co.kr/LogInUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text.Equals("TRUE"))
                {
                    callback(true);
                }
                else if (www.downloadHandler.text.Equals("FALSE"))
                {
                    callback(false);
                }

            }
        }

    }

}

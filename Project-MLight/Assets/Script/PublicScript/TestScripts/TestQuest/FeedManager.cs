using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//퀘스트 진척도 표시
public class FeedManager : MonoBehaviour
{
    private static FeedManager instance;

    private GameObject messagePrefab;

    public static FeedManager Instance 
    { 
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<FeedManager>();               
            }
            return instance;
        }
    }

    public void WriteMessage(string message)
    {
        GameObject go = Instantiate(messagePrefab, transform);
      
        go.GetComponent<Text>().text = message;

        go.transform.SetAsFirstSibling();

        Destroy(go, 2f);
    }
}

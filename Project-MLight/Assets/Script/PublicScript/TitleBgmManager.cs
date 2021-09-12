using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBgmManager : MonoBehaviour
{
    private static TitleBgmManager instance;

    private Dictionary<string, AudioClip> bgmDic = new Dictionary<string, AudioClip>();


    public static TitleBgmManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TitleBgmManager>();
            }
            return instance;
        }
    }

    public BgmData[] bgmData;
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < bgmData.Length; i++)
        {
            bgmDic[bgmData[i].bgnName] = bgmData[i].clip;
        }
    }

    public void PlayBgm(string name)
    {
        audioSource.clip = bgmDic[name];
        audioSource.Play();
    }

    public void StopBgm()
    {
        audioSource.Stop();
    }

    public void PlayEffectSound(string name)
    {
        audioSource.PlayOneShot(bgmDic[name], 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private static BgmManager instance;

    public static BgmManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BgmManager>();
            }
            return instance;
        }
    }

    public AudioSource backASource;
    public AudioSource effectASource;
    public Dictionary<string, AudioClip> bgmDic = new Dictionary<string, AudioClip>();
    public BgmData[] bgmData;

    [System.Serializable]
    public struct BgmData
    {
        public string bgnName;
        public AudioClip clip;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i=0; i<bgmData.Length; i++)
        {
            bgmDic[bgmData[i].bgnName] = bgmData[i].clip;
        }
    }

    public void PlayBgm(string name)
    {
        backASource.clip = bgmDic[name];
        backASource.Play();
    }

    public void StopBgm()
    {
        backASource.Stop();
    }

    public void PlayEffectSound(string name)
    {
        effectASource.PlayOneShot(bgmDic[name], 0.3f);
    }

    public void SetBgmVolume(float volume)
    {
        backASource.volume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        effectASource.volume = volume;
    }

}

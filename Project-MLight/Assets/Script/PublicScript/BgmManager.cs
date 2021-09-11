using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


[System.Serializable]
public struct BgmData
{
    public string bgnName;
    public AudioClip clip;
}

public class BgmManager : MonoBehaviour
{
    private static BgmManager instance;

    private Dictionary<string, AudioClip> bgmDic = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> eBgmDic = new Dictionary<string, AudioClip>();

    private float bVol = 1f;
    private float eVol = 1f;

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

    public AudioSource backAudio;
    public AudioSource eAudio;

    public Slider bSlider;
    public Slider eSlider;

    public BgmData[] bgmData;
    public BgmData[] effectBgmData;


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

        for(int i = 0; i<effectBgmData.Length; i++)
        {
            eBgmDic[effectBgmData[i].bgnName] = effectBgmData[i].clip;
        }


        bVol = PlayerPrefs.GetFloat("bVol", 1f);
        bSlider.value = bVol;
        backAudio.volume = bSlider.value;

        eVol = PlayerPrefs.GetFloat("eVol", 1f);
        eSlider.value = eVol;
        eAudio.volume = eSlider.value;

    }

    private void Update()
    {
        SetBgmVolume();
        SetEffectVolume();
    }

    public void PlayBgm(string name)
    {
        backAudio.clip = bgmDic[name];
        backAudio.Play();
    }

    public void StopBgm()
    {
        backAudio.Stop();
    }

    public void PlayEffectSound(string name)
    {
        eAudio.PlayOneShot(eBgmDic[name], 0.5f);
    }


    public void PlayCharacterSound(AudioClip aClip)
    {
        eAudio.PlayOneShot(aClip);
    }

    public void SetBgmVolume()
    {
        // backASource.volume = volume;
        backAudio.volume = bSlider.value;

        bVol = bSlider.value;
        PlayerPrefs.SetFloat("bVol", bVol);
    }

    public void SetEffectVolume()
    {
        // effectASource.volume = volume;

        eAudio.volume = eSlider.value;

        eVol = eSlider.value;
        PlayerPrefs.SetFloat("eVol", eVol);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    static public AudioMgr _instacne=null;

    public float MusicVol = 1f; //背景音乐
    public float EffectVol = 1f;//音效


    public static AudioMgr Instance
    {
        get
        {
            if (_instacne == null)
            {
                Debug.LogError("AudioMgr Awake error");
            }
            return _instacne;
        }
    }

    private void Awake()
    {
        if (_instacne == null)
        {
            DontDestroyOnLoad(gameObject);
            AudioMgr._instacne = gameObject.GetComponent<AudioMgr>();
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            MusicVol = PlayerPrefs.GetFloat("MusicVol");
        }
        else {
            PlayerPrefs.SetFloat("MusicVol",MusicVol);
        }

        if (PlayerPrefs.HasKey("EffectVol"))
        {
            EffectVol = PlayerPrefs.GetFloat("EffectVol");
        }
        else
        {
            PlayerPrefs.SetFloat("EffectVol", EffectVol);
        }


        //设置音量
        SetVolume(1, MusicVol);
        SetVolume(2, EffectVol);

        //播放背景音乐
        PlayBgm("Sound/bg-sound");
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="type">1:背景音乐 2：音效</param>
    /// <param name="volume"></param>
    public void SetVolume(int type,float volume)
    {
        if (type == 1)
        {
            MusicVol = volume;
            PlayerPrefs.SetFloat("MusicVol", volume);
            transform.Find("Background").GetComponent<AudioSource>().volume = volume;
        }
        else if(type == 2)
        {
            EffectVol = volume;
            PlayerPrefs.SetFloat("EffectVol", volume);
            foreach (AudioSource item in transform.Find("Effects").GetComponentsInChildren<AudioSource>())
            {
                item.volume = volume;
            } 
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBgm(string _audioName="")
    {
        AudioSource audioSource = transform.Find("Background").GetComponent<AudioSource>();
        audioSource.volume = MusicVol;
        audioSource.clip = Resources.Load<AudioClip>(_audioName==""? "Sound/bg-sound":_audioName);
        audioSource.loop = true;
        audioSource.Play();
    }
    /// <summary>
    /// 暂停播放背景
    /// </summary>
    public void PauseBgm()
    {
        AudioSource audioSource = transform.Find("Background").GetComponent<AudioSource>();
        audioSource.Pause();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_audioName"></param>
    /// <param name="_delay"></param>
    public void PlayAudio(string _audioName, float _delay = 0f)
    {
        GameObject effectSound = new GameObject();
        AudioSource sound = effectSound.AddComponent<AudioSource>();
        effectSound.transform.parent = transform.Find("Effects");

        sound.clip = Resources.Load<AudioClip>(_audioName);
        sound.loop = false;
        sound.volume = EffectVol;
       
        sound.PlayDelayed(_delay);

        Destroy(effectSound, sound.clip.length);
    }

    /// <summary>
    /// 按钮音效
    /// </summary>
    public void PlayBtnAudio()
    {
        PlayAudio("Sound/btn");
    }

}

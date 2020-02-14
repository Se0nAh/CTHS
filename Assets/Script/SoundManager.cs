using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //코드 참고 https://stuban.tistory.com/43

    private static SoundManager _Instance = null;

    public static SoundManager I
    {
        get
        {
            if (_Instance == null)
            {
                Debug.Log("instance is null");
            }
            return _Instance;
        }
    }

    void Awake()
    {
        if( _Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _Instance = this;
        DontDestroyOnLoad(this);
    }

    public int audioSourceCount;

    public AudioClip[] BGMs = new AudioClip[2];
    public AudioClip[] SFXs = new AudioClip[7];


    private AudioSource BGMsource;
    private AudioSource[] SFXsource;

    
    void OnEnable()
    {
        //float volume = PlayerPrefs.GetFloat("volumeBGM", 0.8f);
        float volume = 0.8f;
        BGMsource = gameObject.AddComponent<AudioSource>();
        BGMsource.volume = volume;
        BGMsource.playOnAwake = false;
        BGMsource.loop = true;

        //sfx 소스 초기화
        SFXsource = new AudioSource[audioSourceCount];

        volume = PlayerPrefs.GetFloat("volumeSFX", 1);

        for (int i = 0; i < SFXsource.Length; i++)
        {
            SFXsource[i] = gameObject.AddComponent<AudioSource>();
            SFXsource[i].playOnAwake = false;
            SFXsource[i].volume = volume;
        }

        ChangeBGM("openingBgm", false);
    }

    public void PlaySFX(string name, bool loop = false, float pitch = 1)//효과음 재생
    {
        for (int i = 0; i < SFXs.Length; i++)
        {
            if (SFXs[i].name == name)
            {
                AudioSource a = GetEmptySource();
                a.loop = loop;
                a.pitch = pitch;
                a.clip = SFXs[i];
                a.Play();
                return;
            }
        }
    }

    public void StopSFXByName(string name)
    {
        for (int i = 0; i < SFXsource.Length; i++)
        {
            if (SFXsource[i].clip.name == name)
                SFXsource[i].Stop();
        }
    }


    private AudioSource GetEmptySource()//비어있는 오디오 소스 반환
    {
        int lageindex = 0;
        float lageProgress = 0;
        for (int i = 0; i < SFXsource.Length; i++)
        {
            if (!SFXsource[i].isPlaying)
            {
                return SFXsource[i];
            }

            //만약 비어있는 오디오 소스를 못찿으면 가장 진행도가 높은 오디오 소스 반환(루프중인건 스킵)

            float progress = SFXsource[i].time / SFXsource[i].clip.length;
            if (progress > lageProgress && !SFXsource[i].loop)
            {
                lageindex = i;
                lageProgress = progress;
            }
        }
        return SFXsource[lageindex];
    }

    /**********BGM***********/

    private AudioClip changeClip;//바뀌는 클립
    //private bool isChanging = false;
    private float startTime;
    
    public float ChangingSpeed;


    public void ChangeBGM(string name, bool isSmooth = false)//브금 변경 (브금이름 , 부드럽게 바꾸기)
    {

        changeClip = null;
        for (int i = 0; i < BGMs.Length; i++)//브금 클립 탐색
        {
            if (BGMs[i].name == name)
            {
                changeClip = BGMs[i];
            }
        }

        if (changeClip == null)//없으면 탈주
            return;

        if (!isSmooth)
        {
            BGMsource.clip = changeClip;
            BGMsource.Play();
        }
        else
        {
            startTime = Time.time;
            //isChanging = true;
        }
    }

    public void StopBGM()
    {
        BGMsource.Stop();
    }

    public void SetPitch(float pitch)
    {
        BGMsource.pitch = pitch;
    }

    public void changeBGMVolume(float volume)
    {
        //PlayerPrefs.SetFloat("volumeBGM", volume);
        BGMsource.volume = volume;
    }

    public void changeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("volumeSFX", volume);
        for (int i = 0; i < SFXsource.Length; i++)
        {
            SFXsource[i].volume = volume;
        }
    }

    public void ChangePitch()
    {
        StartCoroutine(pitchDown());

    }

    public void ChangePitchBack()
    {
        BGMsource.pitch = 1f;
    }

    IEnumerator pitchDown()
    {
        float pitch = 1f;
        for (pitch = 1f; pitch > 0; pitch -= 0.05f)
        {
            BGMsource.pitch = pitch;
            yield return new WaitForSeconds(0.1f);
        }

    }

    /*
    public AudioClip bgm1, bgm2;
    public AudioClip cowSound;
    public AudioClip btClick;
    public AudioClip ticking;
    public AudioClip right, wrong;
    public AudioClip coin;
    public AudioClip timeOver;
    
    */
}

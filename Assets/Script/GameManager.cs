using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject Hearts;
    public GameObject PauseButton;
    public GameObject PauseMenu;
    public GameObject ttsPanel;
    public bool IsDead=false;//GameOver를 할지 안 할지
    public bool IsPaused = false;//일시정지

    public Image[] lifeHearts;//목숨 이미지
    public Sprite heart;
    public Slider TimeSlider;
    static GameManager _instance = null;//싱글톤 패턴 인스턴스

    bool isTicking = false; //시간초 효과음 재생 중인지
    bool start = false;
    void Awake()
    {
        Time.timeScale = 0f;
        if (_instance == null)//싱글톤 인스턴스 없다면
        {
            _instance = this;
        }
        else//현재 인스턴스로 결정
        {
            Destroy(gameObject);
        }
        
    }

    void Start()//세팅
    {   

        PauseMenu.SetActive(false);
        ttsPanel.SetActive(true);
        Buttons.SetActive(false);
        PauseButton.SetActive(false);

        SoundManager.I.changeBGMVolume(0f);
        for (int i = 0; i < 3; i++)//시작할 때 마다 하트 스프라이트 띄우기 
        {
            lifeHearts[i].enabled = true;
        }       
    }

    void Update()
    {   if(!start && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            start = true;
            ttsPanel.SetActive(false);
            SoundManager.I.PlaySFX("cow", false, 1);
            SoundManager.I.ChangeBGM("playBgm");
            SoundManager.I.changeBGMVolume(1f);
            Buttons.SetActive(true);
            PauseButton.SetActive(true);
            Time.timeScale = 1f;
        }
        if (start == true && IsDead == false)
        {
            if (!IsPaused) //일시정지 상태가 아니면
            {
                TimeSlider.value -= 1f*Time.deltaTime;//시간 줄어드는건데 기기당 성능달라서 시간달라짐
            }
            if (!isTicking && TimeSlider.value <= 6f)   //시간초 6초 남았을때 효과음 시작, 배경음 감소
            {
                isTicking = true;
                timeTicking();
                SoundManager.I.changeBGMVolume(0.4f);
            }
            if (TimeSlider.value <= 0)
            {
                IsDead = true;
                TimeOver();//영업종료
            }
        }
    }

    public static GameManager Instance() //다른 곳에서 인스턴스가져오기
    {
        return _instance;
    }

    public void Life()//목숨 이미지
    {
        int life = GameObject.Find("GameManager").GetComponent<ButtonClick>().FailureHairCount;//실패한 머리의 개수를 life에 가져옴

        switch (life)
        {
            case 1:
                lifeHearts[2].enabled = false;
                break;
            case 2:
                lifeHearts[1].enabled = false;
                break;
            case 3:
                {
                    lifeHearts[0].enabled = false;
                    IsDead = true;
                    GameOver();//게임오버
                    break;
                }
         }
    }

    public void PausePlay()
    {
        if (IsPaused) //다시 플레이
        {
            if(isTicking) SoundManager.I.changeBGMVolume(0.4f);
            else
                SoundManager.I.changeBGMVolume(0.8f);
            PauseButton.SetActive(true);
            PauseMenu.SetActive(false);
            Time.timeScale = 0f;
            IsPaused = false;
        }
        else //일시정지 화면
        {
            SoundManager.I.changeBGMVolume(0f);
            PauseButton.SetActive(false);
            PauseMenu.SetActive(true);
            Time.timeScale = 1f;
            IsPaused = true;
        }
    }

    public void OnClicked_restart()
    {
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().TextReset();
        SoundManager.I.ChangeBGM("playBgm", false);
        SoundManager.I.changeBGMVolume(0.8f);
        SceneManager.LoadScene("Play");
    }

    public void OnClicked_main()
    {
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().TextReset();
        SoundManager.I.ChangeBGM("openingBgm", false);
        SoundManager.I.changeBGMVolume(0.8f);
        SceneManager.LoadScene("Starting");
    }

    public void OnClickSound()
    {
        SoundManager.I.PlaySFX("click1", false, 1f);
    }

    void GameOver()//게임오버
    {
        Buttons.SetActive(false);
        Hearts.SetActive(false);
        PauseButton.SetActive(false);
        SoundManager.I.ChangePitch();
        TimeSlider.value = 90;//TimeSlider Value 초기화
        TimeSlider.gameObject.SetActive(false);//타임슬라이더 비활성화 //구글링 해보니 Value 초기화 하고 슬라이더 비활성화 해야 하는 것 같았음

        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().ShowScore();

        SceneManager.LoadScene("GameOver");
    }

    void TimeOver()//영업종료 
    {
        Buttons.SetActive(false);
        Hearts.SetActive(false);
        PauseButton.SetActive(false);

        SoundManager.I.PlaySFX("gameover", false, 0.7f);

        TimeSlider.value = 180;//TimeSlider Value 초기화
        TimeSlider.gameObject.SetActive(false);//타임슬라이더 비활성화 //구글링 해보니 Value 초기화 하고 슬라이더 비활성화 해야 하는 것 같았음

        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().ShowScore();

        SceneManager.LoadScene("TimeOver");
    }

    void timeTicking()
    {
        SoundManager.I.PlaySFX("ticking2", false, 1f);    //째깍 소리 근데 너무...작아서 안들림
    }

}

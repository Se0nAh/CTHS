using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeOver_script : MonoBehaviour
{
    public Text Score;
    public Text BestScore;

    public Image DarkBackground;
    public Image ScoreBackground;
    public Image Coin;
    public Image ScoreImage;
    public Image BestScoreImage;

    public GameObject Main;
    public GameObject Restart;


    void Start()
    {
        PlayerPrefs.SetFloat("Best_Score", GameObject.Find("ScoreManager").GetComponent<ScoreManager>().Best_Score);//bestscore를 저장
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().Best_Score = PlayerPrefs.GetFloat("Best_Score");//변수에 bestscore를 불러옴

        Score.enabled = false;
        Score.text = "" + GameObject.Find("ScoreManager").GetComponent<ScoreManager>().Score_Playing;
        BestScore.enabled = false;
        BestScore.text = "" + GameObject.Find("ScoreManager").GetComponent<ScoreManager>().Best_Score;

        DarkBackground.enabled = false;
        ScoreBackground.enabled = false;
        Coin.enabled = false;
        ScoreImage.enabled = false;
        BestScoreImage.enabled = false;

        Main.SetActive(false);
        Restart.SetActive(false);

        Invoke("ShowScore", 1.3f);
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

    void ShowScore()
    {
        Score.enabled = true;
        BestScore.enabled = true;

        DarkBackground.enabled = true;
        ScoreBackground.enabled = true;
        Coin.enabled = true;
        ScoreImage.enabled = true;
        BestScoreImage.enabled = true;

        Main.SetActive(true);
        Restart.SetActive(true);
    }
}

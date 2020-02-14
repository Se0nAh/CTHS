using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText;//플레이 시 점수

    public float Score_Playing = 0f;//게임 중 나의 점수
    public int ForSuccessCountScore = 0;//성공횟수
    static float My_Score = 0f;//이번 게임 내 점수
    public float Best_Score = 0f;//최고점수

    private void Awake()
    {
        Best_Score = PlayerPrefs.GetFloat("Best_Score");
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(float score)//점수더하기
    {
        Score_Playing += score;//점수 더하기
        ScoreText.text = "" + Score_Playing;//머니 띄우기
        My_Score = Score_Playing;

        if (My_Score > Best_Score) //BestScore 값보다 더 크면
        {
            Best_Score = My_Score;
        }
    } 

    public void SuccessCount()
    {
        ForSuccessCountScore++;
    }

    public void ShowScore()
    {
        ScoreText.enabled = false;//게임 도중 점수 화면에서 숨기기
    }

    public void TextReset()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_script : MonoBehaviour
{
    public Image GameoverImage;
    public float animTime = 2f;
    private float start = 1f;
    private float end = 0f;
    private float time = 0f;

    public Coroutine co;


    private void Start()
    {
        co = StartCoroutine(Fadein());
    }

    public void OnClicked_restart()
    {
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().TextReset();
        SoundManager.I.ChangePitchBack();
        SceneManager.LoadScene("Play");
    }

    public void OnClicked_main()
    {
        SoundManager.I.ChangeBGM("openingBgm", false);
        SoundManager.I.ChangePitchBack();
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().TextReset();
        SceneManager.LoadScene("Starting");
    }

    public void OnClickSound()
    {
        SoundManager.I.PlaySFX("click1", false, 1f);
    }

    IEnumerator Fadein()
    {
        Color color = GameoverImage.color;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a > 0f)
        {
            time += Time.deltaTime / animTime;//2초동안 재생될 수 있게 animtime으로 나눔
            color.a = Mathf.Lerp(start, end, time);
            GameoverImage.color = color;
            yield return null;
        }
        SoundManager.I.changeBGMVolume(0f);
        yield break;
    }

}


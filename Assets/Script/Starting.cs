using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Starting : MonoBehaviour
{
    public Image Image;
    public GameObject ttsPanel;
    public GameObject ttsText;
    bool waitingTouch = false;

    void Start()
    {
        SoundManager.I.changeBGMVolume(1f);
        Image.gameObject.SetActive(false);
        ttsPanel.SetActive(false);
    }

    void Update()
    {
        if (waitingTouch && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("Play");
        }
    }

    public void startBt()
    {
        StartCoroutine("touchToStart");
    }

    public void OnButtonClick()
    {
        SoundManager.I.PlaySFX("click1", false, 1);
    }

    public void Onclicked()
    {

        SceneManager.LoadScene("Play");
    }

    public void OnclickDevelop()
    {
        Debug.Log("DEV");
        Image.gameObject.SetActive(true);
    }

    public void TouchImage()
    {
        Image.gameObject.SetActive(false);
    }

    //매니저...싱글톤 사운드 매니저가필요합니다


    IEnumerator touchToStart()
    {
        float volume = 1f;
        bool ttsStat = true;
        Image ttsImage = ttsPanel.gameObject.GetComponent<Image>();
        ttsImage.color = new Color(255, 255, 255, 0);
        ttsPanel.SetActive(true);
        for (float i = 0f; i <= 1.5f; i += 0.1f)
        {
            Color color = new Color(255, 255, 255, i);
            ttsImage.color = color;
            volume -= 0.05f;
            Debug.Log(volume);
            SoundManager.I.changeBGMVolume(volume);
            yield return new WaitForSeconds(0.04f);
        }
        Debug.Log("페이드인 끝");
        waitingTouch = true;
        for (int i = 0; i < 30; i++)
        {
            ttsStat = !ttsStat;
            ttsText.SetActive(ttsStat);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
}

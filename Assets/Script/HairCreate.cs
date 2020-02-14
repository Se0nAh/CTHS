using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairCreate : MonoBehaviour
{
    public Sprite[,] HairFace = new Sprite[5, 5];//프리팹안 정보(열은 캐릭터, 행은 표정)
                                                 //행: 1)다람쥐 2)개구리 3)앵무새 4)시바 5)뱁새
                                                 //열: 1)기본표정_전 2)언짢표정_전 3)화난표정_전 4)기본표정_후 5)언짢표정_후
    public GameObject CustomerPrefabs;
    public GameObject now, next;

    public int MoveHairCount = 0;//넘어간 머리 개수


    public void Start()//첫 시작시 머리 세팅
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                string temp = i + "/" + j;
                HairFace[i, j] = Resources.Load<Sprite>(temp);
            }

        }//캐릭터의 정보를 배열에 넣음

        //랜덤숫자 2개를 뽑아서 기본표정의 손님을 뽑음.[n,0]
        int numNow = Random.Range(0, 5);
        int numNext = Random.Range(0, 5);

        now = Instantiate(CustomerPrefabs, new Vector3(0, -5, 0), Quaternion.identity) as GameObject;
        now.GetComponent<SpriteRenderer>().sprite = HairFace[numNow, 0];

        next = Instantiate(CustomerPrefabs, new Vector3(5, -5, 0), Quaternion.identity) as GameObject;
        next.GetComponent<SpriteRenderer>().sprite = HairFace[numNext, 0];
    }

    IEnumerator MoveHair()
    {
        Debug.Log("MoveHair");
        if(GameObject.Find("GameManager").GetComponent<ButtonSetting>().QuizShapeCount < 6)
        {
            MoveHairCount++;
        }
        yield return new WaitForSeconds(0.5f);//딜레이
        while (now.transform.position.x > -5.0f)
        {
            now.transform.position += new Vector3(-0.3f, 0, 0);
            next.transform.position += new Vector3(-0.3f, 0, 0);

            yield return null;
        }

        Change();

        yield break;
    }

    public void Change()
    {
        GameObject temp;

        temp = now;
        now = next;
        next = temp;//Now와 Next바꾸는 과정

        int i = Random.Range(0, 5);//랜덤숫자 설정

        next.GetComponent<SpriteRenderer>().sprite = HairFace[i, 0];//랜덤 얼굴 배정
        next.transform.position = new Vector3(5, -5, 0);//next의 위치 변경
    }

    public void CustomerFaceChange()
    {
        bool FinishHair = GameObject.Find("GameManager").GetComponent<ButtonClick>().FinishHair;

        int WrongCount = GameObject.Find("GameManager").GetComponent<ButtonClick>().WrongCount;
        
        for (int i = 0; i < 5; i++) //얼굴 바꾸기
        {
            if (now.GetComponent<SpriteRenderer>().sprite == HairFace[i, 0] && FinishHair)//now가 기본 표정이고 성공했으면
            {
                now.GetComponent<SpriteRenderer>().sprite = HairFace[i, 3];//기본 표정과 완성된 헤어
                break;
            }
            else if (now.GetComponent<SpriteRenderer>().sprite == HairFace[i, 1] && FinishHair)//now가 언짢은 표정이고 성공했으면
            {
                now.GetComponent<SpriteRenderer>().sprite = HairFace[i, 4];//언짢은 표정과 완성된 헤어
                break;
            }
            else if (WrongCount == 3 && now.GetComponent<SpriteRenderer>().sprite == HairFace[i, 1])//세 번째로 틀렸고 now가 언짢은 표정이면
            {
                now.GetComponent<SpriteRenderer>().sprite = HairFace[i, 2];//화난 표정
                break;
            }
            else if (WrongCount >= 1 && now.GetComponent<SpriteRenderer>().sprite == HairFace[i, 0])//틀린 횟수가 1 이상이고 now가 기본 표정이면
            {
                now.GetComponent<SpriteRenderer>().sprite = HairFace[i, 1];//언짢은 표정
                if (WrongCount == 1)//처음 틀린 거면 Invoke로 1초 후 얼굴 되돌리기
                {
                    Invoke("ReturntoFirstFace", 0.4f);
                    break;
                }
            }  
        }
    }

    void ReturntoFirstFace()//처음 틀렸을 때 얼굴 원상복구
    {
        for (int i = 0; i < 5; i++)
        {
            if (HairFace[i, 1] == now.GetComponent<SpriteRenderer>().sprite)
            {
                now.GetComponent<SpriteRenderer>().sprite = HairFace[i, 0];//기본 표정
                break;
            }
        }
    }

}
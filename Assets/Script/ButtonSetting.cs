using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSetting : MonoBehaviour
{ //랜덤 모형구현
    public int QuizShapeCount = 4;//패턴 개수
    public int[] Qarray = new int[4];//모양 설정하기 위한 번호배열
    public Sprite[] Shape = new Sprite[4];

    public GameObject ShapePrefabs;
    public GameObject[] positions = new GameObject[6];//Position

    public void Start()
    {
        for (int i = 0; i < QuizShapeCount; i++) //Shape에 Resources 로드
        {
            string temp = "Pattern/" + i;
            Shape[i] = Resources.Load<Sprite>(temp);
        }
        CreateShape(); //패턴 생성
    }

    public void PatternCountUp()
    {
        QuizShapeCount++;
        GameObject.Find("HairCreate").GetComponent<HairCreate>().MoveHairCount = 0;
        CreateShape();
    }

    public void CreateShape()
    {
        Debug.Log("CreateShape");
        System.Array.Resize(ref Qarray, QuizShapeCount);
        System.Array.Resize(ref positions, QuizShapeCount);

        for (int i = 0; i < QuizShapeCount; i++) //패턴 설정을 위한 랜덤 숫자를 Qarray에 넣음
        {
            Qarray[i] = Random.Range(0, 4);//랜덤 숫자
        }

        if (QuizShapeCount == 4)
        {
            for (int i = 0; i < QuizShapeCount; i++)
            {
                positions[i] = Instantiate(ShapePrefabs, new Vector3(-1.8f + (i * 1.2f), -2.2f, 0), Quaternion.identity);
                positions[i].GetComponent<SpriteRenderer>().sprite = Shape[Qarray[i]];
            }

        }
        else if (QuizShapeCount == 5)
        {
            for (int i = 0; i < QuizShapeCount; i++)
            {
                positions[i] = Instantiate(ShapePrefabs, new Vector3(-2 + (i * 1), -2.2f, 0), Quaternion.identity);
                positions[i].GetComponent<SpriteRenderer>().sprite = Shape[Qarray[i]];
            }
        }
        else if (QuizShapeCount == 6)
        {
            for (int i = 0; i < QuizShapeCount; i++)
            {
                positions[i] = Instantiate(ShapePrefabs, new Vector3(-2f + (i * 0.8f), -2.2f, 0), Quaternion.identity);
                positions[i].GetComponent<SpriteRenderer>().sprite = Shape[Qarray[i]];
            }
        }
    }

    public void Clear() //Qarray를 Clear하고 position에 새로운 패턴 설정
    {
        for (int i = 0; i < QuizShapeCount; i++)
        {
            Qarray[i] = Random.Range(0, 4);//랜덤숫자 설정
            positions[i].GetComponent<SpriteRenderer>().sprite = Shape[Qarray[i]];//랜덤도형 배정
        }
        Debug.Log("QarrayClear and New Pattern");
    }

}

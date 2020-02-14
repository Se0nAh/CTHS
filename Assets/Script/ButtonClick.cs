using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    /*public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject Tongue;*/

    public int count = 0;//Qarray의 몇 번째를 찾는지, 정답 몇 번 했는지 세는 변수
    public int FailureHairCount = 0;//틀린 머리 개수
    public GameObject[] Shapes;
    public int WrongCount = 0;//틀린 횟수
    public bool FinishHair = false;//헤어를 완성했는지 안했는지
    public float customerMoney = 1000.0f;//손님이 주는 코인
    public float deductMoney = 0.0f;//차감될 코인  
    public GameObject Cow_Sweat;//땀
    public GameObject Cow_Twikle;//반짝
    public GameObject Coin;
    public Text Coin_text;
    public GameObject[] buttonObjects;
    GameObject[] ShapeImage;

    Color color;

    public void Start()
    {
        Coin_text.text = "";
        Coin.SetActive(false);
        Coin_text.gameObject.SetActive(false);
        Cow_Sweat.SetActive(false);
        Cow_Twikle.SetActive(false);
    } 
  
    public void Onclick(int ShapeNumber)//Unity에서 받은 ShapeNumber와 비교
    {
        Quiz(ShapeNumber);//ShapeNumber는 입력받은 도형의 숫자
        if (ShapeNumber == 0)
        {
            GameObject.Find("Cow_left_arm").GetComponent<Animator>().SetTrigger("FinishTrigger");
            SoundManager.I.PlaySFX("scissor", false, 1);
        }
        else if (ShapeNumber == 1)
        {
            GameObject.Find("Cow_right_arm_out").GetComponent<Animator>().SetTrigger("FinishTrigger");
            SoundManager.I.PlaySFX("comb", false, 1);
        }
        else if (ShapeNumber == 2)
        {
            GameObject.Find("Cow_Tongue").GetComponent<Animator>().SetTrigger("FinishTrigger");
        }
        else if (ShapeNumber == 3)
        {
            GameObject.Find("Cow_left_arm").GetComponent<Animator>().SetTrigger("FinishTrigger");
            GameObject.Find("Cow_right_arm_out").GetComponent<Animator>().SetTrigger("FinishTrigger");
            GameObject.Find("Cow_Tongue").GetComponent<Animator>().SetTrigger("FinishTrigger");
            SoundManager.I.PlaySFX("scissor", false, 1);
        }
        
        //StartCoroutine(Animate(ShapeNumber));
    }

    void Quiz(int Number)
    {
   
        ShapeImage = GameObject.Find("GameManager").GetComponent<ButtonSetting>().positions;

        int[] ArrayNum = GameObject.Find("GameManager").GetComponent<ButtonSetting>().Qarray;//Qarray를 불러옴//Qarray는 화면 상에 나온 도형의 숫자를 담고 있음

        if (Number == ArrayNum[count])//도형을 맞추면 
        {
            
            //GetComponent<Animator>().SetTrigger("FinishTrigger");//소 손움직임
            //if(GameObject.Find("HairCreate").GetComponent<HairCreate>().HairFace[2, 0]|| GameObject.Find("HairCreate").GetComponent<HairCreate>().HairFace[4, 0])
            Debug.Log("OK");
            SoundManager.I.PlaySFX("click1", false, 1f);
            SpriteRenderer spr = ShapeImage[count].GetComponent<SpriteRenderer>();
            color = spr.color;
            color.r = 0.7f;
            color.g = 0.7f;
            color.b = 0.7f;
            spr.color = color;   //맞춘 패턴은 알파값 0.7로 변경

            count++;
            Check();
        }
        else //도형을 맞추지 못했으면
        {
            SoundManager.I.PlaySFX("right", false, 1f);
            for (int i = 0; i < count; i++)
            {
                SpriteRenderer spr = ShapeImage[i].GetComponent<SpriteRenderer>();
                color = spr.color;
                color.r = 1;
                color.g = 1;
                color.b = 1;
                spr.color = color;
            }           //처음부터 다시 알파값 원상태


            StartCoroutine("changeButtonStatus"); //버튼 0.3초간 비활성화


            WrongCount++;
            GameObject.Find("HairCreate").GetComponent<HairCreate>().CustomerFaceChange();//틀릴 때마다 Face 변화

            Cow_Sweat.SetActive(true);//틀렸을 때 0.5초간 땀 흘림
            Invoke("Cow_Sweating",0.5f);//땀애니메이션 나오기
            

            //손님 코인
            if (WrongCount == 1)//한 번 틀렸을 때 코인 차감
            {
                deductMoney = customerMoney * (2.0f / 10.0f);//30% 깎임
            }
            else if (WrongCount == 2)//두 번 틀렸을 때 코인차감
            {
                deductMoney = customerMoney * (5.0f / 10.0f);//60% 깍임
            }
            else if (WrongCount == 3)//세 번 틀렸을 때 코인차감
            {
                deductMoney = customerMoney;//0코인
            }

            count = 0;//Qarry 처음부터 다시 확인

            Check();
        }
    }


    public void Check()
    {
        int QuizCount = GameObject.Find("GameManager").GetComponent<ButtonSetting>().QuizShapeCount;//패턴 개수

        if (WrongCount >= 3) //틀린 횟수가 3이상이면
        {
            Debug.Log("Wrong");

            FailureHairCount++;
            GameObject.Find("GameManager").GetComponent<GameManager>().Life();//하트 수 개수 달라지는 함수 불러오기

            if (FailureHairCount < 3) //GameOver가 될 때 Play화면에 움직임 없이 GameOver가 되도록
            {
                //0.5초간 딜레이
                GameObject.Find("HairCreate").GetComponent<HairCreate>().StartCoroutine("MoveHair");//머리 이동

                if (GameObject.Find("HairCreate").GetComponent<HairCreate>().MoveHairCount == 10)//Pattern 개수 증가
                {
                    PatternDelete(); //Pattern이 늘어날 때 적은 수의 Pattern을 삭제
                    GameObject.Find("GameManager").GetComponent<ButtonSetting>().PatternCountUp();
                    customerMoney += 1000.0f;//손님이 주는 코인 증가
                }
                else //개수 증가할 필요 없으면
                {
                    GameObject.Find("GameManager").GetComponent<ButtonSetting>().Clear();//Qarray를 Clear하고 position에 새로운 패턴 설정
                    ShapeImageReset(); //Pattern 다시 진하게
                }

                GameObject.Find("GameManager").GetComponent<ButtonSetting>().Clear();//Qarray를 Clear하고 position에 새로운 패턴 설정
                ShapeImageReset(); //Pattern 다시 진하게
            }
            
            WrongCount = 0;//리셋
            deductMoney = 0;
        }
        else if (count == QuizCount && WrongCount < 3) //정답이고 틀린 횟수가 3미만이면
        {
            Debug.Log("Correct");
            StartCoroutine("changeButtonStatus");
            Successing();//성공시 땀과 동전 애니메이션

            SoundManager.I.PlaySFX("coin2",false,1f);
            FinishHair = true;
            GameObject.Find("HairCreate").GetComponent<HairCreate>().CustomerFaceChange();//완성된 헤어로 변환
            GameObject.Find("ScoreManager").GetComponent<ScoreManager>().SuccessCount();//성공횟수 올라가기
            //0.5초간 딜레이
            GameObject.Find("HairCreate").GetComponent<HairCreate>().StartCoroutine("MoveHair");//머리이동

            GameObject.Find("ScoreManager").GetComponent<ScoreManager>().AddScore(customerMoney - deductMoney);

            if (GameObject.Find("HairCreate").GetComponent<HairCreate>().MoveHairCount == 10)//Pattern 개수 증가
            {
                PatternDelete();
                GameObject.Find("GameManager").GetComponent<ButtonSetting>().PatternCountUp();
                customerMoney += 1000.0f;//손님이 주는 코인 증가
            }
            else
            {   
                GameObject.Find("GameManager").GetComponent<ButtonSetting>().Clear();//Qarray를 Clear하고 position에 새로운 패턴 설정
                ShapeImageReset(); //Pattern 다시 진하게
            }

            deductMoney = 0;

            count = 0;
            WrongCount = 0;
            FinishHair = false;
        }
    }

    void Cow_Sweating()//틀렸을 때 1초간 땀 흘림
    {
        Cow_Sweat.SetActive(false);
    }

    void Cow_Success()//완성 시 1초간 반짝
    {
        Cow_Twikle.SetActive(false);
        Coin.SetActive(false);
        Coin_text.gameObject.SetActive(false);
    }

    void Successing() {
        Coin.SetActive(true);
        Coin_text.gameObject.SetActive(true);
        Coin_text.text = "+" + (customerMoney - deductMoney);
        Cow_Twikle.SetActive(true);//반짝애니메이션
        Invoke("Cow_Success", 0.5f);//반짝/동전애니메이션 취소
    }

    void ShapeImageReset()
    {
        for (int i = 0; i < count; i++)
        {
            SpriteRenderer spr = ShapeImage[i].GetComponent<SpriteRenderer>();
            color = spr.color;
            color.r = 1;
            color.g = 1;
            color.b = 1;
            spr.color = color;
        }
    }

    public void PatternDelete()
    {
         foreach (GameObject Shape in ShapeImage)
        {
            Destroy(Shape);
            Debug.Log("Shape Destroy");
        }
    }

    IEnumerator changeButtonStatus()
    {
        for (int i = 0; i < 4; i++)
        {
            buttonObjects[i].GetComponent<Button>().interactable = false;
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < 4; i++)
        {
            buttonObjects[i].GetComponent<Button>().interactable = true;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float baseSpeed;
    public float speed;
    
    public Scanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public Vector3 lastFacing;

    [Header("Candle related")]
    public GameObject candle;
    public Image candleUI;
    public bool candleOn;
    public bool cannnotTurnOnCandle;
    public Sprite[] candleImages;
    [Header("Camera Related")]
    public GameObject PlayerCameraOffset;

    [Header("Other items")]
    public GameObject itemMagnet;
    public GameObject shield;
    bool isImmortal = false;

    public GameObject LookAtMe;
    [Range(0.0f, 5.0f)]
    public float tempA;
    [Range(0.0f, 5.0f)]
    public float tempB;
    // public Vector3 movingDir = Vector3.zero;
    // Vector3 prevInputVec = Vector3.zero;
    // public bool FallingIntoIt;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();

    }
    public SpriteRenderer getSpriter(){
        return spriter;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.instance.isLive)
            return;
        ////////////////////////////////
        //1. 가는 방향 체크
        //2. 가는 방향 vector 구해서 rotationVec 방향 구하고
        //3. MovePosition에 그 둘을 합한 방향으로 가기
        /////////////////////////
        // Debug.Log(Vector3.Angle(inputVec, (testingItem.transform.position - transform.position).normalized));

        // if(FallingIntoIt){
        //     if(inputVec != Vector2.zero) movingDir = Vector3.RotateTowards(movingDir,(LookAtMe.transform.position - transform.position),tempA,tempB);
        //     else movingDir = inputVec;
        // }
        // if(FallingIntoIt){
            
        // }

        // rigid.MovePosition(transform.position + movingDir*Time.fixedDeltaTime*speed);
        /////////////////////////////
        Vector3 newVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + newVec);
    }

    void LateUpdate(){
        if(!GameManager.instance.isLive)
            return;
            
        anim.SetFloat("Speed", inputVec.magnitude);
        // if(movingDir.x != 0){
            if(inputVec.x != 0){
            spriter.flipX = inputVec.x<0;
            // spriter.flipX = movingDir.x<0;
        }
        CandleCheck();
        
    }
    void CandleCheck(){
        if(!cannnotTurnOnCandle){//candle can be turned on
            if(Input.GetKeyDown(KeyCode.Space)){
                candleOn = !candle.activeSelf;
                candle.SetActive(candleOn);
            }
        }else{//cannot turn on candle!
            candleOn = false;
            candle.SetActive(false);
        }
        // candleUI.sprite = candleImages[0];
        if(candleOn){
            candleUI.sprite = candleImages[0];
        }else{
            if(cannnotTurnOnCandle)
                candleUI.sprite = candleImages[2];
            else
                candleUI.sprite = candleImages[1];
        }
    }

    void OnMove(InputValue value){
        inputVec = value.Get<Vector2>();
        if (inputVec != Vector2.zero){
            lastFacing = inputVec;
        }

        //checking and setting movingDir and prevInputVec
        // if((Vector3)inputVec != prevInputVec){
        //     movingDir = inputVec;
        //     prevInputVec = inputVec;
        // }
    }

    void OnCollisionStay2D(Collision2D collision){
        if(!GameManager.instance.isLive || !collision.transform.CompareTag("normalMonster") || isImmortal)
            return;
        if(collision.transform.GetComponent<normalMonster>().isFoodBox)//foodbox does not attack
            return;

        //일정시간 무적 코루틴 만들기, ㅛㅟㄹ드 스택 소모시 짧게 무적
        if(GameManager.instance.curShieldStack > 0){
            GameManager.instance.curShieldStack--;
            StartCoroutine(Immortal(0.1f));
        }else{
            GameManager.instance.HealthDamage(Time.deltaTime*10);
            GameManager.instance.MentalDamage(Time.deltaTime*20);

            PlayerDeadCheck();
        }
        
    }

    IEnumerator Immortal(float second){
        isImmortal = true;
        yield return new WaitForSeconds(second);
        isImmortal = false;
    }

    public void PlayerDeadCheck(){
        if (GameManager.instance.health <= 0){
            for(int i = 2; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    [Header("Other items")]
    public GameObject itemMagnet;
    public GameObject shield;
    bool isImmortal = false;


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
        Vector3 newVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + newVec);
    }

    void LateUpdate(){
        if(!GameManager.instance.isLive)
            return;
            
        anim.SetFloat("Speed", inputVec.magnitude);
        if(inputVec.x != 0){
            spriter.flipX = inputVec.x<0;
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

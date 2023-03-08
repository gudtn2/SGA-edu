using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ** 움직이는 속도
    private float Speed;

    // ** 움직임을 저장하는 벡터
    private Vector3 Movement;

    // ** 플레이어의 Animator 구성요소를 받아오기 위해
    public Animator animator;

    // ** 플레이어의 SpriteRenderer 구성요소를 받아오기 위해
    private SpriteRenderer playerRenderer;


    // ** [상태체크]
    private bool onAttack; // 공격상태
    private bool onHit; // 피격상태
    private bool onRoll;
    private bool onDie;
    private bool onJump;

    // ** 복사할 총알 원본
    public GameObject BulletPrefab;

    // ** 복제된 총알의 저장공간
    private List<GameObject> Bullets = new List<GameObject>();

    // ** 플레이어가 마자막으로 바라본 방향
    private float Direction;

    private void Awake()
    {
        // ** player 의 Animator를 받아온다.
        animator = GetComponent<Animator>();

        // ** player 의 SpriteRenderer를 받아온다.
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // ** 유니티 기본 제공 함수
    // ** 초기값을 설정할때 사용
    void Start()
    {
        // ** 속도를 초기화
        Speed = 5.0f;       

        // ** 초기값 셋팅
        onAttack = false;
        onHit = false;
        onRoll = false;
        onDie = false;
        onJump = false;

        Direction = 1.0f;
    }

    // ** 유니티 기본 제공 함수
    // ** 프레임마다 반복적으로 실행되는 함수
    void Update()
    {
        // ** 실수 연산 IEEE754

        // ** Input.GetAxis = -1 ~ 1 사이의 값을 반환함.
        float Hor = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1 셋중에 하나를 반환
        float Ver = Input.GetAxis("Vertical"); // -1 ~ 1까지 실수로 반환 

        // ** Hor이 0이라며 멈춰있는 상태이므로 예외처리를 해준다.
        if (Hor != 0)
            Direction = Hor;

        // ** 플레이어가 바라보고있는 방향에 따라 플립설정
        if (Direction < 0)
            playerRenderer.flipX = true;
        
        else if (Direction > 0)
            playerRenderer.flipX = false;

        // **  입력받은 값으로 플레이어를 움직인다
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed, 
            Ver * Time.deltaTime * Speed,
            0.0f);

       
        // ** 좌측 컨트롤키를 입력한다면
        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        // ** 좌측 쉬프트키를 입력한다면
        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        if (Input.GetKey(KeyCode.Z))
            OnRoll();

        if (Input.GetKey(KeyCode.X))
            OnDie();

        if (Input.GetKey(KeyCode.C))
            OnJump();

        // ** 스페이스바를 입력한다면
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Obj = Instantiate(BulletPrefab);
            //Obj.transform.name = "";
            Obj.transform.position = transform.position;
            BulletController Controller = Obj.AddComponent<BulletController>();
            SpriteRenderer bulletRenderer = Obj.GetComponent<SpriteRenderer>();
            Controller.Direction = (Hor < 0) ? new Vector3(-1.0f, 0.0f, 0.0f) : new Vector3(1.0f, 0.0f, 0.0f);
            

            // ** 총알의 SpriteRenderer를 받아온다.
            SpriteRenderer renderer = Obj.GetComponent<SpriteRenderer>();

            // ** 총알의 이미지 반전 상태를 플레이어의 이미지 반전 설정한다.
            bulletRenderer.flipY = playerRenderer.flipX;

            // ** 모든 설정이 종료되었다면 저장소에 보관한다.
            Bullets.Add(Obj);
        }

        // ** 플레이어의 움직임에 따라 이동 모션을 실행 한다.
        animator.SetFloat("Speed", Hor);

        // ** 실제 플레이어를 움직인다.
        transform.position += Movement;
    }

    private void OnAttack()
    {
        // ** 이미 공격모션이 진행중이라면
        if (onAttack)
            // ** 함수를 종료시킨다.
            return;

        //** 함수가 종료되지 않았다면
        // ** 공격상태를 활성화 하고
        onAttack = true;

        // ** 공격모션을 실행시킨다.
        animator.SetTrigger("Attack");

    }

    private void SetAttack()
    {
        // ** 함수가 실행되면 공격모션이 비활성화 된다.
        // ** 함수는 애니메이션 클립의 이벤트 프레임으로 삽입됨
        onAttack = false;
    }

    private void OnHit()
    {
        if (onHit)
            return;

        onHit = true;
        animator.SetTrigger("Hit");

    }

    private void SetHit()
    {
        onHit = false;
    }

    private void OnRoll()
    {
        if (onRoll)
            return;

        onRoll = true;
        animator.SetTrigger("Roll");

    }

    private void SetRoll()
    {
        onRoll = false;
    }

    private void OnDie()
    {
        if (onDie)
            return;

        onDie = true;
        animator.SetTrigger("Die");

    }

    private void SetDie()
    {
        onDie = false;
    }

    private void OnJump()
    {
        if (onJump)
            return;

        onJump = true;
        animator.SetTrigger("Jump");
    }

    private void SetJump()
    {
        onJump = false;
    }


}

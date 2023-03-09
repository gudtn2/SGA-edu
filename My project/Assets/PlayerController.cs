using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ** �����̴� �ӵ�
    private float Speed;

    // ** �������� �����ϴ� ����
    private Vector3 Movement;

    // ** �÷��̾��� Animator ������Ҹ� �޾ƿ��� ����
    public Animator animator;

    // ** �÷��̾��� SpriteRenderer ������Ҹ� �޾ƿ��� ����
    private SpriteRenderer playerRenderer;


    // ** [����üũ]
    private bool onAttack; // ���ݻ���
    private bool onHit; // �ǰݻ���
    private bool onRoll;
    private bool onDie;
    private bool onJump;

    // ** ������ �Ѿ� ����
    public GameObject BulletPrefab;

    // ** ������ fx ����
    public GameObject fxPrefab;


    public GameObject[] stageBack = new GameObject[7];


    // ** ������ �Ѿ��� �������
    private List<GameObject> Bullets = new List<GameObject>();

    // ** �÷��̾ ���ڸ����� �ٶ� ����
    private float Direction;

    private void Awake()
    {
        // ** player �� Animator�� �޾ƿ´�.
        animator = GetComponent<Animator>();

        // ** player �� SpriteRenderer�� �޾ƿ´�.
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �ʱⰪ�� �����Ҷ� ���
    void Start()
    {
        // ** �ӵ��� �ʱ�ȭ
        Speed = 5.0f;       

        // ** �ʱⰪ ����
        onAttack = false;
        onHit = false;
        onRoll = false;
        onDie = false;
        onJump = false;

        Direction = 1.0f;

        for (int i = 0; i < 7; ++i)
            stageBack[i] = GameObject.Find(i.ToString());
    }

    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �����Ӹ��� �ݺ������� ����Ǵ� �Լ�
    void Update()
    {
        // ** �Ǽ� ���� IEEE754

        // ** Input.GetAxis = -1 ~ 1 ������ ���� ��ȯ��.
        float Hor = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1 ���߿� �ϳ��� ��ȯ
        float Ver = Input.GetAxis("Vertical"); // -1 ~ 1���� �Ǽ��� ��ȯ 

        // ** Hor�� 0�̶�� �����ִ� �����̹Ƿ� ����ó���� ���ش�.
        if (Hor != 0)
            Direction = Hor;

        // ** �÷��̾ �ٶ󺸰��ִ� ���⿡ ���� �ø�����
        if (Direction < 0)
            playerRenderer.flipX = true;
        
        else if (Direction > 0)
            playerRenderer.flipX = false;

        // **  �Է¹��� ������ �÷��̾ �����δ�
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed, 
            Ver * Time.deltaTime * Speed,
            0.0f);

       
        // ** ���� ��Ʈ��Ű�� �Է��Ѵٸ�
        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        // ** ���� ����ƮŰ�� �Է��Ѵٸ�
        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        if (Input.GetKey(KeyCode.Z))
            OnRoll();

        if (Input.GetKey(KeyCode.X))
            OnDie();

        if (Input.GetKey(KeyCode.C))
            OnJump();

        // ** �����̽��ٸ� �Է��Ѵٸ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ** ����
            OnAttack();

            // ** �Ѿ˿����� �����Ѵ�.
            GameObject Obj = Instantiate(BulletPrefab);

            // ** ������ �Ѿ��� ��ġ�� ���� �÷��̾��� ��ġ�� �ʱ�ȭ�Ѵ�.
            Obj.transform.position = transform.position;

            // ** �Ѿ��� BullerController ��ũ��Ʈ�� �޾ƿ´�.
            BulletController Controller = Obj.AddComponent<BulletController>();

            // ** �Ѿ� ��ũ��Ʈ������ ���� ������ ���� �÷��̾��� ���� ������ ���� �Ѵ�.
            Controller.Direction = new Vector3(Direction, 0.0f, 0.0f);

            // ** �Ѿ� ��ũ��Ʈ ������ FX Prefab�� �����Ѵ�.
            Controller.fxPrefab = fxPrefab;

            // ** �Ѿ��� SpriteRenderer�� �޾ƿ´�.
            SpriteRenderer buleltRenderer = Obj.GetComponent<SpriteRenderer>();

            // ** �Ѿ��� �̹��� ���� ���¸� �÷��̾��� �̹��� ���� ���·� �����Ѵ�.
            buleltRenderer.flipY = playerRenderer.flipX;

            // ** ��� ������ ����Ǿ��ٸ� ����ҿ� �����Ѵ�.
            Bullets.Add(Obj);
        }

        // ** �÷��̾��� �����ӿ� ���� �̵� ����� ���� �Ѵ�.
        animator.SetFloat("Speed", Hor);

        // ** ���� �÷��̾ �����δ�.

        // ** offset box
        //transform.position += Movement;
    }

    private void OnAttack()
    {
        // ** �̹� ���ݸ���� �������̶��
        if (onAttack)
            // ** �Լ��� �����Ų��.
            return;

        //** �Լ��� ������� �ʾҴٸ�
        // ** ���ݻ��¸� Ȱ��ȭ �ϰ�
        onAttack = true;

        // ** ���ݸ���� �����Ų��.
        animator.SetTrigger("Attack");

    }

    private void SetAttack()
    {
        // ** �Լ��� ����Ǹ� ���ݸ���� ��Ȱ��ȭ �ȴ�.
        // ** �Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Ե�
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

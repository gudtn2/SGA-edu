using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    // ** �����̴� �ӵ�
    private float Speed;

    // ** �������� �����ϴ� ����
    private Vector3 Movement;

    // ** �÷��̾��� Animator ������Ҹ� �޾ƿ�������...
    private Animator animator;

    // ** �÷��̾��� SpriteRenderer ������Ҹ� �޾ƿ�������...
    private SpriteRenderer playerRenderer;

    // ** [����üũ]
    private bool onAttack; // ���ݻ���
    private bool onHit; // �ǰݻ���

    // ** ������ �Ѿ� ����
    private GameObject BulletPrefab;

    // ** ������ FX ����
    private GameObject fxPrefab;
    private GameObject fxPrefab2;
    public GameObject boomEffect;

    // ���� list�� ����
    

    /*
    Dictionary<string, object>;
    Dictionary<string, GameObject>;
     */

    // ** ������ �Ѿ��� �������.
    private List<GameObject> Bullets = new List<GameObject>();

    // ** �÷��̾ ���������� �ٶ� ����.
    private float Direction;

    [Header("����")]
    // ** �÷��̾ �ٶ󺸴� ����

    [Tooltip("����")]
    public bool DirLeft;
    [Tooltip("������")]
    public bool DirRight;


    private float CoolDown;

    public int life;
    public int score;
    public int maxPower;
    public int power;

    private void Awake()
    {
        // ** player �� Animator�� �޾ƿ´�.
        animator = this.GetComponent<Animator>();

        // ** player �� SpriteRenderer�� �޾ƿ´�.
        playerRenderer = this.GetComponent<SpriteRenderer>();

        // ** [Resources] �������� ����� ���ҽ��� ���´�.
        BulletPrefab = Resources.Load("Prefabs/Bullet3") as GameObject;
        //fxPrefab = Resources.Load("Prefabs/FX/Smoke") as GameObject;
        fxPrefab = Resources.Load("Prefabs/FX/Hit") as GameObject;
        fxPrefab2 = Resources.Load("Prefabs/FX/Sword") as GameObject;
    }

    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �ʱⰪ�� ������ �� ���
    void Start()
    {
        // ** �ӵ��� �ʱ�ȭ.
        Speed = 8.0f;
        
        // ** �ʱⰪ ����
        onAttack = true;        
        onHit = false;
        Direction = 1.0f;

        DirLeft = false;
        DirRight = false;

        CoolDown = 1.0f;

        
    }

    // ** ����Ƽ �⺻ ���� �Լ�
    // ** �����Ӹ��� �ݺ������� ����Ǵ� �Լ�.
    void Update()
    {
        // **  Input.GetAxis =     -1 ~ 1 ������ ���� ��ȯ��. 
        float Hor = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1 ���߿� �ϳ��� ��ȯ.
        float Ver = Input.GetAxisRaw("Vertical"); // -1 or 0 or 1 ���߿� �ϳ��� ��ȯ.

        // ** �Է¹��� ������ �÷��̾ �����δ�.
        Movement = new Vector3(
            Hor * Time.deltaTime * Speed,
            Ver * Time.deltaTime * Speed,
            0.0f);

        transform.position += new Vector3(0.0f, Movement.y, 0.0f);

        // ** Hor�� 0�̶�� �����ִ� �����̹Ƿ� ����ó���� ���ش�. 
        if (Hor != 0)
            Direction = Hor;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // ** �÷��̾��� ��ǥ�� 0.1f ���� ������ �÷��̾ �����δ�.
          
                transform.position += Movement;

                ControllerManager.GetInstance().DirRight = true;
                ControllerManager.GetInstance().DirLeft = false;
            
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = true;

            // ** �÷��̾��� ��ǥ�� -15.0 ���� Ŭ�� �÷��̾ �����δ�.
            if (transform.position.x > -15.0f)
                // ** ���� �÷��̾ �����δ�.
                transform.position += Movement;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (transform.position.y < 6.5f)
                transform.position += Movement;

        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (transform.position.y > -10.0f)
                transform.position += Movement;
        }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || 
            Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            ControllerManager.GetInstance().DirRight = false;
            ControllerManager.GetInstance().DirLeft = false;
        }
        

        // ** �÷��̾ �ٶ󺸰��ִ� ���⿡ ���� �̹��� ���� ����.
        //if (Direction < 0)
        //{
        //    playerRenderer.flipX = DirLeft = true;
        //}
        //else if (Direction > 0)
        //{
        //    playerRenderer.flipX = false;
        //    DirRight = true;
        //}

        if (onAttack)
        {
            onAttack = false;
            StartCoroutine(OnAttack());
        }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }

        // ** ���� ����ƮŰ�� �Է��Ѵٸ�.....
        if (Input.GetKey(KeyCode.LeftShift))
            // ** �ǰ�
            OnHit();

        // ** �÷����� �����ӿ� ���� �̵� ����� ���� �Ѵ�.
        animator.SetFloat("Speed", Hor);
    }

    IEnumerator OnAttack()
    {
        // ** ���ݸ���� ���� ��Ų��.
        //animator.SetTrigger("Attack");

        // ** �Ѿ˿����� �����Ѵ�.
        GameObject Obj = Instantiate(BulletPrefab);

        // ** ������ �Ѿ��� ��ġ�� ���� �÷��̾��� ��ġ�� �ʱ�ȭ�Ѵ�.
        Obj.transform.position = transform.position + Vector3.right + Vector3.up;

        // ** �Ѿ��� BullerController ��ũ��Ʈ�� �޾ƿ´�.
        BulletController Controller = Obj.AddComponent<BulletController>();

        // ** �Ѿ� ��ũ��Ʈ������ ���� ������ ���� �÷��̾��� ���� ������ ���� �Ѵ�.
        Controller.Direction = new Vector3(1.0f, 0.0f, 0.0f);

        // ** �Ѿ� ��ũ��Ʈ������ FX Prefab�� �����Ѵ�.
        Controller.fxPrefab = fxPrefab;

        // ** �Ѿ��� SpriteRenderer�� �޾ƿ´�.
        SpriteRenderer buleltRenderer = Obj.GetComponent<SpriteRenderer>();

        // ** �Ѿ��� �̹��� ���� ���¸� �÷��̾��� �̹��� ���� ���·� �����Ѵ�.
        //buleltRenderer.flipY = playerRenderer.flipX;

        // ** ��� ������ ����Ǿ��ٸ� ����ҿ� �����Ѵ�.
        Bullets.Add(Obj);

        while (CoolDown > 0.0f)
        {
            CoolDown -= Time.deltaTime;
            yield return null;
        }

        CoolDown = 0.5f;
        onAttack = true;
    }


    private void SetAttack()
    {
        // ** �Լ��� ����Ǹ� ���ݸ���� ��Ȱ��ȭ �ȴ�.
        // ** �Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Ե�.
        onAttack = false;
    }

    private void OnHit()
    {
        // ** �̹� �ǰݸ���� �������̶��
        if (onHit)
            // ** �Լ��� �����Ų��.
            return;

        // ** �Լ��� ������� �ʾҴٸ�...
        // ** �ǰݻ��¸� Ȱ��ȭ �ϰ�.
        onHit = true;

        // ** �ǰݸ���� ���� ��Ų��.
        animator.SetTrigger("Hit");
    }

    private void SetHit()
    {
        // ** �Լ��� ����Ǹ� �ǰݸ���� ��Ȱ��ȭ �ȴ�.
        // ** �Լ��� �ִϸ��̼� Ŭ���� �̺�Ʈ ���������� ���Ե�.
        onHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet")
        {
            OnHit();
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                        power++;
                    break;
                case "Boom":
                    // ����Ʈ Ȱ��
                    boomEffect.SetActive(true);
                    // ������
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    for(int index = 0; index < enemies.Length; index++)
                    {
                        
                    }
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}

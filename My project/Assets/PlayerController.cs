using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ** 움직이는 속도
    private float Speed;
    private Vector3 Movement;

    public Animator animator;

    private bool onAttack;
    private bool onHit;
    private bool onRoll;
    private bool onDie;
    private bool onJump;
    // ** 유니티 기본 제공 함수
    // ** 초기값을 설정할때 사용
    void Start()
    {
        // ** 속도를 초기화
        Speed = 5.0f;

        // ** player 의 Animator를 받아온다.
        animator = GetComponent<Animator>();
        onAttack = false;
        onHit = false;
        onRoll = false;
        onDie = false;
        onJump = false;
    }

    // ** 유니티 기본 제공 함수
    // ** 프레임마다 반복적으로 실행되는 함수
    void Update()
    {
        // ** 실수 연산 IEEE754

        // ** Input.GetAxis = -1 ~ 1 사이의 값을 반환함.
        float Hor = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1 셋중에 하나를 반환
        float Ver = Input.GetAxis("Vertical"); // -1 ~ 1까지 실수로 반환 

        Movement = new Vector3(
            Hor * Time.deltaTime * Speed, 
            Ver * Time.deltaTime * Speed,
            0.0f);

        if (Input.GetKey(KeyCode.LeftControl))
            OnAttack();

        if (Input.GetKey(KeyCode.LeftShift))
            OnHit();

        if (Input.GetKey(KeyCode.Z))
            OnRoll();

        if (Input.GetKey(KeyCode.X))
            OnDie();

        if (Input.GetKey(KeyCode.C))
            OnJump();

        animator.SetFloat("Speed", Hor);
        transform.position += Movement;
    }

    private void OnAttack()
    {
        if (onAttack)
            return;

        onAttack = true;
        animator.SetTrigger("Attack");

    }

    private void SetAttack()
    {
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

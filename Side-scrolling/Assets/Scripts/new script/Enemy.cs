using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int health;
    public Sprite[] sprites;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;

    public GameObject player;

    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Fire();
        Reload();
    }

    void Fire()
    {   
        if (curShotDelay < maxShotDelay)
            return;

        if(enemyName == "S")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);

        }
        else if (enemyName == "L")
        {
            GameObject bulletU = Instantiate(bulletObjA, transform.position + Vector3.up*0.5f, transform.rotation);
            GameObject bulletD = Instantiate(bulletObjA, transform.position + Vector3.down * 0.5f, transform.rotation);

            Rigidbody2D rigidU = bulletU.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidD = bulletD.GetComponent<Rigidbody2D>();

            Vector3 dirVecU = player.transform.position - (transform.position + Vector3.up * 0.5f);
            Vector3 dirVecD = player.transform.position - (transform.position + Vector3.down * 0.5f);

            rigidU.AddForce(dirVecU.normalized * 10, ForceMode2D.Impulse);
            rigidD.AddForce(dirVecD.normalized * 10, ForceMode2D.Impulse);
        }
        
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        //spriteRenderer.sprite = sprites[1];
        //Invoke ("RetrunSprite", 0.1f);
        anim.SetTrigger("Hit");
        //Invoke("ReturnAnim", 0.1f);
        if(health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // 랜덤 아이템 드랍
            int ran = Random.Range(0, 10);
            if(ran < 5)
            { Debug.Log("Not Item"); }
            else if (ran < 8) // 코인
            {
                Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
            }
            else if (ran < 9) // 파워
            {
                Instantiate(itemPower, transform.position, itemPower.transform.rotation);

            }
            else if (ran < 10) // 붐
            {
                Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    void RetrunSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void ReturnAnim()
    {
        anim.SetTrigger("Hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            Destroy(collision.gameObject);
        }
           
    }
}

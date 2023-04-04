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
    public ObjectManager objectManager;
    public GameManager gameManager;


    SpriteRenderer spriteRenderer;
    Animator anim;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 3000;
                Invoke("Stop", 3);

                break;
            case "L":
                health = 100;
                break;
            case "M":
                health = 20;
                break;
            case "S":
                health = 5;
                break;
        }
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        Invoke("Think", 1);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
        
        switch(patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        anim.SetTrigger("OnAttack");

        // 앞으로 5발 발사
        GameObject bulletU = objectManager.MakeObj("BulletBossA");
        bulletU.transform.position = transform.position + Vector3.up * Random.Range(5, 10);
        GameObject bulletUU = objectManager.MakeObj("BulletBossA");
        bulletUU.transform.position = transform.position + Vector3.up * Random.Range(0, 5);
        GameObject bulletD = objectManager.MakeObj("BulletBossA");
        bulletD.transform.position = transform.position + Vector3.down * Random.Range(0, 4);
        GameObject bulletDD = objectManager.MakeObj("BulletBossA");
        bulletDD.transform.position = transform.position + Vector3.down * Random.Range(4, 6);
        GameObject bulletDDD = objectManager.MakeObj("BulletBossA");
        bulletDDD.transform.position = transform.position + Vector3.down * Random.Range(6, 10);

        Rigidbody2D rigidU = bulletU.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidUU = bulletUU.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidD = bulletD.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidDD = bulletDD.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidDDD = bulletDDD.GetComponent<Rigidbody2D>();

        rigidU.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
        rigidUU.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
        rigidD.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
        rigidDD.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
        rigidDDD.AddForce(Vector2.left * 20, ForceMode2D.Impulse);

        // 패턴 카운팅
        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireFoward", 0.8f);
        else
            Invoke("Think", 2.0f);
    }

    void FireShot()
    {
        anim.SetTrigger("OnAttack");

        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(0f, 4f), Random.Range(-1.5f, 1.5f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 20, ForceMode2D.Impulse);
        }        

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 0.2f);
        else
            Invoke("Think", 1.5f);
    }
    void FireArc()
    {
        anim.SetTrigger("OnAttack");

        GameObject bullet = objectManager.MakeObj("BulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(-1, Mathf.Sin(curPatternCount));
        rigid.AddForce(dirVec.normalized * 20, ForceMode2D.Impulse);
        
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.05f);
        else
            Invoke("Think", 1.5f);

    }
    void FireAround()
    {
        anim.SetTrigger("OnAttack");

        int roundNumA = 40;
        int roundNumB = 35;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int index = 0; index < roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * -90;
            bullet.transform.Rotate(rotVec);
        }        
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 5.0f);
    }
    void Update()
    {
        if (enemyName == "B")
            return;
        Fire();
        Reload();
    }

    void Fire()
    {   
        if (curShotDelay < maxShotDelay)
            return;

        if(enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;
                
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);

        }
        else if (enemyName == "L")
        {
            GameObject bulletU = objectManager.MakeObj("BulletEnemyB");
            bulletU.transform.position = transform.position + Vector3.up * 0.5f;

            GameObject bulletD = objectManager.MakeObj("BulletEnemyB");
            bulletD.transform.position = transform.position + Vector3.down * 0.5f;

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
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            //spriteRenderer.sprite = sprites[1];
            //Invoke ("RetrunSprite", 0.1f);
            anim.SetTrigger("Hit");
            //Invoke("ReturnAnim", 0.1f);
        }
        if(health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

           
            // 랜덤 아이템 드랍
            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
            if(ran < 5)
            { Debug.Log("Not Item"); }
            else if (ran < 6) // 코인
            {
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
            }
            else if (ran < 9) // 파워
            {
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
            }
            else if (ran < 10) // 붐
            {
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
            }
            if (enemyName == "B")
            {
                anim.SetTrigger("OnDie");
                Invoke("Victory", 2.0f);
            }
            else
            gameObject.SetActive(false);
            //transform.rotation = Quaternion.identity;
            gameManager.CallExplosion(transform.position, enemyName);
        }
    }

    void Victory()
    {
        gameManager.Victory();
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
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
            gameObject.SetActive(false);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            collision.gameObject.SetActive(false);
        }

    }
}

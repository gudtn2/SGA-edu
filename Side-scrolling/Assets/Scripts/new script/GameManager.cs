using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    private void Awake()
    {
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL" };
    }
    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        // UI score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score); //string.Format("{0:n0}" 1000ÀÚ¸® ²÷±â
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);
        GameObject enemy = objectManager.MakeObj(enemyObjs[ranEnemy]);
        enemy.transform.position = spawnPoints[ranPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if (ranPoint == 5 || ranPoint == 6) //À§¿¡¼­ ½ºÆù
        {
            rigid.velocity = new Vector2(-1, enemyLogic.speed * (-1));
        }
        else if(ranPoint == 7 || ranPoint == 8) //¹Ø¿¡¼­ ½ºÆù
        {
            rigid.velocity = new Vector2(-1, enemyLogic.speed);
        }
        else // ¾Õ¿¡¼­ ½ºÆù
        {
            rigid.velocity = new Vector2(enemyLogic.speed * (-1) , 0);

        }
    }

    public void UpadateLifeIcon(int life)
    {
        // UI ¸ñ¼û ºñÈ°¼º
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
        // UI ¸ñ¼û È°¼º
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int Boom)
    {
        // UI ÆøÅº ºñÈ°¼º
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }
        // UI ÆøÅº È°¼º
        for (int index = 0; index < Boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }
    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.left * 13f + Vector3.down * 1.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}

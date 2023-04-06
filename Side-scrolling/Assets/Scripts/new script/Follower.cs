using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;
    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }
    void Watch()
    {
        //Queue = FIFO (First Input First Out) ���� �Էµ� �����Ͱ� ���� ������ ����
        //# Input Pos
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        //# Output Pos
        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (!(Input.GetButton("Fire1") || Input.GetKey(KeyCode.X)))
            return;

        if (curShotDelay < maxShotDelay)
            return;
       
                GameObject bullet = objectManager.MakeObj("BulletFollower");
                bullet.transform.position = transform.position + Vector3.right * 1.0f;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.right * 30, ForceMode2D.Impulse);
             

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager
{
    private ControllerManager() { }
    private static ControllerManager Instance = null;

    public static ControllerManager GetInstance()
    {
        if (Instance == null)
            Instance = new ControllerManager();
        return Instance;
    }

    public bool DirLeft;
    public bool DirRight;

    public float BulletSpeed = 10.0f;
    public float PlayerSpeed = 5.0f;
    public float Player_HP = 100;
}

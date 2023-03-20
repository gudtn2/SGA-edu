using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public List<GameObject> Images = new List<GameObject>();
    public List<GameObject> Buttons = new List<GameObject>();
    public List<Image> ButtonImages = new List<Image>();
    private float cooldown;
    int a;
    private void Start()
    {
        GameObject SkillsObj = GameObject.Find("Skills");

        for (int i = 0; i < SkillsObj.transform.childCount; ++i)
        {
            GameObject Obj = SkillsObj.transform.GetChild(i).gameObject;
            Images.Add(Obj);
        }

        for (int i = 0; i < Images.Count; ++i)
            Buttons.Add(Images[i].transform.GetChild(0).gameObject);

        for (int i = 0; i < Buttons.Count; ++i)
        {
            ButtonImages.Add(Buttons[i].GetComponent<Image>());
        }

        cooldown = 0.0f;
   
    }

    public void PushButton0()
    {
        ButtonImages[a].fillAmount = 0;
        Buttons[a].GetComponent<Button>().enabled = false;
        StartCoroutine(PushButton_Coroutine());
    }

    IEnumerator PushButton_Coroutine()
    {
        float cool = cooldown;
        while(ButtonImages[a].fillAmount != 1)
        {
            ButtonImages[a].fillAmount += Time.deltaTime * cool;
            yield return null;
        }
        Buttons[a].GetComponent<Button>().enabled = true;
    }
    public void Testcase1()
    {
        a = 0;
        cooldown = 0.5f;
        ControllerManager.GetInstance().BulletSpeed += 5.0f;

    }
    public void Testcase2()
    {
        a = 1;

        cooldown = 0.5f;
        GameObject.Find("Player").transform.localScale = new Vector3(GameObject.Find("Player").transform.localScale.x + 1, GameObject.Find("Player").transform.localScale.y + 1, 0);
    }
    public void Testcase3()
    {
        a = 2;

        cooldown = 0.5f;
        ControllerManager.GetInstance().PlayerSpeed += 1.5f;
    }

    public void Testcase4()
    {
        a = 3;

        cooldown = 0.5f;

    }
    public void Testcase5()
    {
        cooldown = 0.5f;

    }
}

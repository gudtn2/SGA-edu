using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public GameObject SkillCanvus;
    public bool SkillCanvasActive;

    void Start()
    {
        //
        SkillCanvasActive = true;
        SkillCanvus.SetActive(SkillCanvasActive);
    }

    public void onSkillCanvasActive()
    {
        SkillCanvasActive = !SkillCanvasActive;
        SkillCanvus.SetActive(SkillCanvasActive);
    }
}

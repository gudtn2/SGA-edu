using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBar : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public Text text;
    public Text messagetext;
    public Image image;
    IEnumerator Start()
    {
        //EditorApplication.isPaused = true;        
        asyncOperation = SceneManager.LoadSceneAsync("Game Start");
        asyncOperation.allowSceneActivation = false;
        while(!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f * 100f;
            text.text = progress.ToString() + "%";
            image.fillAmount = 0.01f * progress;
            yield return null;           

            if (asyncOperation.progress > 0.7f)
            {
                yield return new WaitForSeconds(2.5f);

                messagetext.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                    asyncOperation.allowSceneActivation = true;
            }
        }
    }
}

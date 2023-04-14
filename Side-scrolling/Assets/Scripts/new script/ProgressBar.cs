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
    public GameObject runPlayer;
    IEnumerator Start()
    {
        //EditorApplication.isPaused = true;
        asyncOperation = SceneManager.LoadSceneAsync("Game Start");
        asyncOperation.allowSceneActivation = false;
        while(!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f * 100f;
            text.text = progress.ToString() + "%";
            runPlayer.transform.position = new Vector3(-14.5f + progress *0.01f * 28.5f ,-7, 0);
            image.fillAmount = 0.01f * progress;
            yield return null;           

            if (asyncOperation.progress > 0.7f)
            {


                if (Input.GetMouseButton(0))
                {
                    Debug.Log("zz");
                    asyncOperation.allowSceneActivation = true;
                }
                yield return new WaitForSeconds(1.5f);
                messagetext.gameObject.SetActive(true);

            }
        }
    }
}

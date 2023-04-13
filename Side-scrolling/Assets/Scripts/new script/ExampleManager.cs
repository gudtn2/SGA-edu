using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;




public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbx7nzu7bazW30J6sQEsjtWvpa17RAbN04k76ffmqce0TYy2GwbV8kLeg4jXLBXoBxFf/exec";
    public InputField IDInput, PassInput;
    string id, pass;

    bool SetIDPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();

        if (id == "" || pass == "") return false;
        else return true;
    }

    public void Register()
    {
        if (!SetIDPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }

    public void Login()
    {
        if (!SetIDPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }
    IEnumerator Post(WWWForm form)
    {
        // ** ��û�� �ϱ����� �۾�
        //UnityWebRequest request = UnityWebRequest.Get(URL);

        //MemberForm member = new MemberForm("�����", 45);

        //form.AddField("Name", member.Name);
        //form.AddField("Age", member.Age);

        //using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        //{
        //    yield return request.SendWebRequest();
            
        //    // ** ���信 ���� �۾�

        //    print(request.downloadHandler.text);
        //}


        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {
            yield return request.SendWebRequest();

            if (request.isDone) print(request.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("progressScene");
    }

}

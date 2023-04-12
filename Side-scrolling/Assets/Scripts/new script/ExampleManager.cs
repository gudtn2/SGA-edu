using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


[System.Serializable]
public class MemberForm
{
    public int index;
    public string name;
    public int age;
    public int gender;

    public MemberForm(int index, string name, int age, int gender)
    {
        this.index = index;
        this.name = name;
        this.age = age;
        this.gender = gender;
    }
}
// ȸ������
// �α���


public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbx7nzu7bazW30J6sQEsjtWvpa17RAbN04k76ffmqce0TYy2GwbV8kLeg4jXLBXoBxFf/exec";

    IEnumerator Start()
    {
        // ** ��û�� �ϱ����� �۾�
        //UnityWebRequest request = UnityWebRequest.Get(URL);

        //MemberForm member = new MemberForm("�����", 45);

        //WWWForm form = new WWWForm();

        //form.AddField("Name", member.Name);
        //form.AddField("Age", member.Age);

        //using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        //{
        //    yield return request.SendWebRequest();
            
        //    // ** ���信 ���� �۾�

        //    print(request.downloadHandler.text);
        //}


        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            MemberForm json = JsonUtility.FromJson<MemberForm>(request.downloadHandler.text);
            // ** ���信 ���� �۾�
            print(json.name);
            print(json.age);
            print(json.index);
            print(json.gender);
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("progressScene");
    }

}

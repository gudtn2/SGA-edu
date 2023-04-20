using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}

public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbx7nzu7bazW30J6sQEsjtWvpa17RAbN04k76ffmqce0TYy2GwbV8kLeg4jXLBXoBxFf/exec";
    public GoogleData GD;
    public InputField IDInput, PassInput, ValueInput, NewIDInput, NewPassInput, NameInput, AgeInput;
    public Toggle Man, Woman;
    public GameObject RegisterUI;
    public GameObject Successlogin;
    public GameObject Faillogin;
    public GameObject SuccessRegister;
    string id, pass, newid, newpass, nname, age;

    bool SetLoginPass()
    {
        id = IDInput.text.Trim();
        pass = PassInput.text.Trim();
        newid = NewIDInput.text.Trim();
        newpass = NewPassInput.text.Trim();
        nname = NameInput.text.Trim();
        age = AgeInput.text.Trim();
        if (id == "" || pass == "") return false;
        else return true;
    }
    bool SetRegisterPass()
    {
        newid = NewIDInput.text.Trim();
        newpass = NewPassInput.text.Trim();
        nname = NameInput.text.Trim();
        age = AgeInput.text.Trim();
        if (newid == "" || newpass == "" || nname == "" || age == "") return false;
        else return true;
    }

    public void Register()
    {
        if (!SetRegisterPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�.");
            return;
        }
        string password = Security(newpass);
        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", newid);
        form.AddField("pass", password);
        form.AddField("name", nname);
        form.AddField("age", age);
        form.AddField("age", age);
        if(Man.isOn)
        {
            form.AddField("gender", 1);
        }
        else if(Woman.isOn)
        {
            form.AddField("gender", 2);
        }
        else
        {
            return;
        }

        StartCoroutine(Post(form));
    }

    public void Login()
    {
        if (!SetLoginPass())
        {
            print("���̵� �Ǵ� ��й�ȣ�� ����ֽ��ϴ�.");
            return;
        }
        string password = Security(pass);

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", password);

        StartCoroutine(Post(form));
    }
    void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }

    public void SetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("value", ValueInput.text);

        StartCoroutine(Post(form));
    }

    public void GetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");

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

            if (request.isDone) Response(request.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }
    }

    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "ERROR")
        {
            print(GD.order + "�� ������ �� �����ϴ�. ���� �޽��� : " + GD.msg);
            Faillogin.SetActive(true);
            Invoke("FailOff", 1.0f);
            return;
        }

        print(GD.order + "�� �����߽��ϴ�. �޽��� : " + GD.msg);

        if (GD.order == "getValue")
        {
            ValueInput.text = GD.value;
        }

        if (GD.order == "login")
        {
            Successlogin.SetActive(true);
            Invoke("NextScene", 1.0f);
        }

        if (GD.order == "register")
        {
            SuccessRegister.SetActive(true);
            RegisterUI.SetActive(false);
            Invoke("RegisterOff", 1.0f);
        }
    }

    void LoginOff()
    {
        Successlogin.SetActive(false);
    }

    void FailOff()
    {
        Faillogin.SetActive(false);
    }

    void RegisterOff()
    {
        SuccessRegister.SetActive(false);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("progressScene");
    }

    string Security(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            // ** true
            //message.text = "password�� �ʼ� �Է� �� �Դϴ�.";
            return "null";

        }
        else
        {
            // ** ��ȣȭ & ��ȣȭ
            // ** false
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }
    }
}

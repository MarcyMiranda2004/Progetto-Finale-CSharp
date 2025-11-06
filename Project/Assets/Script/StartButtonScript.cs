using System;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    //This class is used to change the scene using the button, it should call the instance of the Manager and its method in order to change scene

    
    // Update is called once per frame
    void Update()
    {
        GetComponent<Button>().onClick.AddListener(() => ChangingScene("PresentationScene"));
    }


    void ChangingScene(string sceneName)
    {
        TitleSceneManager.Instance.ChangeScene(sceneName);
    }

}

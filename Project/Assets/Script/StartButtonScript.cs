using System;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    //This class is used to change the scene using the button, it should call the instance of the Manager and its method in order to change scene



    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => ChangingScene("PresentationScene")); //To Use this, change the name of the scene to the one u need to load.
    }


    void ChangingScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{

    // This class should manage the transition to another scene, we should implement this to all the scenes we make.
    private static TitleSceneManager _instance;
    public static TitleSceneManager Instance { get { return _instance; } }
    

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    /// <summary>
    /// This method allow us to change the scene, it needs the name of the scene in string
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// This method allow us to change scene by an index (int)
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void ChangeSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}

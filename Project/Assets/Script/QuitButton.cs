using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}

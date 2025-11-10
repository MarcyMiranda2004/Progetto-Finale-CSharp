using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    [SerializeField] private GameObject WinSprite;
    [SerializeField] private TextMeshProUGUI puntiText;
    [SerializeField] private TextMeshProUGUI eventiText;

    private float points = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            WinSprite.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoint()
    {
        points++;
        CheckPoints();
        Debug.Log("Punti totali: " + points);
    }

    public float GetPoints()
    {
        return points;
    }

    public void menusPoints()
    {
        points--;
        CheckPoints();
        Debug.Log("Punti totali: " + points);
    }

    public void AddPointX2()
    {
        points += 2;
        CheckPoints();
        Debug.Log("Punti totali: " + points);
    }

    public void CheckPoints()
    {
        if (points >= 5)
        {
            Debug.Log("Hai raggiunto il punteggio necessario!");
            Win();
        }
    }

    private async Task Win()
    {
        
        WinSprite.SetActive(true);
        puntiText.gameObject.SetActive(false);
        eventiText.gameObject.SetActive(false);
        Time.timeScale = 0f;
        // yield return new WaitForSecondsRealtime(10f);
        await Task.Delay(5000);
        GameManager.Instance.ChangeScene("AspettiSocialiScene");
    }
}

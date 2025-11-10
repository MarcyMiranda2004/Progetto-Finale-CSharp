using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PyramidManager : MonoBehaviour
{
    public List<GrabAndDrag> allFoods = new List<GrabAndDrag>();

    [SerializeField]
    private string nextSceneName;

    [SerializeField]
    private GameObject victorySprite;

    private void Start()
    {
        if (victorySprite != null)
            victorySprite.SetActive(false);
    }

    public async Task CheckCompletion()
    {
        // Controlla che tutti i cibi siano posizionati correttamente
        foreach (GrabAndDrag food in allFoods)
        {
            if (!food.IsCorrectlyAttached)
            {
                return;
            }
        }

        // Tutti i cibi sono corretti!
        if (victorySprite != null)
        {
            victorySprite.SetActive(true);
        }

        // Aspetta 4 secondi prima di cambiare scena
        await Task.Delay(4000);

        GameManager.Instance.ChangeScene(nextSceneName);
    }
}

using UnityEngine;

// Enumerazione che definisce le diverse categorie alimentari
//    (utile per distinguere tipi di cibo nel gioco)
public enum FoodCategory
{
    Base, // Alimenti di base (es. cereali, frutta, verdura)
    ConsumoModerato, // Cibi da consumare con moderazione (es. carne, formaggi)
    ConsumoLimitato, // Cibi da limitare (es. dolci, snack)
    Occasionale, // Cibi da consumare raramente o in occasioni speciali
}

// Classe che rappresenta il tipo di alimento
//    Da assegnare a ogni GameObject "cibo" nella scena
public class FoodType : MonoBehaviour
{
    // Categoria del cibo (selezionabile dall’Inspector)
    public FoodCategory category;
}

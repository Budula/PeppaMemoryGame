using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController 
{
    public static CardsController _instance = null;
    public List<Card> cards = new List<Card>();
    public int totalSelected = 0;
    public List<Sprite> spritesPlayable = new List<Sprite>();
    public int TotalAcertado { get; set; }
    public static CardsController GetInstance()
    {
        if (_instance == null) _instance = new CardsController();

        return _instance;
    }

    private CardsController()
    {
        TotalAcertado = 0;
    }
       
    public void Shuffle()
    {
        for (int i = 0; i < spritesPlayable.Count; i++)
        {
            Sprite temp = spritesPlayable[i];
            int randomIndex = Random.Range(i, spritesPlayable.Count);
            spritesPlayable[i] = spritesPlayable[randomIndex];
            spritesPlayable[randomIndex] = temp;
            cards[i].CardFront = spritesPlayable[i];
        }

    }
 

   
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public GameObject CardObject { get; set; }
    public Sprite CardBack { get; set; }
    public Sprite CardFront { get; set; }
    public bool Selected { get; set; }
    public string Name { get; set; }    
    public Button CardButton { get; set; }
    public Card(GameObject cardButton)
    {
        CardObject = cardButton;
        CardButton = CardObject.GetComponent<Button>();
        //CardButton.onClick.AddListener(CardClickAction);
    }

    


}
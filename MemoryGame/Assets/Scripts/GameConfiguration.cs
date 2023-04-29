using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameConfiguration : MonoBehaviour
{
    [SerializeField]
    private GameObject btnPrefab;
    
    [SerializeField]
    private GameObject boardPanel;

    [SerializeField]
    private ELevel level = ELevel.EASY;
    [SerializeField]
    private Sprite spriteBack;
    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private AudioSource popCard;
    [SerializeField]
    private AudioSource wrongSound;
    [SerializeField]
    private AudioSource correctSound;


    private void Awake()
    {
        CreateLevel();   
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateLevel()
    {
        int totalCards=0;
        int indexSprites = 0;
        switch (level)
        {
            case ELevel.EASY:
                totalCards = 16;
                break;
            case ELevel.MEDIUM:
                totalCards = 32;
                break;
            case ELevel.HARD:
                totalCards = 40;
                break;
        }
        for (int i = 0; i < totalCards; i++)
        {
            GameObject btn = Instantiate(btnPrefab);
            btn.name = "Card" +i;

            btn.transform.SetParent(boardPanel.transform, false);

            Card card = new Card(btn) { Name = btn.name, CardBack = spriteBack, Selected = false };
            card.CardButton.onClick.AddListener(() => CardClickAction(card));

            CardsController.GetInstance().cards.Add(card);
            
            if (indexSprites == totalCards/2)
            {
                indexSprites = 0;
            }
            CardsController.GetInstance().spritesPlayable.Add(sprites[indexSprites]);
            indexSprites++;
        }
        CardsController.GetInstance().Shuffle();
        
    }

    public void CardClickAction(Card currentCard)
    {
        if (currentCard.Selected || CardsController._instance.totalSelected >= 2) return;

        //GameObject goCurrent = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        currentCard.CardObject.GetComponent<Button>().image.sprite = currentCard.CardFront;
      
        if (CardsController._instance.totalSelected == 0)
        {
            currentCard.Selected = true;
            CardsController._instance.totalSelected++;
            popCard.PlayOneShot(popCard.clip);
        }
        else
        {
            CardsController._instance.totalSelected++;
            StartCoroutine(CheckCardMatch(currentCard));
        }
         
    }
    public IEnumerator CheckCardMatch(Card currentCard)
    {
        yield return new WaitForSeconds(1f);
        Card firstSelected = CardsController._instance.cards.Find(c => c.Selected == true);    

        if (firstSelected.CardFront == currentCard.CardFront)
        {
            correctSound.PlayOneShot(correctSound.clip);
            DisableCard(currentCard);
            DisableCard(firstSelected);
            CardsController._instance.cards.Remove(currentCard);
            CardsController._instance.cards.Remove(firstSelected);
          
            if (CardsController._instance.cards.Count == 0)
            {
                Debug.Log("Game Over!!!");
                GameRestart();
            }
        }
        else
        {
            wrongSound.PlayOneShot(wrongSound.clip);
            firstSelected.CardObject.GetComponent<Button>().image.sprite = spriteBack;
            currentCard.CardObject.GetComponent<Button>().image.sprite = spriteBack;
        }
        currentCard.Selected = false;
        firstSelected.Selected = false;
        CardsController._instance.totalSelected = 0;
    
    }

    public void DisableCard(Card card)
    {
        card.CardObject.GetComponent<Button>().image.color = new Color(0, 0, 0, 0);
        card.CardObject.GetComponent<Button>().interactable = false;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }

}

public enum ELevel
{
    EASY,
    MEDIUM,
    HARD
}
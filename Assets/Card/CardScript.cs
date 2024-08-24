using UnityEngine;
using UnityEngine.Events;

public class CardClickEvent : UnityEvent<CardScript> { }

public class CardScript : MonoBehaviour
{
    public CardData cardData; // Reference to the CardData scriptable object
    public SpriteRenderer spriteRenderer; // To render the card sprite

    public CardClickEvent onCardClicked;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCardVisual();

        if (onCardClicked == null)
            onCardClicked = new CardClickEvent();
    }

    public void InitializeCard(CardData data)
    {
        cardData = data;
        cardData.IsFaceUp = false; // Set the card face down initially
        UpdateCardVisual(); // Update visual when card data is set or changed

    }

    public void Flip()
    {
        cardData.IsFaceUp = !cardData.IsFaceUp;
        UpdateCardVisual();
    }

    private void UpdateCardVisual()
    {
        if (!cardData)
        {
            Debug.Log("Card data is null");
            return;
        }
        // spriteRenderer.sprite = cardData.IsFaceUp ? cardData.FaceUpSprite : cardData.FaceDownSprite;
        if (cardData?.FaceUpSprite)
        {
            spriteRenderer.sprite = cardData.FaceUpSprite;
        }
        else
        {
            Debug.Log("Card data error: Missing sprite references at " + cardData.name);
        }
    }


    public void OnMouseDown()
    {
        if (onCardClicked == null)
        {
            Debug.LogError("onCardClicked is null");
        }
        else
        {
            // Emit an event that this card has been clicked
            onCardClicked.Invoke(this);
        }
    }
}

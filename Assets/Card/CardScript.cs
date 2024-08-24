using UnityEngine;

public class CardScript : MonoBehaviour
{
    public CardData cardData; // Reference to the CardData scriptable object
    public SpriteRenderer spriteRenderer; // To render the card sprite

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCardVisual();
    }

    public void InitializeCard(CardData data)
    {
        cardData = data;
        cardData.IsFaceUp = true; // Set the card face down initially
        UpdateCardVisual(); // Update visual when card data is set or changed

    }

    public void Flip()
    {
        cardData.IsFaceUp = !cardData.IsFaceUp;
        UpdateCardVisual();
    }

    private void UpdateCardVisual()
    {
        spriteRenderer.sprite = cardData.IsFaceUp ? cardData.FaceUpSprite : cardData.FaceDownSprite;
    }
}

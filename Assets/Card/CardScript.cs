using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardClickEvent : UnityEvent<CardScript> { }

public class CardScript : MonoBehaviour
{
    public CardData cardData; // Reference to the CardData scriptable object
    public SpriteRenderer spriteRenderer; // To render the card sprite

    public CardClickEvent onCardClicked;

    public List<CardScript> dependsOnCards = new List<CardScript>();  // Cards that need to be removed before this can flip

    public bool IsCollected { get; set; } = false;

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
        this.IsCollected = false;
        cardData.IsFaceUp = false; // Set the card face down initially
        UpdateCardVisual(); // Update visual when card data is set or changed

    }

    public void Flip()
    {
        // cardData.IsFaceUp = !cardData.IsFaceUp;
         cardData.IsFaceUp = true;
        UpdateCardVisual();
    }

    private void UpdateCardVisual()
    {
        if (!cardData)
        {
            Debug.Log("Card data is null");
            return;
        }
        spriteRenderer.sprite = cardData.IsFaceUp ? cardData.FaceUpSprite : cardData.FaceDownSprite;
        // if (cardData?.FaceUpSprite)
        // {
            // spriteRenderer.sprite = cardData.FaceUpSprite;
        // }
        // else
        // {
        //     Debug.Log("Card data error: Missing sprite references at " + cardData.name);
        // }
    }


    public void OnMouseDown()
    {
        Debug.Log("Card clicked: " + cardData.name);
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

    public void AddDependency(CardScript dependentCard)
    {
        if (!dependsOnCards.Contains(dependentCard))
            dependsOnCards.Add(dependentCard);
    
    //Print all dependencies name in single line
    Debug.Log("Dependencies: for "+cardData.name +" is: "+ string.Join(", ", dependsOnCards.ConvertAll(c => c.cardData.name)));
    }

    public bool CanFlip()
    {
        foreach (var dep in dependsOnCards)
        {
            if (dep != null && !dep.IsCollected) // Assuming isFaceUp is true when the card is visible
                return false;
        }
        return true; // Can flip if all dependencies are not visible
    }

   public bool  isDependentAreCollected()
    {
        //Check if all dependencies are collected
        bool allCollected = true;
        foreach (var dep in dependsOnCards)
        {
            if (!dep.IsCollected)
                allCollected = false;
        }

        return allCollected;
    

        // foreach (var dep in dependsOnCards)
        // {
        //     if (dep != null && !dep.IsCollected) // Assuming isFaceUp is true when the card is visible
        //         return false;
        // }
        // return true; // Can flip if all dependencies are not visible
    }

    public void handleFlipIfEligible()
    {
        if (isDependentAreCollected())
        {
            Flip();
        }
    }
}

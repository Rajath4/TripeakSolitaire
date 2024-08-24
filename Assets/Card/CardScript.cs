using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

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
        // UpdateCardVisual();

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

    public void FlipWithAnimation(float flipDuration = 0.5f)
    {
        transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Flip();
            transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad).OnComplete(() =>
            {

            });
        });

    }

    private void UpdateCardVisual()
    {
        if (!cardData)
        {
            Debug.Log("Card data is null");
            return;
        }
        spriteRenderer.sprite = cardData.IsFaceUp ? cardData.FaceUpSprite : cardData.FaceDownSprite;
    }


    public void OnMouseDown()
    {
        if (!cardData.IsFaceUp)
        {
            return;
        }
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
        Debug.Log("Dependencies: for " + cardData.name + " is: " + string.Join(", ", dependsOnCards.ConvertAll(c => c.cardData.name)));
    }

    public bool CanFlip()
    {
        if (cardData.IsFaceUp)
        {
            return false;
        }
        foreach (var dep in dependsOnCards)
        {
            if (dep != null && !dep.IsCollected) // Assuming isFaceUp is true when the card is visible
                return false;
        }
        return true; // Can flip if all dependencies are not visible
    }


    public void handleFlipIfEligible()
    {
        if (CanFlip())
        {
            FlipWithAnimation();
        }
    }

    public void MoveToDestination(Transform destination, float duration = 1f)
    {
        // Tween this card's position to the destination's position over the specified duration
        this.transform.DOMove(destination.position, duration).SetEase(Ease.InOutQuad);

        // Optionally, you can also smoothly rotate the card to match the destination's rotation
        this.transform.DORotateQuaternion(destination.rotation, duration).SetEase(Ease.InOutQuad);
    }
}

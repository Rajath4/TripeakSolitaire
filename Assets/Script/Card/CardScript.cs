using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class CardClickEvent : UnityEvent<CardScript> { }

public class CardScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image spriteRenderer; // To render the card sprite

    public CardData cardData; // Reference to the CardData scriptable object
    public CardClickEvent onCardClicked = new CardClickEvent();
    public bool IsCollected { get; set; } = false;
    public bool IsDeckCard { get; set; } = false;

    private List<CardScript> dependsOnCards = new List<CardScript>();  // Cards on which current card depends on to be flipped.

    public void InitializeCard(CardData data)
    {
        cardData = data;
        this.IsCollected = false;
        cardData.IsFaceUp = false;
        UpdateCardVisual();
    }

    public void Flip()
    {
        cardData.IsFaceUp = !cardData.IsFaceUp;
        UpdateCardVisual();
    }

    public async Task handleFlipIfEligible()
    {
        if (CanFlip())
        {
            await FlipWithAnimation();
        }
        else
        {
            await Task.CompletedTask; // Returns immediately, no animation needed
        }
    }

    public async Task FlipWithAnimation(float flipDuration = 0.5f)
    {
        spriteRenderer.raycastTarget = false;

        await transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        Flip();
        await transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();

        // Reset the transform rotation to 0 degrees for standard alignment
        transform.localEulerAngles = Vector3.zero;
        spriteRenderer.raycastTarget = true;
    }


    private void UpdateCardVisual()
    {
        if (cardData == null)
        {
            Debug.LogError("Card data is not set!");
            return;
        }
        spriteRenderer.sprite = cardData.IsFaceUp ? cardData.FaceUpSprite : cardData.FaceDownSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + cardData.name);
        ProcessCardClick();
    }

    private void ProcessCardClick()
    {
        if (!cardData.IsFaceUp && !IsDeckCard)
        {
            Debug.LogWarning("Card is not face up or is a deck card, cannot interact.");
            return;
        }
        Debug.Log("Card clicked: " + cardData.name);
        onCardClicked.Invoke(this);
    }

    public void AddDependency(CardScript dependentCard)
    {
        if (!dependsOnCards.Contains(dependentCard))
        {
            dependsOnCards.Add(dependentCard);
        }
    }

    private bool CanFlip()
    {
        return !cardData.IsFaceUp && dependsOnCards.TrueForAll(dep => dep == null || dep.IsCollected);
    }

    public async Task MoveToDestination(Transform destination, float duration = 1f)
    {
        await transform.DOMove(destination.position, duration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    }

    public void ShakeCard()
    {
        transform.DOShakePosition(0.5f, strength: new Vector3(10f, 0f, 0f), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
    }
}

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
    public CardData cardData; // Reference to the CardData scriptable object
    public Image spriteRenderer; // To render the card sprite

    public CardClickEvent onCardClicked;

    public List<CardScript> dependsOnCards = new List<CardScript>();  // Cards that need to be removed before this can flip

    public bool IsCollected { get; set; } = false;

    public bool IsDeckCard { get; set; } = false;

    private void Awake()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();
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
        // Temporarily disable raycast targeting during the flip
        spriteRenderer.raycastTarget = false;

        // Start the first half of the rotation to 90 degrees
        transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // Change card visuals at the halfway point
            Flip();

            // Complete the rotation from 90 to 180 degrees
            transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                // Reset the transform rotation to 0 degrees for standard alignment
                transform.localEulerAngles = Vector3.zero;

                // Re-enable raycast targeting after the animation
                spriteRenderer.raycastTarget = true;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + cardData.name);
        this.OnMouseDown();
    }


    public void OnMouseDown()
    {
        if (!cardData.IsFaceUp && !IsDeckCard)
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

    public Task MoveToDestination(Transform destination, float duration = 1f)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        // Start the DOTween animation and use OnComplete to signal the task's completion
        this.transform.DOMove(destination.position, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            tcs.SetResult(true);
        });

        // Await the TaskCompletionSource's task
        return tcs.Task;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public Suit Suit; // Use the globally defined Suit
    public Rank Rank; // Use the globally defined Rank
    public bool IsFaceUp;

    public Sprite FaceUpSprite;
    public Sprite FaceDownSprite;
}

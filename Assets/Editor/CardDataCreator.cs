using UnityEditor;
using UnityEngine;

public class CardDataCreator : Editor
{
    [MenuItem("Tools/Create All Card Data")]
    public static void CreateAllCardData()
    {
        string[] suits = new string[] { "Clubs", "Diamonds", "Hearts", "Spades" };
        string[] ranks = new string[] { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };

        string cardBackPath = "Assets/Sprites/Cards/Back Red 1.png"; // Path to the card back sprite
        Sprite cardBackSprite = AssetDatabase.LoadAssetAtPath<Sprite>(cardBackPath);

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                string cardName = $"{suit.ToLower()}_of_{rank.ToLower()}";
                string spritePath = $"Assets/Sprites/Cards/{cardName}.png";
                Sprite cardFrontSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (cardFrontSprite == null)
                {
                    Debug.LogError($"Sprite not found at path: {spritePath}");
                }

                CreateCardData(rank, suit, cardFrontSprite, cardBackSprite);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("All card data created successfully!");
    }

    private static void CreateCardData(string rank, string suit, Sprite frontSprite, Sprite backSprite)
    {
        CardData cardData = ScriptableObject.CreateInstance<CardData>();
        cardData.Suit = (Suit)System.Enum.Parse(typeof(Suit), suit);
        cardData.Rank = (Rank)System.Enum.Parse(typeof(Rank), rank);
        cardData.IsFaceUp = false;
        cardData.FaceUpSprite = frontSprite;
        cardData.FaceDownSprite = backSprite;

        string path = $"Assets/Resources/CardsData/{rank}_of_{suit}.asset";
        AssetDatabase.CreateAsset(cardData, path);
    }
}

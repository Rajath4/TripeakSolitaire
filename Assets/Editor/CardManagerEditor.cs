using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(GameplayController))]
public class CardManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the normal inspector

        GameplayController manager = (GameplayController)target; // Cast the target script to CardManager

        if (GUILayout.Button("Load All CardData"))
        {
            LoadAllCardData(manager);
        }

        if (GUILayout.Button("Validate CardData"))
        {
            ValidateCardData(manager);
        }
    }

    private void LoadAllCardData(GameplayController manager)
    {
        // Path where all CardData ScriptableObjects are stored, e.g., "Assets/Resources/CardData"
        string path = "CardsData";
        // Load all CardData assets from the specified path
        var cardDatas = Resources.LoadAll<CardData>(path).OrderBy(c => c.name).ToArray();
        // Assign loaded CardData array to the CardManager's array
        manager.allCardData = cardDatas;

        // Important: Mark the manager object as 'dirty' so Unity knows to save the changes
        EditorUtility.SetDirty(manager);
    }

    private void ValidateCardData(GameplayController manager)
    {
        foreach (var card in manager.allCardData)
        {
            if (card.FaceUpSprite == null || card.FaceDownSprite == null )
            {
                Debug.LogError($"Card data error in {card.name}: Missing sprite references.");
            }
        }
    }
}

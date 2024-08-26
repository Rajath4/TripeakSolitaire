using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ILayoutIndex
{
    int RowIndex { get; set; }
    int ColumnIndex { get; set; }
}

public class CardGrid : MonoBehaviour
{
    [System.Serializable]
    public struct RowData
    {
        public Transform[] positions; // Positions for each card in this row
    }

    [@SerializeField]
    private RowData[] rows; // Data for each row, set this up in the inspector

    private List<List<CardScript>> cardsToPick = new List<List<CardScript>>();
    private IGridCardClickHandler clickHandler;
    private ICardFactory cardFactory;
    private ICardDataHandler cardDataHandler;

    public void Initialize(ICardDataHandler cardDataHandler, ICardFactory cardFactory, IGridCardClickHandler clickHandler)
    {
        this.cardDataHandler = cardDataHandler;
        this.cardFactory = cardFactory;
        this.clickHandler = clickHandler;
    }

    public void SetupCards(GameObject cardPrefab)
    {
        int currentIndex = 0;
        int currentRow = 0;

        foreach (RowData row in rows)
        {
            List<CardScript> currentRowCardScripts = new List<CardScript>();
            foreach (Transform pos in row.positions)
            {
                CardScript cardScript = cardFactory.CreateDeckCard(cardDataHandler.DrawCard(), cardPrefab, pos);
                cardScript.onCardClicked.AddListener(clickHandler.HandleGridCardClick);
                currentRowCardScripts.Add(cardScript);
                currentIndex++;
            }
            cardsToPick.Add(currentRowCardScripts);
            currentRow++;
        }
    }

    public void ComputeDependencies()
    {
        for (int rowIndex = 0; rowIndex < cardsToPick.Count - 1; rowIndex++)
        {
            List<CardScript> row = cardsToPick[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
            {
                CardScript card = row[columnIndex];
                int[] dependentIndices = GetDependentsIndex(rowIndex, columnIndex);
                int nextRowIndex = rowIndex + 1;
                foreach (int dependentIndicesColumn in dependentIndices)
                {
                    card.AddDependency(cardsToPick[nextRowIndex][dependentIndicesColumn]);
                }
            }
        }
    }

    public async Task CheckForPossibleCardFlips()
    {
        List<Task> flipTasks = new List<Task>();

        foreach (var row in cardsToPick)
        {
            foreach (var card in row)
            {
                // Start all eligible flips and collect the tasks
                flipTasks.Add(card.handleFlipIfEligible());
            }
        }

        // Wait for all flip animations to complete
        await Task.WhenAll(flipTasks);
    }

    public List<CardScript> GetFlippedCards()
    {
        List<CardScript> flippedCards = new List<CardScript>();
        foreach (var row in cardsToPick)
        {
            foreach (var card in row)
            {
                if (card.IsFaceUp() && !card.IsCollected)
                {
                    flippedCards.Add(card);
                }
            }
        }
        return flippedCards;
    }


    private int[] GetDependentsIndex(int currentRowIndex, int currentColumnIndex)
    {
        if (currentRowIndex >= dependents.Length)
        {
            Debug.LogError("Row index out of range");
            return null;
        }
        if (currentColumnIndex >= dependents[currentRowIndex].Length)
        {
            Debug.LogError("Column index out of range");
            return null;
        }
        return dependents[currentRowIndex][currentColumnIndex];
    }

    private int[][][] dependents = new int[][][]
    {
        new int[][]
        {
            new int[] { 0, 1 },
            new int[] { 2, 3 },
            new int[] { 4, 5 }
        },
        new int[][]
        {
            new int[] { 0, 1 },
            new int[] { 1, 2 },
            new int[] { 3, 4 },
            new int[] { 4, 5 },
            new int[] { 6, 7 },
            new int[] { 7, 8 }
        },
         new int[][]
        {
            new int[] { 0, 1 },
            new int[] { 1, 2 },
            new int[] { 2, 3 },
            new int[] { 3, 4 },
            new int[] { 4, 5 },
            new int[] { 5, 6 },
            new int[] { 6, 7 },
            new int[] { 7, 8 },
            new int[] { 8, 9 },
        }
  };
}

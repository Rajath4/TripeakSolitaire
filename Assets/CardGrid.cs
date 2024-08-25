using System.Collections.Generic;
using Unity;
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

    public RowData[] rows; // Data for each row, set this up in the inspector


    public int[] GetDependentsIndex(int currentRowIndex, int currentColumnIndex)
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

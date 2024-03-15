using System;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance;

    public int numRows = 5;
    public int numColumns = 5;
    public float hexWidth = 1.0f;
    public float hexHeight = 1.0f;

    [SerializeField] private ScreenSpaceSizeTest size;

    public Matrix matrix = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        GenerateHexGrid();

        SetNeighborsAll();
    }

    public void GenerateHexGrid()
    {
        GameUtils.Instance.ClearChildren(this.transform);

        matrix = new();

        Vector3 min = new Vector3(float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue);

        int index = 0;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                float xPos = col * hexWidth * 0.75f;
                float yPos = row * hexHeight + (col % 2) * 0.5f * hexHeight;

                GameObject hexagon = Instantiate(HexManager.Instance.hexSlotPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);

                var hexSlot = hexagon.GetComponent<HexSlot>();

                hexagon.transform.SetParent(this.transform);

                hexagon.transform.localPosition = new Vector3(xPos, yPos, 0);

                hexagon.transform.localRotation = Quaternion.Euler(0f, 90, 90);

                hexagon.name = index++.ToString();

                min.x = Mathf.Min(min.x, xPos);
                min.y = Mathf.Min(min.y, yPos);
                min.z = Mathf.Min(min.z, 0);

                max.x = Mathf.Max(max.x, xPos);
                max.y = Mathf.Max(max.y, yPos);
                max.z = Mathf.Max(max.z, 0);

                var element = new Element();
                element.row = row;
                element.col = col;
                element.hexSlot = hexSlot;

                hexSlot.element = element;

                matrix.elements.Add(element);
            }

        }

        var center = new Vector3((min.x + max.x) / 2f, (min.y + max.y) / 2f, (min.z + max.z) / 2f);

        foreach (var element in matrix.elements)
        {
            element.hexSlot.transform.localPosition -= center;
        }

        Bounds bound = new Bounds(center, new Vector3(max.x - min.x + 2, max.y - min.y + 2, max.z - min.z + 2));

        size.E3(bound);
    }

    public Element GetElement(int x, int y)
    {
        var element = matrix.elements.Find(val => val.row == x && val.col == y);

        return element;
    }

    public Element GetElement(HexSlot hexSlot)
    {
        return hexSlot.element;
    }

    public List<HexSlot> GetNeighbors(HexSlot hexSlot)
    {
        var curRow = hexSlot.element.row;
        var curCol = hexSlot.element.col;

        List<HexSlot> neighbors = new();

        for (int row = -1; row <= 1; row++)
        {
            for (int col = -1; col <= 1; col++)
            {
                if (row == 0 && col == 0) continue;

                if (curCol % 2 == 0)
                {
                    if ((row == 1 && col == -1) || (row == 1 && col == 1)) continue;
                }
                else
                {
                    if ((row == -1 && col == -1) || (row == -1 && col == 1)) continue;
                }

                var newRow = curRow + row;
                var newCol = curCol + col;

                if (newRow < 0 || newCol < 0 || newRow >= numRows || newCol >= numColumns) continue;

                neighbors.Add(GetElement(newRow, newCol).hexSlot);
            }
        }

        return neighbors;
    }

    public void SetNeighborsAll()
    {
        foreach (var element in matrix.elements)
        {
            element.hexSlot.neighbors = GetNeighbors(element.hexSlot);
        }
    }

}


[Serializable]
public class Matrix
{
    public List<Element> elements = new();
}

[Serializable]
public class Element
{
    public int row, col;
    public HexSlot hexSlot;
}
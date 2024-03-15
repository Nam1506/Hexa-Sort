using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GridGenerator.Instance.Init();

        HolderGenerator.Instance.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GridGenerator.Instance.GenerateHexGrid();
        }
    }

    public void CheckMergeHex(HexSlot hexSlot)
    {
        var neighbors = hexSlot.neighbors;

        var hexStack = hexSlot.hexStack;

        var curTopHexColor = hexStack.GetTopHex().eHexColor;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.hexStack == null) continue;

            var neighborHexStack = neighbor.hexStack;

            var neighborTopHex = neighborHexStack.GetTopHex();

            var neighborTopHexColor = neighborTopHex.eHexColor;

            if (curTopHexColor == neighborTopHexColor)
            {
                hexStack.AddHex(neighborHexStack.GetListTopHex());

                neighborHexStack.RemoveHex(neighborHexStack.GetListTopHex());

                return;
            }
        }
    }
}

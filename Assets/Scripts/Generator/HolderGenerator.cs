using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HolderGenerator : MonoBehaviour
{
    public static HolderGenerator Instance;

    public List<Transform> places;

    public List<FakeData> fakeDatas;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        GenerateStackHexa();
    }

    public void GenerateStackHexa()
    {
        for (int i = 0; i < places.Count; i++)
        {
            var hexStack = Instantiate(HexManager.Instance.hexStackPrefab, places[i]).GetComponent<HexStack>();

            hexStack.GenerateHex(fakeDatas[i]);
        }
    }

    public void Regenerate()
    {
        foreach (var place in places)
        {
            if (place.childCount != 0)
            {
                return;
            }
        }

        GenerateStackHexa();
    }

}


[Serializable]
public class FakeDatas
{
    public List<FakeData> fakeDatas;
}

[Serializable]
public class FakeData
{
    public List<EHexColor> data;
}
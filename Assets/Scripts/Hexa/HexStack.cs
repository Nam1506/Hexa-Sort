using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HexStack : MonoBehaviour
{
    public List<Hex> hexStacks = new();
    public Collider hexCollider;

    public Transform parentTransform;

    private Vector3 screenPoint;
    public float offsetY;

    private HexSlot curHexSlot;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + new Vector3(0, offsetY, 0);

        curPosition.z = -0.5f;

        transform.position = curPosition;

        CheckRaycast(GetBottomHex());

    }

    private void OnMouseUp()
    {
        if (!curHexSlot)
        {
            transform.localPosition = Vector3.zero;
            return;
        }

        curHexSlot?.UnSelect();

        if (curHexSlot.IsAvailable())
        {
            curHexSlot.SetHexStack(this);

            hexCollider.enabled = false;
        }
        else
        {
            transform.localPosition = Vector3.zero;

            curHexSlot = null;
        }

    }

    public Hex GetTopHex()
    {
        if (hexStacks.Count == 0) return null;

        return hexStacks[hexStacks.Count - 1];
    }

    public Hex GetBottomHex()
    {
        if (hexStacks.Count == 0) return null;

        return hexStacks[0];
    }


    public List<Hex> GetListTopHex()
    {
        if (hexStacks.Count == 0) return null;

        List<Hex> results = new();

        var topHex = hexStacks[hexStacks.Count - 1];

        var eHexColor = topHex.eHexColor;

        results.Add(topHex);

        for (int i = hexStacks.Count - 2; i >= 0; i--)
        {
            if (hexStacks[i].eHexColor != eHexColor)
            {
                return results;
            }

            results.Add(hexStacks[i]);
        }

        return results;
    }

    public float GetCurrentOffsetY()
    {
        return -0.2f * (hexStacks.Count);
    }

    public float GetCurrentOffsetX()
    {
        //if (hexStacks.Count == 0) return 0;

        return 0.01f * (hexStacks.Count - 1);
    }

    public void GenerateHex(FakeData fakeData)
    {
        for (int i = 0; i < fakeData.data.Count; i++)
        {
            var hex = Instantiate(HexManager.Instance.hexPrefab, this.transform).GetComponent<Hex>();

            hex.SetHexColor(fakeData.data[i]);

            var pos = Vector3.zero;

            pos.x = 0.02f * (i);

            pos.y = -0.2f * (i + 1);

            hex.transform.localPosition = pos;

            hexStacks.Add(hex);
        }
    }

    public void AddHex(Hex hex)
    {
        hexStacks.Add(hex);

        hex.transform.SetParent(this.transform);

        var pos = Vector3.zero;

        pos.x = GetCurrentOffsetX();

        pos.y = GetCurrentOffsetY();

        hex.transform.DOLocalMove(pos, 0.5f);
    }

    public void AddHex(List<Hex> hexs, HexStack hexStack)
    {
        StartCoroutine(IEAddHex(hexs, hexStack));
    }

    private IEnumerator IEAddHex(List<Hex> hexs, HexStack hexStack)
    {
        var firstHex = hexs[0];

        var firstPos = Vector3.zero;

        firstPos.x = GetCurrentOffsetX();

        firstPos.y = GetCurrentOffsetY();

        Vector3 midPoint = firstHex.transform.localPosition + (firstPos - firstHex.transform.localPosition) / 2f;

        midPoint.y = Mathf.Max(firstHex.transform.localPosition.y, firstPos.y);

        midPoint.y -= 0.2f * hexs.Count;

        float timeMove = 0.5f;

        foreach (var hex in hexs)
        {
            hexStacks.Add(hex);

            hex.transform.SetParent(this.transform);

            var targetPos = Vector3.zero;

            targetPos.x = GetCurrentOffsetX();

            targetPos.y = GetCurrentOffsetY();

            //Vector3 dr = (targetPos - hex.transform.localPosition);

            Vector3 dr = (transform.position - hexStack.transform.position);

            dr.y = midPoint.y;

            Vector3 axis = Vector3.Cross(dr, Vector3.up);

            hex.transform.DOLocalPath(new Vector3[] { hex.transform.localPosition, midPoint, targetPos }, timeMove, PathType.CatmullRom, PathMode.Full3D, 10);

            hex.RotateAround(axis, timeMove);

            yield return new WaitForSeconds(0.03f);
        }
    }

    public void RemoveHex(Hex hex)
    {
        hexStacks.Remove(hex);
    }

    public void RemoveHex(List<Hex> hexs)
    {
        foreach (var hex in hexs)
        {
            RemoveHex(hex);
        }

        if (hexStacks.Count == 0)
        {
            curHexSlot.hexStack = null;
            curHexSlot = null;
        }
    }

    private void CheckRaycast(Hex hex)
    {
        var curPosition = hex.transform.position;
        RaycastHit hitInfo;

        if (Physics.Raycast(curPosition, hex.transform.TransformDirection(Vector3.up), out hitInfo, 10f))
        {
            var hexSlot = hitInfo.collider.GetComponent<HexSlot>();

            if (hexSlot != curHexSlot || curHexSlot == null)
            {
                curHexSlot?.UnSelect();

                curHexSlot = hexSlot;

                curHexSlot.Select();
            }
        }
        else
        {
            if (curHexSlot != null)
            {
                curHexSlot?.UnSelect();
                curHexSlot = null;
            }
        }
    }
}

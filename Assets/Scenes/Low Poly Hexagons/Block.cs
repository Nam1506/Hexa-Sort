using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void RotateAround(Vector3 point, Vector3 axis, float duration, Vector3 target)
    {
        StartCoroutine(Rotate(point, axis, duration, target));
    }

    IEnumerator Rotate(Vector3 point, Vector3 axis, float duration, Vector3 target)
    {
        float lerp = 180 / duration * Time.deltaTime;
        float angle = 0;
        while (angle < 180)
        {
            transform.RotateAround(point, axis, lerp);
            yield return null;
            angle += lerp;
        }
        transform.position = target;
        transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 90f));
    }
}

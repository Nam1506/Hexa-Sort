using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceSizeTest : MonoBehaviour
{

    public static float ScreenSpaceSize = 3;

    public Camera cam;
    public GameObject target;
    public Transform root;
    public float minSize = 4;
    public float offset;

    public Transform offsetPos;

    public void E()
    {
        Bounds bounds = GetBoundsWithChildren(target);
        float virtualsphereRadius = Vector3.Magnitude(bounds.max - bounds.center);
        float minD = (virtualsphereRadius) / Mathf.Sin(Mathf.Deg2Rad * cam.fieldOfView / 2);
        Vector3 normVectorBoundsCenter2CurrentCamPos = (cam.transform.position - bounds.center) / Vector3.Magnitude(cam.transform.position - bounds.center);
        cam.transform.position = minD * normVectorBoundsCenter2CurrentCamPos;
        cam.transform.LookAt(bounds.center);
        cam.nearClipPlane = minD - virtualsphereRadius;
    }
    Camera camera => cam;
    public float marginPercentage = 1;
    public float cameraViewSize = 1;
    [ContextMenu("Bound 2")]
    public void E2()
    {
        Bounds bounds = GetBoundsWithChildren(target);
        var centerAtFront = new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
        var centerAtFrontTop = new Vector3(bounds.center.x, bounds.max.y, bounds.max.z);
        var centerToTopDist = (centerAtFrontTop - centerAtFront).magnitude;
        var minDistance = (centerToTopDist * marginPercentage) / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, -minDistance);
        camera.transform.LookAt(bounds.center);
    }

    [ContextMenu("Bound 3")]
    public void E3(Bounds bounds)
    {
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float tmpMarginPercentage = marginPercentage;

        if (objectSize > 2 && objectSize < 6)
        {
            tmpMarginPercentage += 0.15f;
        }
        else if (objectSize >= 10)
        {
            tmpMarginPercentage -= 0.15f;
        }

        float cameraDistance = tmpMarginPercentage; // Constant factor
        objectSize = Mathf.Max(minSize, objectSize);
        ScreenSpaceSize = objectSize;

        //camera.fieldOfView = cameraControl.defaultFov;
        float cameraView = cameraViewSize * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += offset * objectSize; // Estimated offset from the center to the outside of the object

        //camera.transform.position = bounds.center - distance * camera.transform.forward;

        var pos = bounds.center - distance * camera.transform.forward;

        var minZ = Mathf.Min(-32f, pos.z);

        offsetPos.localPosition = new Vector3(0, -1, minZ);
    }

    public static Vector3 GetRotatedPointDelta(Vector3 startPosition, Vector3 rotationCentre, Vector3 rotationAxis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, rotationAxis);
        Vector3 localPosition = startPosition - rotationCentre;
        return (q * localPosition) - localPosition;
    }

    public static Bounds GetBoundsWithChildren(GameObject gameObject)
    {
        // GetComponentsInChildren() also returns components on gameobject which you call it on
        // you don't need to get component specially on gameObject
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        // If renderers.Length = 0, you'll get OutOfRangeException
        // or null when using Linq's FirstOrDefault() and try to get bounds of null
        Bounds bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds();

        // Or if you like using Linq
        // Bounds bounds = renderers.Length > 0 ? renderers.FirstOrDefault().bounds : new Bounds();

        // Start from 1 because we've already encapsulated renderers[0]
        for (int i = 1; i < renderers.Length; i++)
        {
            if (renderers[i].enabled)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
        }

        return bounds;
    }

}
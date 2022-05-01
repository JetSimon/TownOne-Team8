using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DymamicCamera : MonoBehaviour
{
    public Camera targetCamera;

    public float cameraDamping = 5.0f;
    public List<Transform> followedTransforms = new List<Transform>();
    public float additionalPadding = 0.5f;
    public float minSize = 1;

    public void Update()
    {
        if (followedTransforms.Count > 0)
        {
            //Calculate min, max bounds
            Vector2 min = followedTransforms[0].position;
            Vector2 max = followedTransforms[0].position;
            for (int i = 1; i < followedTransforms.Count; i++)
            {
                min = Vector3.Min(min, followedTransforms[i].position);
                max = Vector3.Max(max, followedTransforms[i].position);
            }
            min -= Vector2.one * additionalPadding;
            max += Vector2.one * additionalPadding;

            //Calculate middle, and necessary dimensions
            Vector3 middle = (min + max) / 2;
            middle.z = -1;
            Vector3 dimensions = max - min;


            //Lerp to camera position
            targetCamera.transform.position = Vector3.Lerp(targetCamera.transform.position, middle, Time.deltaTime * cameraDamping);

            //Lerp size
            float size = Mathf.Max(dimensions.y / 2, dimensions.x / targetCamera.aspect / 2);
            size = Mathf.Max(size, minSize);
            targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, size, Time.deltaTime * cameraDamping);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollow : MonoBehaviour
{
    public Transform sourceTransform; // The source transform to copy the position from
    public Transform targetTransform; // The target transform to apply the copied and offset position
    public Vector3 offset = Vector3.zero; // The offset to apply to the copied position

    void Update()
    {
        if (sourceTransform != null && targetTransform != null)
        {
            // Copy the position from the source transform
            Vector3 newPosition = sourceTransform.position;

            // Apply the offset
            newPosition += offset;

            // Set the position of the target transform
            targetTransform.position = newPosition;
        }
    }
}

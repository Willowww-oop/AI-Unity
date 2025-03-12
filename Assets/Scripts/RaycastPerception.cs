using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class RaycastPreception : Perception
{
    [SerializeField] int numRaycast = 2;
    public override GameObject[] GetGameObjects()
    {
        // Create result list

        List<GameObject> result = new List<GameObject>();

        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);
        
        // Iterate throug directions
        foreach (Vector3 direction in directions)
        {
            Ray ray = new Ray(transform.position, transform.rotation * direction);

            // Raycast ray

            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
            {
                // Check if collision is self, skip if so
                if (raycastHit.collider.gameObject == gameObject) continue;

                // Check tag, skip if tagName != "" and !CompareTag
                if (tagName != "" && !raycastHit.collider.CompareTag(tagName)) continue;

                // Add game object to results
                result.Add(raycastHit.collider.gameObject);
            }
        }

        // Convert list to Array
        return result.ToArray();
    }

    public override bool GetOpenDirection(ref Vector3 openDirection)
    {
        // Get array of directions using Ultilites.GetDirectionsInCircle
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRaycast, maxAngle);

        // Iterate through directions 
        foreach (var direction in directions)
        {
            // Cast ray from transform position in the direction of (transform.rotation * direction)
            Ray ray = new Ray(transform.position, transform.rotation * direction);

            // If there are no reycast hit then that is an open direction
            if (!Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
            {
                // Set open direction
                openDirection = ray.direction;
                return true;
            }
        }

        // No open direction
        return false;
    }
}

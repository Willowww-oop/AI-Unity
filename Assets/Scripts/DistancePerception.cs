using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{
    public override GameObject[] GetGameObjects() 
    {
        List<GameObject> result = new List<GameObject>();

        var colliders = Physics.OverlapSphere(transform.position, maxDistance);
        
        // Getting all of the colliders inside of the sphere

        foreach ( var collider in colliders )
        {
            // Checking to not include ourselves

            if (collider.gameObject == gameObject) continue;

            // Checking if the tags match

            if (tagName == "" || collider.tag == tagName)
            {
                // Is the object within max angle range

                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle <= maxAngle)
                {
                    result.Add(collider.gameObject);
                }
            }
        }

        return result.ToArray();
    }
}

using UnityEngine;

public class StateAgent : AIAgent
{
    [SerializeField] Perception perception;

    private void Update()
    {
        if (perception != null)
        {
            var gameObjects = perception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.Destination = gameObjects[0].transform.position;
            }
        }
    }
}

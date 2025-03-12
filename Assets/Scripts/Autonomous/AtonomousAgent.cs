using Unity.VisualScripting;
using UnityEngine;

public class AtonomousAgent : AIAgent
{
    [SerializeField] AutonomousData data;

    [Header("Preception")]
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;
    public Perception obstaclePerception;

    float angle;

    private void Update()
    {
        // SEEK
        if (seekPerception != null)
        { 
            var gameObjects = seekPerception.GetGameObjects();

            if (gameObjects.Length > 0 )
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        // FLEE
        if (fleePerception != null)
        {
            var gameObjects = fleePerception.GetGameObjects();

            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        // FLOCK
        if (flockPerception != null)
        {
            var gameObjects = flockPerception.GetGameObjects();

            if (gameObjects.Length > 0)
            {
                movement.ApplyForce(Cohesion(gameObjects) * data.cohesionWeight);
                movement.ApplyForce(Separation(gameObjects, data.seperationRadius) * data.seperationWeight);
                movement.ApplyForce(Allignment(gameObjects) * data.alignmentWeight);
            }
        }

        // OBSTACLE
        if (obstaclePerception != null && obstaclePerception.CheckDirection(Vector3.forward))
        {
            Vector3 direction = Vector3.zero;
            if(obstaclePerception.GetOpenDirection(ref direction))
            {
                Debug.DrawRay(transform.position, transform.rotation * Vector3.forward * 3, Color.red, 0.5f);
                movement.ApplyForce(GetSteeringForce(direction) * data.obstacleWeight);
            }
        }

        // WANDER
        if (movement.Acceleration.sqrMagnitude == 0)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }

        Vector3 acceleration = movement.Acceleration;
        acceleration.y = 0;
        movement.Acceleration = acceleration;

        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);
        }

        float size = 25;
        transform.position = Utilities.Wrap(transform.position, new Vector3(-size, -size, -size), new Vector3(size, size, size));
    }

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            positions += neighbor.transform.position; 
        }

        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;

        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;  
        foreach (var neighbor in neighbors)
        {
            Vector3 direction = (transform.position - neighbor.transform.position);
            if (direction.magnitude < radius)
            {
                separation += direction / direction.sqrMagnitude;
            }
        }

        Vector3 force = GetSteeringForce(separation);
        return force;
    }

    private Vector3 Allignment(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;
        
        foreach(var neighbor in neighbors)
        {
            velocities += neighbor.GetComponent<AIAgent>().movement.Velocity;
            
        }
            Vector3 averageVelocity = velocities / neighbors.Length;    
            Vector3 force = GetSteeringForce(averageVelocity);
            return force;
    }

    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = transform.position - go.transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Wander()
    {
        // Changes the angle randomly
        angle += Random.Range(-data.displacement, data.displacement);

        // Creates the rotation quaternion around the y-axis
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        // Calcuating the point on the circle radius
        Vector3 point = rotation * (Vector3.forward * data.radius);

        // Seting the point in fornt of an agent at the distance length
        Vector3 forward = movement.Direction * data.distance;
        Vector3 force = GetSteeringForce(forward + point);
        //movement.ApplyForce(force);

        return force;
    }

    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxSpeed);

        return force;
    }

}
    
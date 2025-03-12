using UnityEngine;

[CreateAssetMenu(fileName = "AutonomousData", menuName = "Data/AutonomousData")]
public class AutonomousData : ScriptableObject
{
    [Range(0, 10)] public float distance;
    [Range(0, 10)] public float radius;
    [Range(0, 180)] public float displacement;

    [Range(0, 5)] public float cohesionWeight;
    [Range(0, 5)] public float seperationWeight;
    [Range(0, 5)] public float seperationRadius;
    [Range(0, 5)] public float alignmentWeight;
    [Range(0, 20)] public float obstacleWeight;
}

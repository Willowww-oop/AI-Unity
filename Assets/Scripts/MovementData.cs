using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Data/MovementData")]
public class MovementData : ScriptableObject
{
    [SerializeField, Range(1, 20)] public float minSpeed = 5;
    [SerializeField, Range(1, 20)] public float maxForce = 5;
    [SerializeField, Range(1, 50)] public float maxSpeed = 5;
    [SerializeField, Range(1, 50)] public float turnRate = 5;
}

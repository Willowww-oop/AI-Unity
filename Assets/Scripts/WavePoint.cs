using UnityEngine;

public class WavePoint : MonoBehaviour
{
    [SerializeField] WavePoint[] wavePoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<NavAgent>(out NavAgent agent))
        {
           //agent.wavepoint = wavePoints[Random.Range(0, wavePoints.Length)];
        }
    }
}

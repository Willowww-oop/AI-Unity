using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField] AIAgent[] agents;
    [SerializeField] LayerMask layerMask;
    [SerializeField, Range(0, 5)] float randomMax = 0;

    int index = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) index = ++index % agents.Length;

        if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.LeftShift)))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
            {
                Instantiate(agents[index], hitInfo.point + Random.onUnitSphere * randomMax * Random.value, Quaternion.identity);
            }
        }
    }
}

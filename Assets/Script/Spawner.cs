using UnityEngine;

    // Spawn du barril
public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        // Rapiditer du spawner
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

}
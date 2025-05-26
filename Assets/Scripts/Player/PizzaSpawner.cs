using UnityEngine;

public class PizzaSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject pizzaPrefab; // the prefab to instantiate
    public Transform table1SpawnPoint;
    public Transform table2SpawnPoint;
    public PizzaManager manager;

    private GameObject currentPizza;

    void Start()
    {
        RespawnPizza();
    }

    public void RespawnPizza()
    {
        if (currentPizza != null)
        {
            Destroy(currentPizza);
        }

        Transform spawnPoint = Random.value < 0.5f ? table1SpawnPoint : table2SpawnPoint;
        Vector3 spawnPos = spawnPoint.position + new Vector3(0, 1, 0);

        currentPizza = Instantiate(pizzaPrefab, spawnPos, spawnPoint.rotation);
        manager.AssignPizza(currentPizza); // send it to PizzaManager
    }
}

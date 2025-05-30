using UnityEngine;
public class DeliverySpawner : MonoBehaviour
{
    [Header("Delivery Setup")]
    public GameObject deliveryZonePrefab;
    public Transform[] deliveryLocations;

    [HideInInspector] public Transform currentDeliveryPoint;

    private GameObject currentDeliveryZone;

    void Start()
    {
        SpawnNewDeliveryZone();
    }

    public void SpawnNewDeliveryZone()
    {
        if (currentDeliveryZone != null)
            Destroy(currentDeliveryZone);

        int index = Random.Range(0, deliveryLocations.Length);
        Vector3 spawnPos = deliveryLocations[index].position + new Vector3(0, 0.1f, 0);

        currentDeliveryZone = Instantiate(deliveryZonePrefab, spawnPos, Quaternion.Euler(0, -90, 0));
        currentDeliveryZone.tag = "DeliveryZone";

        currentDeliveryPoint = currentDeliveryZone.transform;
    }
}

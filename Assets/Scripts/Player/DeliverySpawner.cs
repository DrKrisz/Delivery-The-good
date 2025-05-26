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
        Invoke(nameof(SpawnNewDeliveryZone), 2.5f);
    }

    public void SpawnNewDeliveryZone()
    {
        if (currentDeliveryZone != null)
            Destroy(currentDeliveryZone);

        int index = Random.Range(0, deliveryLocations.Length);
        Vector3 spawnPos = deliveryLocations[index].position + new Vector3(0, 0.1f, 0);

        currentDeliveryZone = Instantiate(deliveryZonePrefab, spawnPos, Quaternion.identity);
        currentDeliveryZone.tag = "DeliveryZone";

        currentDeliveryPoint = currentDeliveryZone.transform;
    }
}

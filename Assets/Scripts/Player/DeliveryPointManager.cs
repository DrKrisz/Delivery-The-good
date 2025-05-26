using UnityEngine;

public class DeliveryPointManager : MonoBehaviour
{
    [Header("Delivery Points")]
    public Transform[] deliveryLocations; // Assign house1, house2, house3 in the Inspector
    private Transform currentDeliveryPoint;

    void Start()
    {
        RandomizeDeliveryPoint();
    }

    public void RandomizeDeliveryPoint()
    {
        int index = Random.Range(0, deliveryLocations.Length);
        currentDeliveryPoint = deliveryLocations[index];
        transform.position = currentDeliveryPoint.position + new Vector3(0, 1, 0); // adjust Y to avoid z-fighting
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DeliveryCompass : MonoBehaviour
{
    public DeliverySpawner spawner;
    public RectTransform arrowUI;

    private Transform target;
    private Transform player;

    void Start()
    {
        player = transform;
        if (spawner != null)
            target = spawner.currentDeliveryPoint;
        DeliverySpawner.DeliveryZoneSpawned += OnDeliveryZoneSpawned;
    }

    void OnDestroy()
    {
        DeliverySpawner.DeliveryZoneSpawned -= OnDeliveryZoneSpawned;
    }

    void Update()
    {
        if (arrowUI == null || target == null || player == null)
            return;

        Vector3 localPos = player.InverseTransformPoint(target.position);
        float angle = Mathf.Atan2(localPos.x, localPos.z) * Mathf.Rad2Deg;
        arrowUI.localEulerAngles = new Vector3(0f, 0f, -angle);
    }

    void OnDeliveryZoneSpawned(Transform newTarget)
    {
        target = newTarget;
    }
}

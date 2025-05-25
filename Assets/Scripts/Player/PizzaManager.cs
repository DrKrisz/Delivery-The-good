using UnityEngine;

public class PizzaManager : MonoBehaviour
{
    private bool hasPizza = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza") && !hasPizza)
        {
            hasPizza = true;
            Debug.Log("Pizza picked up!");
            other.gameObject.SetActive(false); // hide pizza
        }

        if (other.CompareTag("DeliveryZone") && hasPizza)
        {
            hasPizza = false;
            Debug.Log("Pizza delivered!");
            // Optionally teleport player
            transform.position = new Vector3(0, 1, 0); // <- change to your teleport target
        }
    }
}

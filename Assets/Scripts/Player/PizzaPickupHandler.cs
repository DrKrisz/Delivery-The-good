using UnityEngine;
using TMPro;

public class PizzaPickupHandler : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public LayerMask pizzaLayer;
    public Transform cameraTransform;
    public GameObject pickupPrompt;

    private GameObject lookedAtPizza;

    void Update()
    {
        PizzaManager manager = FindObjectOfType<PizzaManager>();
        if (manager != null && manager.HasPizza())
        {
            pickupPrompt.SetActive(false);
            lookedAtPizza = null;
            return;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pizzaLayer))
        {
            if (hit.collider.CompareTag("PizzaPickup"))
            {
                lookedAtPizza = hit.collider.gameObject;
                pickupPrompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickupPizza();
                }

                return;
            }
        }

        // Hide prompt if not looking at pizza
        lookedAtPizza = null;
        pickupPrompt.SetActive(false);
    }


    void PickupPizza()
    {
        if (lookedAtPizza == null) return;

        PizzaManager manager = FindObjectOfType<PizzaManager>();
        if (manager != null)
        {
            manager.PickedUpPizza(lookedAtPizza);
        }

        // Hide prompt and clear reference
        pickupPrompt.SetActive(false);
        lookedAtPizza = null;
    }
}
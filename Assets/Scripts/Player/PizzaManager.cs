using UnityEngine;
using TMPro; // add this at the top


public class PizzaManager : MonoBehaviour
{
    private bool hasPizza = false;
    private bool pizzaDelivered = false;
    private GameObject currentPizza;

    private float money = 0f;

    [Header("References")]
    public Transform pizzaHoldPoint;
    public DeliverySpawner deliverySpawner;
    public PizzaSpawner spawner;
    public TMP_Text moneyAmountText;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == currentPizza && !hasPizza && !pizzaDelivered)
        {
            hasPizza = true;
            Debug.Log("Pizza picked up!");

            currentPizza.transform.SetParent(pizzaHoldPoint);
            currentPizza.transform.localPosition = Vector3.zero;
            currentPizza.transform.localRotation = Quaternion.identity;
        }

        if (other.CompareTag("DeliveryZone") && hasPizza)
        {
            hasPizza = false;
            pizzaDelivered = true;
            Debug.Log("Pizza delivered!");

            currentPizza.transform.SetParent(null);
            Transform dropPoint = deliverySpawner.currentDeliveryPoint;
            Vector3 dropPos = dropPoint.position + new Vector3(0, 1, 0);
            currentPizza.transform.position = dropPos;
            currentPizza.transform.rotation = dropPoint.rotation;


            float reward = Random.Range(1.00f, 5.00f);
            reward = Mathf.Round(reward * 100f) / 100f;
            money += reward;

            Debug.Log($"You earned ${reward}!");
            UpdateMoneyUI();

            spawner.Invoke("RespawnPizza", 2.5f);
            deliverySpawner.SpawnNewDeliveryZone();

        }
    }

    void UpdateMoneyUI()
    {
        moneyAmountText.text = "$ " + money.ToString("F2");
    }

    public void ResetDelivery()
    {
        pizzaDelivered = false;
    }

    public void AssignPizza(GameObject newPizza)
    {
        currentPizza = newPizza;
        hasPizza = false;
        pizzaDelivered = false;
    }
}

using UnityEngine;
using TMPro;


public class PizzaManager : MonoBehaviour
{
    private bool hasPizza = false;
    private bool pizzaDelivered = false;
    private GameObject currentPizza;

    private float money = 100f;

    [Header("References")]
    public Transform pizzaHoldPoint;
    public DeliverySpawner deliverySpawner;
    public PizzaSpawner spawner;
    public TMP_Text moneyAmountText;

    void OnTriggerEnter(Collider other)
    {
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

    public float GetCurrentMoney()
    {
        return money;
    }

    public void RemoveMoney(float amount)
    {
        money -= amount;
        if (money < 0f) money = 0f;
        UpdateMoneyUI();
    }

    public void PickedUpPizza(GameObject pizza)
    {
        if (pizza == currentPizza && !hasPizza && !pizzaDelivered)
        {
            hasPizza = true;
            pizza.transform.SetParent(pizzaHoldPoint);
            pizza.transform.localPosition = Vector3.zero;
            pizza.transform.localRotation = Quaternion.identity;
            Debug.Log("Pizza picked up!");
        }
    }

    public bool HasPizza()
    {
        return hasPizza;
    }
    
    public void ClearCurrentPizza()
    {
        currentPizza = null;
        hasPizza = false;
        pizzaDelivered = false;
    }

}

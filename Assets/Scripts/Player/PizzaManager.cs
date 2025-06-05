using UnityEngine;
using TMPro;


public class PizzaManager : MonoBehaviour
{
    private bool hasPizza = false;
    private bool pizzaDelivered = false;
    private GameObject currentPizza;

    [Header("Delivery Timer")]
    public float maxDeliveryTime = 30f; // seconds to deliver before pizza gets cold
    private float deliveryTimer = 0f;
    public TMP_Text timerText;

    private float money = 100f;

    [Header("References")]
    public Transform pizzaHoldPoint;
    public DeliverySpawner deliverySpawner;
    public PizzaSpawner spawner;
    public TMP_Text moneyAmountText;

    void Update()
    {
        if (hasPizza && deliveryTimer > 0f)
        {
            deliveryTimer -= Time.deltaTime;
            if (deliveryTimer < 0f)
                deliveryTimer = 0f;
            UpdateTimerUI();

            if (deliveryTimer <= 0f)
                HandleExpiredPizza();
        }
    }

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

            float baseReward = Random.Range(1.00f, 5.00f);
            float timeFactor = Mathf.Clamp01(deliveryTimer / maxDeliveryTime);
            float reward = baseReward * timeFactor;
            reward = Mathf.Round(reward * 100f) / 100f;
            money += reward;

            Debug.Log($"You earned ${reward}!");
            UpdateMoneyUI();

            deliveryTimer = 0f;
            UpdateTimerUI();

            spawner.Invoke("RespawnPizza", 2.5f);
            deliverySpawner.SpawnNewDeliveryZone();
        }
    }


    void UpdateMoneyUI()
    {
        moneyAmountText.text = "$ " + money.ToString("F2");
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(deliveryTimer).ToString();
    }

    void HandleExpiredPizza()
    {
        if (currentPizza != null)
            Destroy(currentPizza);

        hasPizza = false;
        pizzaDelivered = false;

        Debug.Log("Pizza got cold and was discarded!");

        spawner.Invoke("RespawnPizza", 2.5f);
        deliverySpawner.SpawnNewDeliveryZone();
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

    public void SetMoney(float amount)
    {
        money = amount;
        UpdateMoneyUI();
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
            deliveryTimer = maxDeliveryTime;
            UpdateTimerUI();
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
        deliveryTimer = 0f;
        UpdateTimerUI();
    }

}

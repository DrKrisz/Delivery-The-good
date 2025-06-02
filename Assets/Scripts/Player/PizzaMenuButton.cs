using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PizzaMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    public Sprite normalSprite;   // pizza1
    public Sprite hoverSprite;    // pizza2

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.sprite = normalSprite;
    }
}

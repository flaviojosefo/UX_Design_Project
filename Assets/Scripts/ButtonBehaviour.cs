using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public RectTransform cost;

    public int moneyCost;

    void Update() {

        if (cost.gameObject.activeSelf) {

            Vector2 mousePosition = Input.mousePosition;

            cost.position = mousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {

        if (!cost.gameObject.activeSelf) {

            cost.GetChild(0).GetComponent<TMP_Text>().text = moneyCost.ToString() + " €";
            cost.gameObject.SetActive(true);
        }  
    }

    public void OnPointerExit(PointerEventData eventData) {

        if (cost.gameObject.activeSelf) cost.gameObject.SetActive(false);
    }
}

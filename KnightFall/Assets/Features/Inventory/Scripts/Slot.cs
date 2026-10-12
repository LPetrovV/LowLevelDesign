using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Item currentItem;

    public void OnPointerClick(PointerEventData eventData)
    {

        //invoke UseItemController to show wrapper and update attributes
        GameObject controllerObj = GameObject.Find("GameController"); 
        UseItemController itemController = controllerObj.GetComponent<UseItemController>();
        if (itemController != null)
        {
            
            itemController.ShowItemInfo(currentItem);
        }
        else
        {
            Debug.LogError("UseItemController not found on GameController.");
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // save OG parent
        transform.SetParent(transform.root); // above other canvasses
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semitransparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // when end drag
        // set draggable = true, opaque
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        //drop slot = if players mouse entered a slot
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();

        // if drop slot is real 
        if (dropSlot != null)
        {
            // if drop slot has item, switch items
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                // else set original slot current item to null
                originalSlot.currentItem = null;
            }

            // move item into drop slot 
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // if drop slot does not exist, return to original slot
            transform.SetParent(originalParent);

        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;


    }


}

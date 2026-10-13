using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UseItemController : MonoBehaviour
{
    //set wrapper and variables to update
    public InventoryController inventoryController;
    public GameObject itemInfoWrapper;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemBuffText;
    public Image itemImage;

    private Item itemSelected;

    // on slot clicked, make wrapper visible and change attributes
    public void ShowItemInfo(Item item)
    {
        itemSelected = item;
        itemInfoWrapper.SetActive(true);

        itemNameText.text = item.getName();
        itemBuffText.text = "Buff = + "+ item.getBuff();


        // Get the Sprite from the target GameObject's SpriteRenderer
        Image sourceImage = item.GetComponent<Image>();

        if (sourceImage != null && itemImage != null)
        {
            // Assign the sprite to the UI Image
            itemImage.sprite = sourceImage.sprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer or itemImage is null.");
        }
    }

    public void UseSelectedItem()
    {
        if (itemSelected != null)
        {
            inventoryController.useItem(itemSelected);
            Debug.Log("Used item: " + itemSelected.getName());
            itemInfoWrapper.SetActive(false);
        }
    }
}

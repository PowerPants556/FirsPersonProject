using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public string parentName;
    public ItemData itemData;

    private void Start()
    {
        parentName = transform.parent.name;
    }

    private void AddToListOnDrag(List<GameObject>slots, List<ItemData>items, Transform parent)
    {
        if (itemData == null) return;

        if(itemData.isUniq || slots.Count == 0)
        {
            slots.Add(gameObject);
            items.Add(itemData);
            transform.SetParent(parent);
            parentName = transform.parent.name;
        }
        else if (!itemData.isUniq)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].GetComponent<Slot>().itemData.id == itemData.id)
                {
                    items[i].count += itemData.count;
                    slots[i].transform.Find("ItemCount").GetComponent<Text>().text =
                        slots[i].GetComponent<Slot>().itemData.count.ToString();
                    Destroy(gameObject);
                    break;
                }
                else if (i == slots.Count - 1)
                {
                    slots.Add(gameObject);
                    items.Add(itemData);
                    transform.SetParent(parent);
                    parentName = transform.parent.name;
                    break;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        InvMenager.instance.GetDescriptionPanel().SetActive(false);
        transform.SetParent(InvMenager.instance.GetTemptParentForSlots());
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var inventoryManager = InvMenager.instance;
        var pController = PlayerCon.instance;
        float distanceToInvContent = Vector3.Distance(transform.position,
            inventoryManager.GetInventoryContent().transform.position);
        float distanceToChestContent = Vector3.Distance(transform.position,
            inventoryManager.GetChestContent().transform.position);

        if (distanceToInvContent < distanceToChestContent)
        {
            if (parentName == "InvContent")
            {
                transform.SetParent(inventoryManager.GetInventoryContent().transform);
            }
            else
            {
                inventoryManager.currentChestSlots.Remove(gameObject);
                pController.currentChestItems.Remove(itemData);
                AddToListOnDrag(inventoryManager.invSlots,
                    pController.inventoryItems, inventoryManager.GetInventoryContent().transform);
            }
        }
        else
        {
            if (parentName == "ChestContent")
            {
                transform.SetParent(inventoryManager.GetChestContent().transform);
            }
            else
            {
                inventoryManager.invSlots.Remove(gameObject);
                pController.inventoryItems.Remove(itemData);
                AddToListOnDrag(inventoryManager.currentChestSlots,
                    pController.currentChestItems, inventoryManager.GetChestContent().transform);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var iManager = InvMenager.instance;
        iManager.GetDescriptionPanel().SetActive(true);
        if(itemData != null)
        {
            iManager.GetDescriptionPanelText().text = itemData.description;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InvMenager.instance.GetDescriptionPanel().SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvMenager : MonoBehaviour
{
    [SerializeField] private Text descriptionPanelText;
    [SerializeField] private Transform tempParentForSlots;
    [SerializeField] private GameObject slotPref, invPanel, chestPanel, invContent, chestContent, descriptionPanel;
    public ItemData[] items;
    public List<GameObject> invSlots = new List<GameObject>();
    public List<GameObject> currentChestSlots = new List<GameObject>();

    public static InvMenager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        invPanel.SetActive(false);
        chestPanel.SetActive(false);
        descriptionPanel.SetActive(false);
    }

    public Transform GetTemptParentForSlots()
    {
        return tempParentForSlots;
    }
    public Text GetDescriptionPanelText()
    {
        return descriptionPanelText;
    }
    public GameObject GetInventoryPanel()
    {
        return invPanel;
    }
    public GameObject GetChestPanel()
    {
        return chestPanel;
    }
    public GameObject GetInventoryContent()
    {
        return invContent;
    }
    public GameObject GetChestContent()
    {
        return chestContent;
    }
    public GameObject GetDescriptionPanel()
    {
        return descriptionPanel;
    }
    public void CreateItem(int itemId, List<ItemData> items)
    {
        ItemData item = new ItemData(this.items[itemId].name,
                                     this.items[itemId].description,
                                     this.items[itemId].id,
                                     this.items[itemId].count, 
                                     this.items[itemId].isUniq);
        if (!item.isUniq && items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (item.id == items[i].id)
                {
                    items[i].count += 1;
                    break;
                }
                else if (i == items.Count - 1)
                {
                    items.Add(item);
                    break;
                }
            }
        }
        else if (item.isUniq || (!item.isUniq && items.Count == 0))
        {
            items.Add(item);
        }
    }

    public void InstantiatingItem(ItemData item, Transform parent, List<GameObject> items)
    {
        GameObject currentItem = Instantiate(slotPref);
        currentItem.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 1);
        currentItem.transform.SetParent(parent);
        currentItem.AddComponent<Slot>();
        currentItem.GetComponent<Slot>().itemData = item;
        currentItem.transform.Find("ItemName").GetComponent<Text>().text = item.name;
        currentItem.transform.Find("ItemImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(item.name);
        currentItem.transform.Find("ItemCount").GetComponent<Text>().text = item.count.ToString();
        currentItem.transform.Find("ItemCount").GetComponent<Text>().color = item.isUniq ? Color.clear : new Color(1f, 1f, 1f, 1);
        items.Add(currentItem);
    }
}

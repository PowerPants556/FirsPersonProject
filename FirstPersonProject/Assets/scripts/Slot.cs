using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHendler, IPointerUpHandler
{
    [SerializeField]
    private Transform tempParentForSlots;
    [SerializeField]
    private PlayerController pController;
    public string parentName;
    public ItemData itemData;

    public coid OnDrag(PointEventData eventData)
    {

    }

    public coid OnEndDrag(PointEventData eventData)
    {

    }

    public coid OnPointerDown(PointEventData eventData)
    {

    }

    public coid OnPointerUp(PointEventData eventData)
    {

    }
}

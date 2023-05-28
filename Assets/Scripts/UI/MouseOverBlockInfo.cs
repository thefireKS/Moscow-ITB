using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverBlockInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject blockOutline;
    [SerializeField] private GameObject blockDescription;

    public void OnPointerEnter(PointerEventData eventData)
    {
        blockOutline.SetActive(true);
        blockDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        blockOutline.SetActive(false);
        blockDescription.SetActive(false);
    }
}

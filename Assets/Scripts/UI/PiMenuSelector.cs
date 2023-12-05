using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PiMenuSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}

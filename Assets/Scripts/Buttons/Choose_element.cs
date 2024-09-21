using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Choose_element : MonoBehaviour, IPointerClickHandler
{
    int number;
    int type;
    Choose parent;
    string name;
    public void OnPointerClick(PointerEventData eventData)
    {
        parent.choose_action(number, type, name);
    }

    public void set_param(int number, string name, Choose parent, int type, Color color)
    {
        this.number = number;
        GetComponent<Text>().text = name;
        GetComponent<Text>().color = color;
        this.parent = parent;
        this.type = type;
        this.name = name;
    }
}

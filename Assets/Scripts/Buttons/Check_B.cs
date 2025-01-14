using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Check_B : MonoBehaviour, IPointerClickHandler
{
    bool check;
    public async void OnPointerClick(PointerEventData eventData)
    {
        if (!check)
        {
            if (await Controller.instance.push_check())
            {
                check = true;
                transform.GetChild(0).GetComponent<Text>().text = "Следующий";
            }
        }
        else if (check)
        {
            Controller.instance.push_next();
            reset();
        }
    }
    public void reset()
    {
        check = false;
        transform.GetChild(0).GetComponent<Text>().text = "Ответить";
    }
}

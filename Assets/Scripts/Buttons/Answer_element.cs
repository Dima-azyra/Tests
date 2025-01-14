using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Answer_element : MonoBehaviour, IPointerClickHandler
{
    string answer;
    Answers parent;
    [SerializeField] Transform child;
    bool set;
    bool check;

    Color white = new Color(0.88f, 0.88f, 0.88f, 1);
    Color grey = new Color(0.5f, 0.5f, 0.5f, 1);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!set && !check && !parent.not_choose())
        {
            set_color(grey);
            set = true;
        }
        else if (!check && !parent.not_choose())
        {
            set_color(white);
            set = false;
        }
    }

    public void set_param(string answer, Answers parent)
    {
        set = false;
        check = false;
        this.answer = answer;
        this.parent = parent;
        transform.SetAsLastSibling();
        set_color(white);
        child.GetComponent<Text>().text = answer;
    }

    public void set_color(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public string check_answer()
    {
        return answer;
    }

    public bool check_set()
    {
        return set;
    }
}

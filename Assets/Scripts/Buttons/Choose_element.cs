using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Choose_element : MonoBehaviour, IPointerClickHandler
{

    int number;
    int type;
    Choose parent;
    string text;
    Text text_obj;
    Color blue = new Color(0.16f, 0.58f, 0.9f, 1);
    Dictionary<string, Dictionary<int, string[]>> answer_book;
    string name_topic;
    object special;
    bool start;

    private void OnEnable()
    {
        StartCoroutine(update_ui());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parent.choose_action(number, type, text);
    }

    public void set_param(int number, string text, Choose parent, int type, Dictionary<string, Dictionary<int, string[]>> answer_book, string name_topic, object special)
    {
        this.number = number;
        text_obj = GetComponent<Text>();
        this.parent = parent;
        this.type = type;
        this.text = text;
        this.answer_book = answer_book;
        this.name_topic = name_topic;
        this.special = special;
        transform.SetAsLastSibling();
        start = true;
    }

    public async Task write(int _show)
    {
        text_obj.text = "";
        if (type == 3)
        {
            string[] question = (string[])special;
            Color color = await parent.get_number_color(number);
            text = question[0].Insert(question[0].IndexOf("а)") - 1, "\n");
            var answers = new List<string>(answer_book[name_topic][number]);
            string[] t = text.Split("\r\n");
            for (int i = 0; t.Length > i; i++)
            {
                if (i == 0) text_obj.text += "<b><color=#" + color.ToHexString() + ">" + t[i] + "</color></b>";
                else if (i < t.Length - 1)
                {
                    string answer = t[i].Split(")")[0].Trim();
                    if (answers[0].Contains(answer) && _show == 1) text_obj.text += "\r\n" + "<color=#" + blue.ToHexString() + ">" + t[i] + "</color>";
                    else text_obj.text += "\r\n" + t[i];
                }
            }
        }
        if (type == 2)
        {
            text_obj.text = text;
            GetComponent<Text>().color = await parent.get_topic_color(text, (Dictionary<int, string[]>)special);
        }        
        if (type == 1)
        {
            text_obj.text = text;
            GetComponent<Text>().color = await parent.get_book_color(text, number);
            if (text.Equals("Книга 1")) text_obj.text = "Стоматология ч.1";
            else if (text.Equals("Книга 2")) text_obj.text = "Стоматология ч.2";
            else if (text.Equals("Книга 3")) text_obj.text = "Эпидемиология";
        }
    }

    IEnumerator update_ui()
    {
        if (start)
        {
            start = false;
            write(Show_answers.instance._show);
        }
        yield return new WaitForSecondsRealtime(0.01f);
        repeat();
    }

    void repeat()
    {
       StartCoroutine(update_ui());
    }
}

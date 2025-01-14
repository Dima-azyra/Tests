using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using static SaveSystem;
public class Show_answers : MonoBehaviour, IPointerClickHandler
{
    public static Show_answers instance;
    public int _show;
    bool set;
    bool check;
    string show = "?";
    string hide = "?";

    [SerializeField] Transform content;

    Color blue = new Color(0.16f, 0.58f, 0.9f, 1);
    Color yellow = new Color(0.9f, 0.8f, 0.25f, 1);

    private async void Awake()
    {
        instance = this;
        string s = await get_value("Show_answers", "Show");
        if (s != null)
        {
            _show = int.Parse(s);
            if (_show == 1) set_show();
            else set_hide();
        }
        else set_hide();
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!set) set_hide();
        else if (!check) set_show();

        foreach (Transform child in content)
        {
            child.GetComponent<Choose_element>().write(_show);
        }
    }

    void set_show()
    {
        _show = 1;
        set_color(blue);
        set = false;
        transform.GetChild(0).GetComponent<TMP_Text>().text = show;
        set_save("Show_answers", "Show", _show + "");
    }
    void set_hide()
    {
        _show = 0;
        set_color(yellow);
        set = true;
        transform.GetChild(0).GetComponent<TMP_Text>().text = hide;
        set_save("Show_answers", "Show", _show + "");
    }

    public void set_color(Color color)
    {
        GetComponent<Image>().color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField] Text Book;
    [SerializeField] Text Part;
    [SerializeField] Text Number;
    [SerializeField] Text Discription;
    [SerializeField] InputField Look;
    [SerializeField] TMP_Text count_text;

    [Space(20)]

    [SerializeField] GameObject Answer;
    [SerializeField] GameObject Check_B;
    [SerializeField] GameObject Look_B;
    [SerializeField] GameObject Choose;
    [SerializeField] Show_answers_in_main show_answers;

    public static Controller instance;
    string current_input = "";
    Color blue = new Color(0.16f, 0.58f, 0.9f, 1);
    List<string> answers;
    string question;
    private void Awake()
    {
        SaveSystem.start_base();
        count_text.text = "";
        instance = this;
        Choose.GetComponent<Choose>().load();
        Choose.GetComponent<Choose>().hide();
    }
    void OnEnable()
    {
        Look.onEndEdit.AddListener(delegate { inputEndEdit(); });
    }
    void OnDisable()
    {
        Look.onEndEdit.RemoveAllListeners();
    }

    public void put_book_text(string text)
    {
        if (text.Equals("Книга 1")) Book.text = "Стоматология ч.1";
        else if (text.Equals("Книга 2")) Book.text = "Стоматология ч.2";
        else if (text.Equals("Книга 3")) Book.text = "Эпидемиология";  
    }
    public void put_part_text(string text)
    {
        Part.text = text;
    }
    public void put_number_text(string text)
    {
        Number.text = text;
    }
    public void put_discription_text(string text, List<string> answers)
    {
        this.answers = answers;
        question = text;
        write(show_answers._show);
    }

    public void put_answer(string name_topic, int number_q, string[] question, List<string> answers)
    {
        Answer.GetComponent<Answers>().put_answers(name_topic, number_q, question, answers);
    }

     public async Task<bool> push_check()
    {
        return await Answer.GetComponent<Answers>().check();
    }

    public void push_next()
    {
        Choose.GetComponent<Choose>().next();
    }
    
    public void push_prev()
    {
        Choose.GetComponent<Choose>().prev();
    }

    public void open_choose(int type)
    {
        Choose.GetComponent<Take_Button_interface>().open();
        Choose.GetComponent<Choose>().start_choose(type);
    }

    public string get_book_name()
    {
         return Choose.GetComponent<Choose>().get_current_book_name();
    }

    public void set_color_book(Color color)
    {
        Book.color = color;
    }

    public void set_color_part(Color color)
    {
        Part.color = color;
    }

    public void set_color_number(Color color)
    {
        Number.color = color;
    }
    public void check_color()
    {
        Choose.GetComponent<Choose>().check_current_color();
    }

    public void look_for(bool next)
    {
        Choose.GetComponent<Choose>().look_for(Look.text, next, count_text);
    }

    public void reset_button()
    {
        Check_B.GetComponent<Check_B>().reset();
    }

    private void inputEndEdit()
    {
        if(!current_input.Equals(Look.text))
        {
            current_input = Look.text;
            Choose.GetComponent<Choose>().look_for(Look.text, true, count_text);
        }
    }

    public void write(int _show)
    {
        Discription.text = "";
        string[] t = question.Split("\r\n");
        for (int i = 0; t.Length > i; i++)
        {
            if (i == 0) Discription.text += "<b>" + t[i] + "</b>";
            else if (i < t.Length - 1)
            {
                string answer = t[i].Split(")")[0].Trim();
                if (answers[0].Contains(answer) && _show == 1) Discription.text += "\r\n" + "<color=#" + blue.ToHexString() + ">" + t[i] + "</color>";
                else Discription.text += "\r\n" + t[i];
            }
        }
    }
}

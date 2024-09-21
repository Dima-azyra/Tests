using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Answer : MonoBehaviour
{
    [SerializeField] GameObject answer_element;

    Color red = new Color(0.8f, 0.1f, 0, 1);
    Color green = new Color(0.1f, 0.6f, 0, 1);
    Color green_mistake = new Color(0.2f, 1, 0.2f, 1);

    string name_topic;
    int number_q;
    List<string> answers;

    bool start;
    bool stop;

    public void put_answers(string name_topic,int number_q, string[] question, List<string> answers)
    {
        delete_answers();
        this.name_topic = name_topic;
        this.number_q = number_q;
        this.answers = answers;

        int count = 0;
        foreach (string answer in question)
        {
            if (count != 0)
            {
                GameObject answer_element = Instantiate(this.answer_element, transform.GetChild(0).GetChild(0));
                answer_element.GetComponent<Answer_element>().set_param(answer,this);
            }
            count++;
        }
    }

    void delete_answers()
    {
        foreach (Transform child in transform.GetChild(0).GetChild(0))
        {
            start = false;
            stop = false;
            Destroy(child.gameObject);
        }
    }

    public bool check()
    {
        foreach (Transform child in transform.GetChild(0).GetChild(0))
        {
            var answer_element = child.GetComponent<Answer_element>();
            if (answer_element.check_set())
            {
                start = true;
                break;
            }           
        }
        if (start && !stop)
        {
            int good = 1;
            foreach (Transform child in transform.GetChild(0).GetChild(0))
            {
                var answer_element = child.GetComponent<Answer_element>();

                if (answers[0].Split('.').Contains(answer_element.check_answer()) && answer_element.check_set())
                {
                    answer_element.set_color(green);
                }
                else if (answers[0].Split('.').Contains(answer_element.check_answer()))
                {
                    answer_element.set_color(green_mistake);
                    good = 0;
                }
                else if (answer_element.check_set())
                {
                    answer_element.set_color(red);
                    good = 0;
                }
                stop = true;

                SaveSystem.set_save(get_save_name(), number_q + "", good + "");
                SaveSystem.save_to_file(get_save_name());
                Controller.main.check_color();
            }
            return true;
        }
        return false;
    }

    public bool not_choose()
    {
        return stop;
    }

    string get_save_name()
    {
        return Controller.main.get_book_name() + "_" + name_topic;
    }
}

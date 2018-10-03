using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour {

    public int[] questionsAnswers = new int[33];

    public InputField inputField;
    public GameObject nextPersonalButton;
    public GameObject nextExtraButton;
    public GameObject quitExtraButton;

    public GameObject gameExperienceQuestionairePanel;
    public GameObject personalQuestionsPanel;
    public GameObject extraQuestionsPanel;

    public Text ageText;
    public int[] personalQuestionsAnswers = new int[3];

    public InputField extraQuestion1text;
    public InputField extraQuestion2text;

    public void SetQuestion(string question_value)
    {
        string[] data = question_value.Split('_');
        int question = System.Convert.ToInt32(data[0]);
        int value = System.Convert.ToInt32(data[1]);
        if (questionsAnswers[question] == 0)
        {
            questionsAnswers[question] = value;
        }
        else
        {
            questionsAnswers[question] = 0;
        }
    }
    public void SetPersonalQuestion(string question_value)
    {
        string[] data = question_value.Split('_');
        int question = System.Convert.ToInt32(data[0]);
        int value = System.Convert.ToInt32(data[1]);
        if (personalQuestionsAnswers[question] == 0)
        {
            personalQuestionsAnswers[question] = value;
        }
        else
        {
            personalQuestionsAnswers[question] = 0;
        }
    }

    public void ShowGameExperienceQuestionsPanel()
    {
        gameExperienceQuestionairePanel.SetActive(true);
        personalQuestionsPanel.SetActive(false);
        extraQuestionsPanel.SetActive(false);
    }

    public void ShowExtraQuestionsPanel()
    {
        gameExperienceQuestionairePanel.SetActive(false);
        personalQuestionsPanel.SetActive(false);
        extraQuestionsPanel.SetActive(true);
    }

    public void SendGameExperienceQuestionaireData()
    {
        StartCoroutine(SendGameExperienceQuestionaireEnumerator());
    }

    public IEnumerator SendGameExperienceQuestionaireEnumerator()
    {
        yield return StartCoroutine(this.GetComponent<MySQLManager>().RecordData(questionsAnswers));
        ShowExtraQuestionsPanel();
    }

    public void SendPersonalExperienceQuestionaireData()
    {
        StartCoroutine(SendPersonalExperienceQuestionaireDataEnumerator());
    }

    public IEnumerator SendPersonalExperienceQuestionaireDataEnumerator()
    {
        string answers = "Age:" + ageText.text + "_";
        for (int i = 0; i < personalQuestionsAnswers.Length; i++)
        {
            answers += "Q" + (i + 1).ToString() + ":" + personalQuestionsAnswers[i].ToString() + "_";
        }

        yield return StartCoroutine(this.GetComponent<MySQLManager>().RecordPersonalData(answers));
        ShowGameExperienceQuestionsPanel();
    }

    public void SendExtraQuestionaireData()
    {
        StartCoroutine(SendExtraQuestionaireDataEnumerator());
    }

    public IEnumerator SendExtraQuestionaireDataEnumerator()
    {
        string answers = "Q1: " + extraQuestion1text.text + "_____XXXXX_____" + "Q2: " + extraQuestion2text.text;
        
        yield return StartCoroutine(this.GetComponent<MySQLManager>().RecordPersonalData(answers));
        this.GetComponent<MySQLManager>().QuitGame();
    }

    public void Update()
    {
        if (gameExperienceQuestionairePanel.activeSelf)
        {
            bool allAnswered = true;
            for (int i = 0; i < questionsAnswers.Length; i++)
            {
                if (questionsAnswers[i] == 0)
                {
                    allAnswered = false;
                }
            }

            if (allAnswered)
            {
                nextPersonalButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                nextPersonalButton.GetComponent<Button>().interactable = false;
            }
        }

        if (personalQuestionsPanel.activeSelf)
        {
            bool allAnswered = true;
            for (int i = 0; i < personalQuestionsAnswers.Length; i++)
            {
                if (personalQuestionsAnswers[i] == 0)
                {
                    allAnswered = false;
                }
            }

            if (allAnswered && ageText.text != "")
            {
                nextExtraButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                nextExtraButton.GetComponent<Button>().interactable = false;
            }
        }

        if (extraQuestionsPanel.activeSelf)
        {

            if (extraQuestion1text.text != "" && extraQuestion2text.text != "")
            {
                quitExtraButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                quitExtraButton.GetComponent<Button>().interactable = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AnswerScript : MonoBehaviour
{
    public QuizManager quizManager;

    void Awake()
    {
        quizManager.LoadPoints();
        quizManager.PointsTxt.SetText(quizManager.points.ToString());
        quizManager.PointsTxt2.SetText(quizManager.points.ToString());
        quizManager.currentQuestion = 0;
    }

    void Start()
    {
        string quiz = PlayerPrefs.GetString("QuizData");
        string att = PlayerPrefs.GetString("AtencaoData");
        quizManager.readScript("Script/"+quiz);
        quizManager.readText("Script/"+att);
        quizManager.atencao.SetActive(false);

    }

    void Update()
    {
        quizManager.PointsTxt.SetText(quizManager.points.ToString());
        quizManager.PointsTxt2.SetText(quizManager.points.ToString());
    }

    public bool isCorrect = false;

    public void Answer(int optionIndex)
    {
        // Verifique se a opção é correta
        if (quizManager.options[optionIndex].GetComponent<AnswerScript>().isCorrect)
        {
            quizManager.simImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "Certo");
            quizManager.naoImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "Certo");
            quizManager.points++;

            // Chama a corrotina para aguardar 3 segundos antes de chamar a função correct
            StartCoroutine(WaitAndCallCorrect());
        }
        else
        {
            quizManager.simImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "Errado");
            quizManager.naoImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "Errado");

            // Chama a corrotina para aguardar 3 segundos antes de chamar a função correct
            StartCoroutine(WaitAndCallCorrect());
        }
    }

    // Corrotina para aguardar 3 segundos antes de chamar a função correct
    IEnumerator WaitAndCallCorrect()
    {
        yield return new WaitForSeconds(2f);
        quizManager.correct();
    }

    // Suponha que você tem um botão chamado "BotaoOpcao1" que corresponde à primeira opção
    public void BotaoOpcao1Clicado()
    {
        quizManager.simImage.SetActive(true);
        quizManager.backButton.SetActive(false);
        quizManager.options[1].SetActive(false);
        Answer(0); // Passa o índice 0 para representar a primeira opção
    }

    // Suponha que você tem um botão chamado "BotaoOpcao2" que corresponde à segunda opção
    public void BotaoOpcao2Clicado()
    {
        quizManager.naoImage.SetActive(true);
        quizManager.backButton.SetActive(false);
        quizManager.options[0].SetActive(false);
        Answer(1); // Passa o índice 1 para representar a segunda opção
    }
}

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
        quizManager.currentQuestion = 0;
    }

    void Start()
    {
        int numeroAleatorio = PlayerPrefs.GetInt("numeroAleatorio", 1);
        string quiz =  "Quiz/Quiz"+numeroAleatorio;
        string att = "Atencao/atencao"+numeroAleatorio;
        quizManager.readScript("Script/"+quiz);
        quizManager.AtencaoTxt.text = quizManager.readText("Script/"+att);
        quizManager.AtencaoTitulo.text = quizManager.readText("Script/"+att+"T");
        quizManager.conclusaoSource = "Conclusao/CONCLUSAO"+numeroAleatorio;
        quizManager.questoesSource = "Questoes/Historia"+numeroAleatorio;
        quizManager.feedbackSource = "Script/Feedback/Feedback"+numeroAleatorio;
        quizManager.audioFSource = "Audio/Feedback/Feedback"+numeroAleatorio;
        quizManager.atencaoSource = "Script/"+att;
        quizManager.atencao.SetActive(false);
    }

    void Update()
    {

    }

    public bool isCorrect = false;

    public void Answer(int optionIndex)
    {
        // Verifique se a opção é correta
        if (quizManager.options[optionIndex].GetComponent<AnswerScript>().isCorrect)
        {
            quizManager.index = optionIndex + 1;  // Isso faz com que o index varie entre 1 e 2
            quizManager.FalasTxt.text = quizManager.readText(quizManager.feedbackSource + "/F" + quizManager.currentQuestion + "." + quizManager.index);

            quizManager.simImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "joinha");
            quizManager.naoImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "joinha");
            quizManager.points++;

            quizManager.Correct();
        }
        else
        {
            quizManager.index = (optionIndex == 0) ? 1 : 2;  // Isso inverte o valor de index entre 1 e 2
            quizManager.FalasTxt.text = quizManager.readText(quizManager.feedbackSource + "/F" + quizManager.currentQuestion + "." + quizManager.index);

            quizManager.simImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "erro");
            quizManager.naoImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/" + "erro");

            quizManager.Correct();
        }
    }

    // Suponha que você tem um botão chamado "BotaoOpcao1" que corresponde à primeira opção
    public void BotaoOpcao1Clicado()
    {
        quizManager.simImage.SetActive(true);
        quizManager.uiToRemove.SetActive(false);
        quizManager.falso.SetActive(false);
        quizManager.verdadeiro.SetActive(false);
        quizManager.continuar.SetActive(true);
        quizManager.falas.SetActive(true);
        Answer(0); // Passa o índice 0 para representar a primeira opção
    }

    // Suponha que você tem um botão chamado "BotaoOpcao2" que corresponde à segunda opção
    public void BotaoOpcao2Clicado()
    {
        quizManager.naoImage.SetActive(true);
        quizManager.uiToRemove.SetActive(false);
        quizManager.falso.SetActive(false);
        quizManager.verdadeiro.SetActive(false);
        quizManager.continuar.SetActive(true);
        quizManager.falas.SetActive(true);
        Answer(1); // Passa o índice 1 para representar a segunda opção
    }
}

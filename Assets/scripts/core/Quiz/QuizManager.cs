using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public static List<QuestionsAndAnswers> QnA = new List<QuestionsAndAnswers>();
    public GameObject[] options;
    public int currentQuestion;
    public bool selector;
    public int points;

    public TextMeshProUGUI AtencaoTxt;
    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI PointsTxt;
    public TextMeshProUGUI PointsTxt2;

    public GameObject simImage;
    public GameObject naoImage;
    public GameObject backButton;
    public GameObject atencao;

    private AudioSource audioSource; // Referência ao componente AudioSource

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Cria o componente AudioSource
        generateQuestion();
    }

    public void PlayAudio(string audioFilePath, System.Action onComplete = null)
    {
        // Carrega o arquivo de áudio
        AudioClip audioClip = Resources.Load<AudioClip>(audioFilePath);

        // Verifica se o arquivo de áudio foi carregado com sucesso
        if (audioClip != null)
        {
            // Define a clip do AudioSource como o áudio carregado
            audioSource.clip = audioClip;

            // Reproduz o áudio
            audioSource.Play();

            // Invoca a ação onComplete após a reprodução do áudio terminar
            if (onComplete != null)
            {
                StartCoroutine(WaitForAudio(audioClip.length, onComplete));
            }
        }
        else
        {
            Debug.LogError("Falha ao carregar o arquivo de áudio em: " + audioFilePath);
        }
    }

    private IEnumerator WaitForAudio(float duration, System.Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    public void playButtonAudio()
    {
        // Reproduz o áudio ao clicar no botão
        PlayAudio("Audio/button", () =>
        {
            // Funções extras
        });
    }

    public void correct()
    {
        options[0].SetActive(true);
        options[1].SetActive(true);
        naoImage.SetActive(false);
        simImage.SetActive(false);
        backButton.SetActive(true);
        //QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    void SetAnswers()
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            //options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];

            if(QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if(currentQuestion<QnA.Count)
        {
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
            currentQuestion++;
        }
        else
        {
            Debug.Log("Acabaram as perguntas...");
            atencao.SetActive(true);
        }
        
    }

    void SavePoints()
    {
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.Save();
    }

    public void LoadPoints()
    {
        // Carregar o valor de 'points' do PlayerPrefs
        points = PlayerPrefs.GetInt("Points", 0);
        // Se o valor não existir, será usado o valor padrão (0 no caso)
    }
    public void voltarMenu()
    {
        PlayAudio("Audio/button", () =>
        {
            SavePoints();
            SceneManager.LoadScene(1);
        });
    }

    #region LoadingScript

    public void readText(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        if (textAsset != null)
        {
            AtencaoTxt.text = textAsset.text;
        }
        else
        {
            Debug.LogError($"Arquivo '{fileName}' não encontrado na pasta Resources.");
        }
    }

    public void readScript(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile != null)
        {
            QnA = ReadQuestionsFromJson(jsonFile.text);
            Debug.Log($"Loaded {QnA.Count} questions from {fileName}");
        }
        else
        {
            Debug.Log($"Arquivo JSON '{fileName}' não encontrado na pasta Resources.");
        }
    }

    List<QuestionsAndAnswers> ReadQuestionsFromJson(string jsonContent)
    {
        // Criar um wrapper para a lista
        string wrappedJson = $"{{\"questionsList\":{jsonContent}}}";

        // Desserializar diretamente a lista
        QuestionsListWrapper wrapper = JsonUtility.FromJson<QuestionsListWrapper>(wrappedJson);

        if (wrapper != null)
        {
            return wrapper.questionsList;
        }
        else
        {
            Debug.LogError("Erro ao desserializar o JSON. Verifique a estrutura do JSON.");
            return new List<QuestionsAndAnswers>();
        }
    }

    [System.Serializable]
    private class QuestionsListWrapper
    {
        public List<QuestionsAndAnswers> questionsList;
    }

        #endregion
    }

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
    public TextMeshProUGUI AtencaoTitulo;
    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI FalasTxt;

    public GameObject simImage;
    public GameObject naoImage;
    public GameObject verdadeiro;
    public GameObject falso;
    public GameObject uiToRemove;
    public GameObject continuar;
    public GameObject falas;
    public GameObject atencao;
    public GameObject bingo;
    public GameObject jogar;
    public GameObject estrela1;
    public GameObject estrela2;
    public GameObject estrela3;
    

    private AudioSource audioSource; // Referência ao componente AudioSource
    public string conclusaoSource;
    public string questoesSource;
    public string feedbackSource;
    public string audioFSource;
    public string atencaoSource;
    public int index;

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

    public void Correct()
    {
        PlayAudio(audioFSource+"/F"+currentQuestion+"."+index, () =>
        {
        });
    }

    public void Continuar()
    {
        PlayAudio("Audio/button", () =>
        {
            continuar.SetActive(false);
            falso.SetActive(true);
            verdadeiro.SetActive(true);
            falas.SetActive(false);
            naoImage.SetActive(false);
            simImage.SetActive(false);
            uiToRemove.SetActive(true);
            //QnA.RemoveAt(currentQuestion);
            generateQuestion();
        });
    }

    public void replayQuiz()
    {
        PlayAudio("Audio/button", () =>
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        });
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
        bool concluido = false;
        if(currentQuestion<QnA.Count)
        {
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
            currentQuestion++;
            PlayAudio("Audio/"+questoesSource+"/Q"+currentQuestion, () =>
            {
            });
        }
        else
        {
            Debug.Log("Acabaram as perguntas...");
            atencao.SetActive(true);
            PlayAudio("Audio/"+conclusaoSource, () =>
            {
                string filePath = atencaoSource+".1";
                string fileContent = readText(filePath);
                if (!string.IsNullOrEmpty(fileContent))
                {
                    AtencaoTxt.text = fileContent;
                    PlayAudio("Audio/"+conclusaoSource+".1", () =>
                    {
                        atencao.SetActive(false);
                        bingo.SetActive(true);
                    });
                }
                else
                {
                    atencao.SetActive(false);
                    bingo.SetActive(true);
                }

                int statusEstrela = PlayerPrefs.GetInt("statusEstrela", 0);

                if (points >= 3)
                {
                    // Incrementa a variável de status
                    statusEstrela++;
                    PlayerPrefs.SetInt("statusEstrela", statusEstrela);
                    PlayerPrefs.Save();

                    // Define a cor desejada com opacidade total (alpha = 1)
                    Color color = new Color(1f, 1f, 1f, 1f);

                    // Verifica qual estrela atualizar com base no status
                    if (statusEstrela >= 1)
                    {
                        // Atualiza as imagens dos filhos da estrela1
                        foreach (Transform child in estrela1.transform)
                        {
                            var image = child.GetComponent<Image>();
                            if (image != null)
                            {
                                image.color = color;
                            }
                        }
                    }
                    if (statusEstrela >= 2)
                    {
                        // Atualiza as imagens dos filhos da estrela2
                        foreach (Transform child in estrela2.transform)
                        {
                            var image = child.GetComponent<Image>();
                            if (image != null)
                            {
                                image.color = color;
                            }
                        }
                    }
                    if (statusEstrela >= 3)
                    {
                        // Atualiza as imagens dos filhos da estrela3
                        foreach (Transform child in estrela3.transform)
                        {
                            var image = child.GetComponent<Image>();
                            if (image != null)
                            {
                                image.color = color;
                            }
                        }
                        
                        concluido = true;

                        // Reseta o status para zero
                        statusEstrela = 0;
                        PlayerPrefs.SetInt("statusEstrela", statusEstrela);
                        PlayerPrefs.Save();
                    }
                }
                AtivaBingo(concluido);
            });
        }
    }

    public void AtivaBingo(bool concluido)
    {
        if(concluido)
            {
                PlayAudio("Audio/CARTELA_LIBERADA", () =>
                {
                    // Ativa o botão ou outro objeto
                    jogar.SetActive(true);
                });
            }
            else
            {
                PlayAudio("Audio/CARTELA_NAO_LIBERADA", () =>
                {
                });
            }
    }

    public void playBingo()
    {
        PlayAudio("Audio/button", () =>
        {
            SceneManager.LoadScene(4);
        });
    }

    public void ouvirNovamente()
    {
        PlayAudio("Audio/"+questoesSource+"/Q"+currentQuestion, () =>
        {
        });
    }
    public void voltarMenu()
    {
        PlayAudio("Audio/button", () =>
        {
            SceneManager.LoadScene(1);
        });
    }

    #region LoadingScript

    public string readText(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        if (textAsset != null)
        {
            return textAsset.text;
        }
        else
        {
            Debug.LogError($"Arquivo '{fileName}' não encontrado na pasta Resources.");
            return string.Empty;
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

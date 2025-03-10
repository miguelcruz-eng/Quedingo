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

public class MainMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Como;
    public GameObject Sobre;
    public GameObject Trophy;
    public int trofeus;
    public int timesPlayed;
    public GameObject trofeusObject;
    private AudioSource audioSource; // Referência ao componente AudioSource
    public Button jogar;
    public Button comoJogar;

    private string numberSource;

    public bool jogarSet, comoSet;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Cria o componente AudioSource
        numberSource = "Audio/BINGO/";
        LoadTrofeus();
        LoadTimesPlayed();
        if(timesPlayed == 0) 
        {
            comoSet = true;
        }else
        {
            jogarSet = true;
        }
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        SaveTrofeus();
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

    public void PlayButtonAudio()
    {
        // Reproduz o áudio ao clicar no botão
        PlayAudio("Audio/button", () =>
        {
            // funções extras
        });
    }

    public void PlayQuiz()
    {
        PlayAudio("Audio/button", () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    public void ComoJogar()
    {
        PlayAudio("Audio/button", () =>
        {
            timesPlayed ++;
            SaveTimesPlayed();
            Menu.SetActive(false);
            Como.SetActive(true);
            PlayAudio("Audio/instrucoes", () =>
            {
                jogarSet = true;
                comoSet = false;
                Menu.SetActive(true);
                Como.SetActive(false);
            });
        });
    }

    public void SobreOjogo()
    {
        PlayAudio("Audio/button", () =>
        {
            Menu.SetActive(false);
            Sobre.SetActive(true);
        });
    }

    public void MenuTrofeus()
    {
        PlayAudio("Audio/button", () =>
        {
            Menu.SetActive(false);
            Trophy.SetActive(true);

            // Atualiza a imagem do objeto cartela com base no número de estrelas
            string spritePath = GetSpritePathForStars(trofeus);
            Sprite novaImagem = Resources.Load<Sprite>(spritePath);
            Image trofeuImage = trofeusObject.GetComponent<Image>();

            if (novaImagem != null)
            {
                trofeuImage.sprite = novaImagem;
            }
            else
            {
                Debug.LogWarning($"Sprite não encontrado no caminho: {spritePath}");
            }

            // Tocar áudio correspondente
            PlayAudioForStars(trofeus);
        });
    }

    private string GetSpritePathForStars(int estrelas)
    {
        if (estrelas >= 1 && estrelas <= 3)
        {
            return $"images/BRONZE_0{estrelas}";
        }
        else if (estrelas >= 4 && estrelas <= 6)
        {
            return $"images/PRATA_0{estrelas - 3}";
        }
        else if (estrelas >= 7 && estrelas <= 8)
        {
            return $"images/OURO_0{estrelas - 6}";
        }
        else if (estrelas >= 9)
        {
            return "images/OURO_03";
        }
        else
        {
            return "images/0SEM_TROFEU";
        }
    }

    private void PlayAudioForStars(int estrelas)
    {
        if (estrelas >= 1 && estrelas <= 3)
        {
            PlayAudio(estrelas == 3 ? numberSource + "bronze" : numberSource + "sem-trofeu", null);
        }
        else if (estrelas >= 4 && estrelas <= 6)
        {
            PlayAudio(estrelas == 6 ? numberSource + "prata" : numberSource + "mais-trofeus", null);
        }
        else if (estrelas >= 7 && estrelas <= 8)
        {
            PlayAudio(numberSource + "mais-trofeus", null);
        }
        else if (estrelas >= 9)
        {
            PlayAudio(numberSource + "ouro", null);
        }
        else
        {
            PlayAudio(numberSource + "sem-trofeu", null);
        }
    }

    public void QuitGame()
    {
        PlayAudio("Audio/button", () =>
        {
            Debug.Log("Fechando Jogo");
            Application.Quit();
        });
    }

    void SaveTrofeus()
    {
        PlayerPrefs.SetInt("Trofeus", trofeus);
        PlayerPrefs.Save();
    }

    void LoadTrofeus()
    {
        trofeus = PlayerPrefs.GetInt("Trofeus", 0);
        // Se o valor não existir, será usado o valor padrão (0 no caso)
    }

    void SaveTimesPlayed()
    {
        PlayerPrefs.SetInt("TimesPlayed", timesPlayed);
        PlayerPrefs.Save();
    }

    void LoadTimesPlayed()
    {
        timesPlayed = PlayerPrefs.GetInt("TimesPlayed", 0);
        // Se o valor não existir, será usado o valor padrão (0 no caso)
    }

    private IEnumerator Blink()
    {
        while(comoSet)
        {
            SetComoVisible(false);
            yield return new WaitForSeconds(0.5f);
            SetComoVisible(true);
            yield return new WaitForSeconds(0.5f);
        }
        while(jogarSet)
        {
            SetJogarVisible(false);
            yield return new WaitForSeconds(0.5f);
            SetJogarVisible(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SetJogarVisible(bool visible)
    {
        var imagem = jogar.GetComponent<Image>();
       

        Color corAtual = imagem.color;
        corAtual.a = visible ? 0f : 0.5f;

        imagem.color = corAtual;
    }
    private void SetComoVisible(bool visible)
    {
        var imagem = comoJogar.GetComponent<Image>();
       

        Color corAtual = imagem.color;
        corAtual.a = visible ? 0f : 0.5f;

        imagem.color = corAtual;
    }
}

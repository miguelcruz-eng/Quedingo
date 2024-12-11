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
    public GameObject bronze;
    public GameObject prata;
    public GameObject ouro;
    private AudioSource audioSource; // Referência ao componente AudioSource
    public Button jogar;
    public Button comoJogar;

    public bool jogarSet, comoSet;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Cria o componente AudioSource
        LoadTrofeus();
        LoadTimesPlayed();
        if(timesPlayed == 0) 
        {
            comoSet = true;
            StartCoroutine(Blink());
        }
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
            if(trofeus>=3)
            {
                Color color = bronze.GetComponent<Image>().color;
                color.a = 255f;
                bronze.GetComponent<Image>().color = color;
                if(trofeus>=10)
                {
                    prata.GetComponent<Image>().color = color;
                    if(trofeus>=20)
                    {
                        ouro.GetComponent<Image>().color = color;
                    }
                }
            }
        });
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

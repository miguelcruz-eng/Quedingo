using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    private int cont = 1;
    private int numeroAleatorio;
    private string dialogueSource;
    private AudioSource audioSource; // Referência ao componente AudioSource
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Cria o componente AudioSource
        // Inicialize outros objetos no InputDecoder
        InputDecoder.InterfaceElements = GameObject.Find("UI_Elements");
        InputDecoder.canvas = GameObject.Find("ImageLayers");
        InputDecoder.ImageInst = Resources.Load("Prefabs/Background") as GameObject;
        InputDecoder.PI = Resources.Load("Prefabs/PersonagemI") as GameObject;
        InputDecoder.PII = Resources.Load("Prefabs/PersonagemII") as GameObject;
        InputDecoder.DialogueBox = GameObject.Find("Dialogue_box");
        InputDecoder.DialogueTextObject = GameObject.Find("Dialogue_Text");
        //InputDecoder.NamePlateTextObject = GameObject.Find("NamePlate_Text");
        //InputDecoder.CharImage = GameObject.Find("Personagem");

        InputDecoder.Quiz = GameObject.Find("Quiz");
        InputDecoder.Repetir = GameObject.Find("Repetir");

        InputDecoder.Repetir.SetActive(false);
        InputDecoder.Quiz.SetActive(false);

        InputDecoder.labels = new List<Label>();

        InputDecoder.Commands = new List<string>();
        InputDecoder.CommandLine = 0;
        InputDecoder.LastCommand = "";
    }

    public bool seta = false;
    // Start is called before the first frame update
    void Start()
    {
        numeroAleatorio = Random.Range(1, 10);
        InputDecoder.InterfaceElements.SetActive(false);
        InputDecoder.readScript("Script/Enredo"+numeroAleatorio);
        PlayerPrefs.SetInt("numeroAleatorio", numeroAleatorio);
        PlayerPrefs.Save();
        dialogueSource = "Audio/Roteiros/ROTEIRO"+numeroAleatorio;
        PlayAudio(dialogueSource+"/Au1", () =>
        {
            setinha();
        });
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown("h"))
        {
            if(InputDecoder.InterfaceElements.activeInHierarchy)
            {
                InputDecoder.InterfaceElements.SetActive(false);
            }
            else
            {
                InputDecoder.InterfaceElements.SetActive(true);
            }
        }*/

        if(InputDecoder.Commands[InputDecoder.CommandLine] != InputDecoder.LastCommand)
        {
            InputDecoder.LastCommand = InputDecoder.Commands[InputDecoder.CommandLine];
            InputDecoder.PausedHere = false;
            InputDecoder.ParseInputLine(InputDecoder.Commands[InputDecoder.CommandLine]);
        }else
        {
            InputDecoder.PausedHere = true;
        }

        if(!InputDecoder.PausedHere && InputDecoder.CommandLine < InputDecoder.Commands.Count - 1)
        {
            InputDecoder.CommandLine ++;
        }

        if(InputDecoder.PausedHere && seta && InputDecoder.CommandLine < InputDecoder.Commands.Count - 1)
        {
            InputDecoder.CommandLine ++;
            seta = false;
            cont ++;
            PlayAudio(dialogueSource+"/Au"+cont, () =>
            {
                setinha();
            });
        }
    }

    public void setinha()
    {
        seta = true;
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

    public void playAgain()
    {
        PlayAudio("Audio/button", () =>
        {
            InputDecoder.Repetir.SetActive(false);
            InputDecoder.Quiz.SetActive(false);

            InputDecoder.labels = new List<Label>();

            InputDecoder.Commands = new List<string>();
            InputDecoder.CommandLine = 0;
            InputDecoder.LastCommand = "";

            cont = 1;

            InputDecoder.readScript("Script/Enredo"+numeroAleatorio);

            PlayAudio(dialogueSource+"/Au1", () =>
            {
                setinha();
            });
        });
    }
    
    public void goQuiz()
    {
        PlayAudio("Audio/button", () =>
        {
            InputDecoder.end();
        });
    }

    public void playButtonAudio()
    {
        // Reproduz o áudio ao clicar no botão
        PlayAudio("Audio/button", () =>
        {
            // funções extras
        });
    }
}

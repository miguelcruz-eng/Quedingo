using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BingoManager : MonoBehaviour
{
    public ImageManager imageManager; // Referência ao ImageManager
    public Button startButton; // Referência ao botão que inicia a animação
    public List<GameObject> numberImages; // Lista de imagens associadas aos números sorteados
    public ButtonManager buttonManager; // Referência ao script ButtonManager
    public GameObject vitoriaObject;
    public GameObject bingoObject;
    public GameObject alertaObject;
    public GameObject derrotaObject;

    private List<int> availableNumbers = new List<int>();
    public List<int> selectedNumbers = new List<int>(); // Lista para armazenar os números sorteados
    public int selectedNumber;
    public int quantidadeNumerosSorteados = 10; // Defina a quantidade desejada de números sorteados
    public int estrelas;
    public bool vitoria = false;

    private bool buttonPressed = false; // Flag para indicar se o botão foi pressionado

    private void Start()
    {
        bingoObject.SetActive(true);
        vitoriaObject.SetActive(false);
        LoadEstrelas();
        InitializeNumbers();
        InitializeNumberImages();
        // Adiciona um listener para o botão
        startButton.onClick.AddListener(StartAnimation);

        // Inicia o contador para chamar a função StartAnimation a cada 6 segundos
        InvokeRepeating("AutoStartAnimation", 6f, 6f);
    }

    // Função para iniciar automaticamente a animação a cada 6 segundos
    private void AutoStartAnimation()
    {
        if (!buttonPressed)
        {
            StartAnimation();
        }
    }

    private void InitializeNumbers()
    {
        // Inicializa a lista com números de 1 a 50
        for (int i = 1; i <= 50; i++)
        {
            availableNumbers.Add(i);
        }
    }

    private void InitializeNumberImages()
    {
        // Inicializa a lista de imagens associadas aos números
        for (int i = 1; i <= 50; i++)
        {
            GameObject numberImage = GameObject.Find("tag" + i);
            if (numberImage != null)
            {
                numberImages.Add(numberImage);
                Image imageComponent = numberImage.GetComponent<Image>();
                imageComponent.CrossFadeAlpha(0f, 0f, true); // Define a transparência inicial para 0
            }
            else
            {
                Debug.LogError("Objeto com a tag 'tag" + i + "' não encontrado!");
            }
        }
    }

    private void StartAnimation()
    {
        if (availableNumbers.Count == 0)
        {
            // Se não houver mais números disponíveis, encerre as interações
            Debug.Log("Todas as interações foram concluídas!");
            return;
        }

        // Sorteia um número aleatório
        int randomIndex = Random.Range(0, availableNumbers.Count);
        selectedNumber = availableNumbers[randomIndex];
        selectedNumbers.Add(selectedNumber); // Adiciona o número sorteado à lista

        // Remove o número sorteado da lista
        availableNumbers.RemoveAt(randomIndex);

        // Obtém o componente TextMeshPro filho de images[0]
        TextMeshProUGUI textMeshPro = imageManager.images[3].GetComponentInChildren<TextMeshProUGUI>();

        // Atualiza o texto com o número sorteado
        textMeshPro.text = "" + selectedNumber;

        // Ativa a imagem associada ao número sorteado e ajusta a transparência
        GameObject selectedImage = numberImages[selectedNumber - 1];
        Image imageComponent = selectedImage.GetComponent<Image>();
        imageComponent.CrossFadeAlpha(1f, 0f, true); // Faz um fade-in da transparência

        // Chama a função de animação no ImageManager
        StartCoroutine(imageManager.MoveImages());


        // Verifica se atingiu a quantidade desejada de interações
        if (availableNumbers.Count == 50 - quantidadeNumerosSorteados)
        {
            Debug.Log("Quantidade desejada de interações foi alcançada!");
            alertaObject.SetActive(true);
            startButton.interactable = false;
        }
    }
    
    public void botaoFinalizacao()
    {
        alertaObject.SetActive(false);
        Invoke("VerificarVitoria", 15f);
    }

    void VerificarVitoria()
    {
        // Verificando se a condição de vitória foi atendida
        if (!vitoria)
        {
            // Se vitoria for falso, ativar o GameObject chamado "perdeu"
            derrotaObject.SetActive(true);
        }
    }

    // Função chamada quando o botão é pressionado
    // Função chamada quando o botão é pressionado
    public void OnStartButtonPressed()
    {
        buttonPressed = true;
    }

    void SaveEstrelas()
    {
        PlayerPrefs.SetInt("Estrelas", estrelas);
        PlayerPrefs.Save();
    }

    void LoadEstrelas()
    {
        estrelas = PlayerPrefs.GetInt("Estrelas", 0);
        // Se o valor não existir, será usado o valor padrão (0 no caso)
    }

    public void voltarMenu()
    {
        SaveEstrelas();
        SceneManager.LoadScene(1);
    }
}

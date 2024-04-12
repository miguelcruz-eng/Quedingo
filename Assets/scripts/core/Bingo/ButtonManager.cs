using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    public BingoManager bingoManager;
    public List<Button> buttons; // Lista de botões
    public List<TextMeshProUGUI> textMeshPros; // Lista de TextMeshPro associados aos botões ímpares
    int numberOfButtonsToFill;

    private void Start()
    {
        InitializeButtons();
        numberOfButtonsToFill = textMeshPros.Count; // Defina a quantidade desejada de botões a serem preenchidos
        AssignRandomOrderedValuesToButtons(numberOfButtonsToFill);
        bingoManager.quantidadeNumerosSorteados = 48;
        foreach (Button button in buttons)
        {   
            button.onClick.AddListener(() => OnButtonClick(button, button.GetComponentInChildren<TextMeshProUGUI>()));
        }

    }

    private void InitializeButtons()
    {
        // Inicializa as listas de botões e TextMeshPro
        buttons = new List<Button>();
        textMeshPros = new List<TextMeshProUGUI>();

        for (int i = 1; i <= 25; i++)
        {
            Button button = GameObject.Find("Button" + i).GetComponent<Button>();
            buttons.Add(button);

            if (i % 2 != 0)
            {
                TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPros.Add(textMeshPro);
            }
        }
    }

    private void AssignRandomOrderedValuesToButtons(int numberOfButtons)
    {
        // Gera números aleatórios únicos entre 1 e 50
        List<int> randomValues = GenerateUniqueRandomValues(1, 50, numberOfButtons);

        // Ordena os valores em ordem crescente
        randomValues.Sort();

        // Preenche os TextMeshPro dos botões ímpares com os valores em ordem crescente
        for (int i = 0; i < textMeshPros.Count; i++)
        {
            textMeshPros[i].text = randomValues[i].ToString();
        }
    }

    private List<int> GenerateUniqueRandomValues(int min, int max, int count)
    {
        // Gera uma lista de valores únicos e aleatórios entre min e max
        List<int> values = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int randomValue;
            do
            {
                randomValue = Random.Range(min, max + 1);
            } while (values.Contains(randomValue));

            values.Add(randomValue);
        }

        return values;
    }

    // Método para ativar um objeto filho chamado "Image"
    private void ActivateChildObject(Transform parent)
    {
        Transform child = parent.Find("Image");
        if (child != null)
        {
            child.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Objeto filho com o nome 'Image' não encontrado.");
        }
    }


    // Função chamada quando um botão é clicado
    private void OnButtonClick(Button button, TextMeshProUGUI textMeshPro)
    {
        Debug.Log("Botão pressionado! Texto: " + textMeshPro.text);

        // Adicione aqui a lógica específica para o clique do botão
        //int calledNumber = bingoManager.selectedNumber;

        // Verifica se o número sorteado em BingoManager é igual ao conteúdo do TextMeshPro
        if (int.TryParse(textMeshPro.text, out int buttonNumber) && bingoManager.selectedNumbers.Contains(buttonNumber))
        {
            // Ativa o objeto filho chamado "Image" do botão
            ActivateChildObject(button.transform);

            if(numberOfButtonsToFill <= 1)
            {
                Debug.Log("Voce venceu!");
                bingoManager.estrelas++;
                bingoManager.vitoria = true;
                // Desabilite o GameObject "Bingo"
                bingoManager.bingoObject.SetActive(false);
                // Habilite o GameObject "Vitoria"
                bingoManager.vitoriaObject.SetActive(true);
            }else
            {
                numberOfButtonsToFill --;   
            }
            Debug.Log(numberOfButtonsToFill);
            // Desabilita o botão
            button.interactable = false;
        }
    }

}

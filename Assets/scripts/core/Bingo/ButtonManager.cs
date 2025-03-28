using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    public BingoManager bingoManager;
    public List<Button> buttons; // Lista de botões

    private bool[] buttonStates = new bool[25];

    public List<TextMeshProUGUI> textMeshPros; // Lista de TextMeshPro associados aos botões ímpares
    int numberOfButtonsToFill;

    private void Start()
    {
        InitializeButtons();
        AssignRandomOrderedValuesToColumns();
        bingoManager.quantidadeNumerosSorteados = 25;
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

            // if (i % 2 != 0)
            // {
            //     TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
            //     textMeshPros.Add(textMeshPro);
            // }
            TextMeshProUGUI textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPros.Add(textMeshPro);
        }
    }

    private void AssignRandomOrderedValuesToColumns()
    {
        // Define os intervalos para cada coluna
        Dictionary<string, (int, int)> columnRanges = new Dictionary<string, (int, int)>
        {
            { "B", (1, 10) },
            { "I", (11, 20) },
            { "N", (21, 30) },
            { "G", (31, 40) },
            { "O", (41, 50) }
        };

        // Itera pelas colunas e preenche os botões correspondentes
        int buttonIndex = 0;
        foreach (var range in columnRanges.Values)
        {
            List<int> randomValues = GenerateUniqueRandomValues(range.Item1, range.Item2, 5);
            randomValues.Sort(); // Ordena os valores

            for (int i = 0; i < randomValues.Count; i++)
            {
                textMeshPros[buttonIndex].text = randomValues[i].ToString();
                buttonIndex++;
            }
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
        // Debug.Log("Nome do botão: " + button.name);

        // Adicione aqui a lógica específica para o clique do botão
        //int calledNumber = bingoManager.selectedNumber;

        // Verifica se o número sorteado em BingoManager é igual ao conteúdo do TextMeshPro
        if (int.TryParse(textMeshPro.text, out int buttonNumber) && bingoManager.selectedNumbers.Contains(buttonNumber))
        {
           // Extraia o índice do nome do botão
            string buttonName = button.name;
            if (buttonName.StartsWith("Button") && int.TryParse(buttonName.Substring(6), out int buttonIndex))
            {
                // Ajuste o índice para corresponder ao array zero-based (subtrair 1)
                buttonIndex -= 1;

                // Atualize o estado do botão no array
                if (buttonIndex >= 0 && buttonIndex < buttonStates.Length)
                {
                    buttonStates[buttonIndex] = true;
                }
            }

            // Ativa o objeto filho chamado "Image" do botão
            ActivateChildObject(button.transform);

            // Desabilita o botão
            button.interactable = false;

            if(AreSpecificButtonsDisabled())
            {
                Debug.Log("Voce venceu!");
                bingoManager.estrelas++;
                bingoManager.vitoria = true;

                bingoManager.Ganhou();

                bingoManager.PreTelaTrofeus();
            }
        }
    }

    private bool AreSpecificButtonsDisabled()
    {
        return (buttonStates[0] && buttonStates[1] && buttonStates[2] && buttonStates[3] && buttonStates[4]) || // Linha 1
       (buttonStates[5] && buttonStates[6] && buttonStates[7] && buttonStates[8] && buttonStates[9]) || // Linha 2
       (buttonStates[10] && buttonStates[11] && buttonStates[12] && buttonStates[13] && buttonStates[14]) || // Linha 3
       (buttonStates[15] && buttonStates[16] && buttonStates[17] && buttonStates[18] && buttonStates[19]) || // Linha 4
       (buttonStates[20] && buttonStates[21] && buttonStates[22] && buttonStates[23] && buttonStates[24]) || // Linha 5

       (buttonStates[0] && buttonStates[5] && buttonStates[10] && buttonStates[15] && buttonStates[20]) || // Coluna 1
       (buttonStates[1] && buttonStates[6] && buttonStates[11] && buttonStates[16] && buttonStates[21]) || // Coluna 2
       (buttonStates[2] && buttonStates[7] && buttonStates[12] && buttonStates[17] && buttonStates[22]) || // Coluna 3
       (buttonStates[3] && buttonStates[8] && buttonStates[13] && buttonStates[18] && buttonStates[23]) || // Coluna 4
       (buttonStates[4] && buttonStates[9] && buttonStates[14] && buttonStates[19] && buttonStates[24]) || // Coluna 5

       (buttonStates[0] && buttonStates[6] && buttonStates[12] && buttonStates[18] && buttonStates[24]) || // Diagonal principal
       (buttonStates[4] && buttonStates[8] && buttonStates[12] && buttonStates[16] && buttonStates[20]);   // Diagonal secundária
    }

}

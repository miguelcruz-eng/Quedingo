using UnityEngine;
using UnityEngine.UI;

public class HexToColor : MonoBehaviour
{
    [SerializeField]
    private string hexColor = "#000000"; // Defina o valor hexadecimal aqui, ou altere no Inspector

    void Start()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            GetComponent<Image>().color = color;
            // Aplicar cor para o componente Button (se existir)
            Button button = GetComponent<Button>();
            if (button != null)
            {
                // Aplicar cor para o normal color do botão
                ColorBlock colors = button.colors;
                colors.normalColor = color;
                button.colors = colors;
            }
        }
        else
        {
            Debug.LogError("Invalid hex color string");
        }
    }
}

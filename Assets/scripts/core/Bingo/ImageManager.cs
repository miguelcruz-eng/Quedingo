using UnityEngine;

public class ImageManager : MonoBehaviour
{
    public GameObject[] images; // Array de imagens
    public float speed = 2f; // Velocidade da animação
    public float distanceBetweenImages = 1f; // Distância entre as imagens
    public float bottomAnimationDistance = 0.5f; // Distância para baixo da última imagem antes de aparecer no topo

    private Vector3[] initialPositions;

    private void Start()
    {
        // Salva as posições iniciais
        SaveInitialPositions();
    }

    private void SaveInitialPositions()
    {
        // Salva as posições iniciais
        initialPositions = new Vector3[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            initialPositions[i] = images[i].transform.position;
        }
    }

    public System.Collections.IEnumerator MoveImages()
    {
        float elapsedTime = 0f;
        Vector3[] targetPositions = new Vector3[images.Length];

        while (elapsedTime < 1f)
        {
            //desativa a ultima imagem por enquanto
            images[images.Length - 1].SetActive(false);
            // Calcula as novas posições das imagens
            for (int i = 0; i < images.Length - 1; i++)
            {
                targetPositions[i] = Vector3.Lerp(initialPositions[i], initialPositions[i + 1], elapsedTime);
                images[i].transform.position = targetPositions[i];
            }

            // Calcula a posição da última imagem
            float yOffset = Mathf.Lerp(0f, -bottomAnimationDistance, elapsedTime);
            targetPositions[images.Length - 1] = new Vector3(initialPositions[images.Length - 1].x, initialPositions[images.Length - 1].y + yOffset, initialPositions[images.Length - 1].z);
            images[images.Length - 1].transform.position = targetPositions[images.Length - 1];

            // Atualiza o tempo decorrido
            elapsedTime += Time.deltaTime * speed;

            // Aguarda um frame
            yield return null;
        }

        // Move a última imagem para a primeira posição no array
        GameObject lastObject = images[images.Length - 1];
        for (int i = images.Length - 1; i > 0; i--)
        {
            images[i] = images[i - 1];
        }
        images[0] = lastObject;

        // Reposiciona a última imagem na primeira posição
        images[0].transform.position = initialPositions[0];
        images[0].SetActive(true);
    }
}

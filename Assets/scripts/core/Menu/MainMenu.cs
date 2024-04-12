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
    public GameObject Loja;
    public GameObject Erro1;
    public GameObject Erro2;
    public int points;
    public int cartelas;
    public int estrelas;
    public TextMeshProUGUI PointsTxt;
    public TextMeshProUGUI PointsTxt2;
    public TextMeshProUGUI PointsTxt3;
    public TextMeshProUGUI PointsTxt4;
    public TextMeshProUGUI CartelasTxt;
    public TextMeshProUGUI EstrelasTxt;

    // Start is called before the first frame update
    void Start()
    {
        LoadPoints();
        LoadCartelas();
        LoadEstrelas();
        PointsTxt.SetText(points.ToString());
        PointsTxt2.SetText(points.ToString());
        PointsTxt3.SetText(points.ToString());
        PointsTxt4.SetText(points.ToString());
        CartelasTxt.SetText(cartelas.ToString());
        EstrelasTxt.SetText(estrelas.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        PointsTxt.SetText(points.ToString());
        PointsTxt2.SetText(points.ToString());
        PointsTxt3.SetText(points.ToString());
        PointsTxt4.SetText(points.ToString());
        CartelasTxt.SetText(cartelas.ToString());
        EstrelasTxt.SetText(estrelas.ToString());
        SavePoints();
        SaveCartelas();
    }

    public void playQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void comoJogar()
    {
        //MenuPrincipal.SetActive(false);
       // MenuDicas.SetActive(true);
    }

    public void quitGame()
    {
        Debug.Log("Fechando Jogo");
        Application.Quit();
    }

    void SavePoints()
    {
        PlayerPrefs.SetInt("Points", points);
        PlayerPrefs.Save();
    }

    void LoadPoints()
    {
        points = PlayerPrefs.GetInt("Points", 0);
        // Se o valor não existir, será usado o valor padrão (0 no caso)
    }

    void SaveCartelas()
    {
        PlayerPrefs.SetInt("Cartelas", cartelas);
        PlayerPrefs.Save();
    }

    void LoadCartelas()
    {
        cartelas = PlayerPrefs.GetInt("Cartelas", 0);
        // Se o valor não existir, será usado o valor padrão (0 no caso)
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

    public void checaMoedas()
    {
        if(points < 3)
        {
            Loja.SetActive(false);
            Erro1.SetActive(true);
            Debug.Log("Moedas insuficientes");
        }else
        {
            Debug.Log("Comprando Cartela...");
            points -= 3;
            cartelas++;
            Update();
        }

    }

    public void checaCartelas()
    {
        if(cartelas <= 0)
        {
            Loja.SetActive(false);
            Erro2.SetActive(true);
            Debug.Log("Cartelas insuficientes");
        }else
        {
            Debug.Log("Comecando bingo...");
            cartelas--;
            Update();
            SceneManager.LoadScene(4);
        }

    }
}

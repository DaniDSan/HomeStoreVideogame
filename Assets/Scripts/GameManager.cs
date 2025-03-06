using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum ScreenName
{
    MainMenu,Pause,Options,CharacterSelector,World
}

[System.Serializable]
public struct Screen
{
    //Id con el que controlamos el tipo de pantalla.
    public ScreenName name;

    //Objeto que actua como esa pantalla.
    public GameObject reference;
}

[System.Serializable]
public struct Character
{
    public string name;

    public Sprite sprite;
}

public class GameManager : MonoBehaviour
{
    [Header("Dinero")]
    [SerializeField] private float currentMoney;

    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Screens")]
    [SerializeField] private List<Screen> screens;

    [SerializeField] private Screen currentScreen;

    [SerializeField] private ScreenName lastScreen;

    [Header("Personajes")]
    [SerializeField] private Character[] characters;

    [Header("CharacterUI")]
    [SerializeField] private Image playerImage;

    [SerializeField] private TextMeshProUGUI playerName;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        //Hacemos que por defecto se almacene en la currentScreen que este indicada en el enum.
        foreach (Screen screen in screens)
        {
            if (screen.name == currentScreen.name)
            {
                currentScreen.reference = screen.reference;
                return;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentScreen.name == ScreenName.World) ChangeScreen(ScreenName.Pause);
            else if (currentScreen.name == ScreenName.Pause) ChangeScreen(ScreenName.World);
        }
    }

    public void ChangeScreen(ScreenName nextScreenName)
    {
        foreach (Screen screen in screens)
        {
            if(screen.name == nextScreenName)
            {
                Debug.Log("Cambiando a pantalla " + nextScreenName.ToString());

                //1.Activamos la nueva pantalla.
                screen.reference.SetActive(true);

                //2.Desactivamos la pantalla actual en la que estaba el jugador.
                currentScreen.reference.SetActive(false);

                //3.Indicamos cual es la nueva LastScreen.
                lastScreen = currentScreen.name;

                //4.Actualizamos cual es la currentScreen.
                currentScreen = screen;

                //5.Salimos de la función porque se ha completado el cambio de pantalla.
                return;
            }
        }
    }

    public void SetCharacter(Sprite sprite)
    {
        foreach (Character character in characters)
        {
            if(sprite == character.sprite)
            {
                Debug.Log("Se ha elegido un personaje");

                playerImage.sprite = character.sprite;

                playerName.text = character.name;

                break;
            }
        }
        ChangeScreen(ScreenName.World);
    }

    public void ChangeScreen(string nextScreenName)
    {
        foreach (Screen screen in screens)
        {
            if(screen.name.ToString() == nextScreenName)
            {
                ChangeScreen(screen.name);

                return;
            }
        }
        Debug.LogError("Se esta intentando acceder a una pantalla que no existe.");
    }

    public void PauseGame(bool state)
    {
        if (state)
        {
            Time.timeScale = 0f;

            ChangeScreen(ScreenName.Pause);
        }
        else
        {
            Time.timeScale = 1f;

            ChangeScreen(ScreenName.World);
        }
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del videojuego");

        Application.Quit();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;

        UpdateMoneyTexts();
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;

        UpdateMoneyTexts();
    }

    private void UpdateMoneyTexts()
    {
        moneyText.text = currentMoney.ToString("N0");
    }
}
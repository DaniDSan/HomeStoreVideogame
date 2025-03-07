using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum ScreenName {
    MainMenu, Pause, Options, CharacterSelector, World, Podium
}

public enum SurpriseType {
    AddMoney, RemoveMoney, Optional
}

public enum HomeState {
    Bad, Neutral, Good
}

public struct Surprise {
    [Header("DefaultData")]
    public SurpriseType type;

    public int amount;

    [TextArea(2, 5)]
    public string text;

    [Header("Optional")]
    public bool optional;

    public SurpriseType leftOptionType;

    [TextArea(2, 5)]
    public string leftOptionText;

    public SurpriseType rightOptionType;

    [TextArea(2, 5)]
    public string rightOptionText;
}

[System.Serializable]
public struct Screen {
    //Id con el que controlamos el tipo de pantalla.
    public ScreenName name;

    //Objeto que actua como esa pantalla.
    public GameObject reference;
}

[System.Serializable]
public struct Character {
    public string name;

    public Sprite sprite;
}

[System.Serializable]
public class HomeData {
    //Referencia al punto en el que se encuentra esa casa en el mundo.
    public Image worldReference;

    //Variable donde se almacena el precio que va a tener la casa.
    public int playerPrice;

    //Si true, esta casa es propiedad del jugador.
    public bool bought;

    //Precio minimo que puede tener la casa.
    public int minPrice;

    //Precio maximo que puede tener la casa.
    public int maxPrice;

    [Range(0, 100)]
    //Probabilidad de que al comprar la casa aparezca alguan sorpresa.
    public int surpriseProbability;

    //Estado en el que se encuentra la casa.
    public HomeState state = HomeState.Bad;
}

public class GameManager : MonoBehaviour {
    [Header("Dinero")]
    [SerializeField] private float currentMoney;

    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Screens")]
    [SerializeField] private List<Screen> screens;

    [SerializeField] private Screen currentScreen;

    [SerializeField] private ScreenName lastScreen;

    [Header("Personajes")]
    [SerializeField] private Character[] characters;

    [Header("Top")]
    [SerializeField] private TextMeshProUGUI topText;

    [SerializeField] private Image topSprite;

    [SerializeField] private Transform topContainer;

    [Header("Compra casas")]
    [SerializeField] private int playerLevel = 1;

    [SerializeField] public List<HomeData> tier1Home;

    [SerializeField] private GameObject buyHomePanel;

    [SerializeField] private TextMeshProUGUI homePriceText;

    [SerializeField] private Button buyButton;

    [SerializeField] private HomeData currentHomeData;

    [Header("Venta casas")]
    [SerializeField] private GameObject sellHomePanel;

    [SerializeField] private TextMeshProUGUI costText;

    [Header("Sorpresas")]
    [SerializeField] private Surprise[] surprises;

    [Header("CharacterUI")]
    [SerializeField] private Image playerImage;

    [SerializeField] private TextMeshProUGUI playerName;

    [Header("EstadisticasJugador")]
    [SerializeField] private int sales;

    [Header("Casa a instanciar")]
    [SerializeField] GameObject[] houses;
    [SerializeField] GameObject tempHouse;
    [SerializeField] Transform instatiateHousePos;
    [SerializeField] Vector3 offsetCameraEditMode;
    [SerializeField] Vector3 cameraStartPos;
    [SerializeField] GameObject[] objectsToActivate;

    public static GameManager instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }

        //Hacemos que por defecto se almacene en la currentScreen que este indicada en el enum.
        foreach(Screen screen in screens) {
            if(screen.name == currentScreen.name) {
                currentScreen.reference = screen.reference;
                return;
            }
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(buyHomePanel.activeInHierarchy) buyHomePanel.SetActive(false);
            else if(sellHomePanel.activeInHierarchy) sellHomePanel.SetActive(false);
            else if(currentScreen.name == ScreenName.Podium) ChangeScreen(ScreenName.World);
            else if(currentScreen.name == ScreenName.World) ChangeScreen(ScreenName.Pause);
            else if(currentScreen.name == ScreenName.Pause) ChangeScreen(ScreenName.World);
        }
    }

    public void ChangeScreen(ScreenName nextScreenName) {
        foreach(Screen screen in screens) {
            if(screen.name == nextScreenName) {
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

    public void SetCharacter(Sprite sprite) {
        foreach(Character character in characters) {
            if(sprite == character.sprite) {
                Debug.Log("Se ha elegido un personaje");

                playerImage.sprite = character.sprite;
                topSprite.sprite = character.sprite;

                playerName.text = character.name;
                topText.text = character.name;

                break;
            }
        }
        ChangeScreen(ScreenName.World);
    }

    public void ChangeScreen(string nextScreenName) {
        foreach(Screen screen in screens) {
            if(screen.name.ToString() == nextScreenName) {
                ChangeScreen(screen.name);

                return;
            }
        }
        Debug.LogError("Se esta intentando acceder a una pantalla que no existe.");
    }

    public void PauseGame(bool state) {
        if(state) {
            Time.timeScale = 0f;

            ChangeScreen(ScreenName.Pause);
        } else {
            Time.timeScale = 1f;

            ChangeScreen(ScreenName.World);
        }
    }

    public void ReturnMainMenu() {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    public void ExitGame() {
        Debug.Log("Saliendo del videojuego");

        Application.Quit();
    }

    public void AddMoney(int amount) {
        currentMoney += amount;

        UpdateMoneyTexts();
    }

    public void RemoveMoney(int amount) {
        currentMoney -= amount;

        UpdateMoneyTexts();
    }

    private void UpdateMoneyTexts() {
        moneyText.text = currentMoney.ToString("N0") + "$";
    }

    public HomeData GetHomeData(int tier) {
        HomeData homeData = new HomeData();

        switch(tier) {
            case 1:
                homeData.playerPrice = tier1Home[Random.Range(0, tier1Home.Count)].playerPrice;
                break;
        }
        return homeData;
    }

    public void ShowHomeData(HomeData homeData) {
        currentHomeData = homeData;

        //Comprobamos si la casa a la que se quiere acceder ha sido comprada por el jugador.
        if(homeData.bought) {
            costText.text = homeData.playerPrice.ToString("N0") + "$";

            sellHomePanel.SetActive(true);
        } else {
            //Comprobamos si el jugador puede comprar la casa.
            buyButton.interactable = currentMoney >= homeData.playerPrice ? true : false;

            homePriceText.text = homeData.playerPrice.ToString("N0") + "$";

            buyHomePanel.SetActive(true);
        }
    }

    public void BuyHome() {
        RemoveMoney(currentHomeData.playerPrice);

        currentHomeData.bought = true;

        InstantiateHome();
    }

    public void SellHome() {
        Debug.Log("Llamar a agregar dinero");
        Camera.main.transform.position = cameraStartPos;
        Destroy(tempHouse);
    }

    //Bureg
    public void InstantiateHome() {
        cameraStartPos = Camera.main.transform.position;
        Camera.main.transform.position = instatiateHousePos.position + offsetCameraEditMode;
        int randomIndex = Random.Range(0, houses.Length);
        tempHouse = Instantiate(houses[randomIndex], instatiateHousePos.position, Quaternion.identity);
        EditHome();
    }

    public void EditHome() {
        cameraStartPos = Camera.main.transform.position;
        Camera.main.transform.position = instatiateHousePos.position + offsetCameraEditMode;
        foreach(GameObject objectToActivate in objectsToActivate) {
            objectToActivate.SetActive(true);
        }
    }

    public void ExitEditHome() {
        Camera.main.transform.position = cameraStartPos;
        foreach(GameObject objectToActivate in objectsToActivate) {
            objectToActivate.SetActive(false);
        }
    }
}
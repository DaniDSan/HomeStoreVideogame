using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenName
{
    MainMenu,Pause,Options
}

[System.Serializable]
public struct Screen
{
    //Id con el que controlamos el tipo de pantalla.
    public ScreenName name;

    //Objeto que actua como esa pantalla.
    public GameObject reference;
}

public class GameManager : MonoBehaviour
{
    [Header("General")]
    public bool onApartment;

    [Header("Dinero")]
    [SerializeField] private float currentMoney;

    [Header("Screens")]
    [SerializeField] private List<Screen> screens;

    [SerializeField] private Screen currentScreen;

    [SerializeField] private ScreenName lastScreen;

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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratedHome : MonoBehaviour
{
    public int tier;

    public HomeData homeData;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(ShowHome);
    }

    private void Start()
    {
        homeData = GameManager.instance.GetHomeData(tier);

        homeData.worldReference = GetComponent<Image>();
    }

    public void ShowHome()
    {
        GameManager.instance.ShowHomeData(homeData);
    }
}

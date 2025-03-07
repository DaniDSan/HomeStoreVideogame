using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratedHome : MonoBehaviour
{
    public HomeData homeData;

    private Button button;

    private void Awake()
    {
        homeData = GameManager.instance.GetHomeData();

        button = GetComponent<Button>();

        button.onClick.AddListener(ShowHomeData);
    }

    public void ShowHomeData()
    {
        GameManager.instance.ShowHomeData(homeData);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VacuumShaders.CurvedWorld;

public class Game_Controll : MonoBehaviour
{

    public static Game_Controll Instance;
    public bool game, pause;
    [SerializeField] GameObject lose_panel;
    [SerializeField] Text money_text;
    [SerializeField] int money_int;

    public Slider load_slider;
    public Text load_text;
    public GameObject load_panel;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        game = true;
        money_int = 0;
        money_text.text = money_int.ToString();
        
    }
    private void Update()
    {
       
    }

    public void Add_money(int id)
    {
        money_int += id;
        money_text.text = money_int.ToString();
    }
      
    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            pause = false;
            Time.timeScale = 1;
        }
        else
        {
            pause = true;
            Time.timeScale = 0;
        }
    }
    public void Click()
    {
        if (Sound.Instance != null)
            Sound.Instance.Click();
    }

    public void Continue()
    {
        game = true;
        Time.timeScale = 1;
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        SceneManager.LoadScene("game 1");
    }
    public void Lose()
    {
        game = false;
        lose_panel.SetActive(true);
        Player_controll.Instance.game = false;
    }
    public void Load_level(string name)
    {
        Time.timeScale = 1;
        load_panel.SetActive(true);
        StartCoroutine(Load(name));
    }

    IEnumerator Load(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            load_slider.value = asyncLoad.progress;
            load_text.text = (load_slider.value * 100).ToString("f0") + "%";
            if (asyncLoad.progress >= 0.9f && !asyncLoad.allowSceneActivation)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

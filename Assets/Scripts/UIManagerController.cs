using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;    
public class UIManagerController : MonoBehaviour
{
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;

    private void Start()
    {
        panelWin.SetActive(false);
        panelLose.SetActive(false);
        panelPause.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOn();
        }
    }

    
    public void PauseOn()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Rerun()
    {
        SceneManager.LoadScene("basementMain");
        Time.timeScale = 1;
    }
    
    public void PauseOff()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void Win()
    {
        panelWin.SetActive(true);
        Time.timeScale = 0;
    }
    public void Lose()
    {
        panelLose.SetActive(true);
        Time.timeScale = 0;
    }
}

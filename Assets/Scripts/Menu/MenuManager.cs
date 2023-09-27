using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private GameObject[] _menusButton;

    public void play()
    {
        SceneManager.LoadScene("Game");
    }

    public void quit()
    {
        Application.Quit();
    }
    public void options ()
    {

    }

    public void credits()
    {

    }


}

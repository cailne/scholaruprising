using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public void SelectLevelMenu()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void GoToOptions()
    {
        SceneManager.LoadScene("OptionMenu");
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

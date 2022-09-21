using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{

    private void Update()
    {
        // Backspace restarts level
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReloadLevel();
        }
        // Escape key exits application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exit the game");
            Application.Quit();
        }
    }

    // Starts level at beginning
    void ReloadLevel()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }
}

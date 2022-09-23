using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonAbility : MonoBehaviour
{
    private void Awake()
    {
        CanvasGroup group = gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(BlinkButton(this.gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        // Space starts game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex <= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public static IEnumerator BlinkButton(GameObject button)
    {
        CanvasGroup group = button.GetComponent<CanvasGroup>();
        // initial value
        group.alpha = 1;

        bool clicked = false;
        // animate value
        while (!clicked)
        {
            group.alpha = 1;
            yield return new WaitForSeconds(0.6f);
            group.alpha = 0;
            yield return new WaitForSeconds(0.3f);
        }
    }
}

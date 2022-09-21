using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text _collectibleTextUI = null;
    [SerializeField] Text _finishedGameTextUI = null;

    void Start()
    {
        HideText();
    }

    // Hides win/lose message
    public void HideText()
    {
        _finishedGameTextUI.text = "";
        _finishedGameTextUI.gameObject.SetActive(false);
    }

    // Shows either win or lose message
    public void ShowText(string textToShow)
    {
        _finishedGameTextUI.text = textToShow;
        _finishedGameTextUI.gameObject.SetActive(true);
    }

    public void UpdateCollectibleCount(int collectibleCount)
    {
        _collectibleTextUI.text = collectibleCount.ToString();
    }
}

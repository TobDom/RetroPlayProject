using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;


public class WinLoseHandler : MonoBehaviour
{
    public TMP_Text winLoseText;
    public TMP_Text turnTakenText;
    public TMP_Text timeTaken;
    public TMP_Text scoreText;
    //last text after the game
    public void UpdateText(bool win, int turns, string time, int score)
    {
        if (win)
        {
            winLoseText.text = "Wygra³eœ!";
            turnTakenText.text = "Iloœæ tur: "+turns;
            timeTaken.text = "Czas: " + time;
            scoreText.text = "Wynik: " + score;
        }
        if (!win)
        {
            winLoseText.text = "Przegra³eœ!";
            turnTakenText.text = "Iloœæ tur: " + turns;
            timeTaken.text = "Czas: " + time;
            scoreText.text = "Wynik: " + score;
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

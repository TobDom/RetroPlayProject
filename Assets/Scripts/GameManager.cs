using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<DraggingBox> tryList = new List<DraggingBox>();

    public GameObject Spacer;

    int currentTurn = 0;
    public int maxTurn;

    public WinLoseHandler handler;

    //time
    int playTime;
    int seconds;
    int minutes;
    public Text timerText;
    //points
    int maxScore = 10000;
    

    private void Awake()
    {
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        GetDraggingBoxChilds();
        UpdateTrys();
        handler.gameObject.SetActive(false);

        StartCoroutine("PlayTime");
    }

    void UpdateTrys()
    {
        tryList[currentTurn].SetActive(true);
    }

    void GetDraggingBoxChilds()
    {
        DraggingBox[] allChildren = Spacer.GetComponentsInChildren<DraggingBox>();
        tryList.AddRange(allChildren);
        //tryList.Reverse();
        maxTurn = tryList.Count;
    }
    //turn amount
    public void SetTrys()
    {
        currentTurn++;
        if (currentTurn < maxTurn)
        {
            UpdateTrys();
            tryList[currentTurn - 1].SetActive(false);
        }
        else
        {
            //Debug.Log("Przegra³eœ");
            LoseCondition();
        }
    }
    void LoseCondition()
    {
        //Debug.Log("Przegra³eœ!");
        tryList[currentTurn-1].SetActive(false);
        UpdateWinHandler(false);
    }
    public void WinCondition()
    {
        //Debug.Log("Wygra³eœ");
        tryList[currentTurn].SetActive(false);
        currentTurn++;
        UpdateWinHandler(true);
    }

    public bool TrysLeft()
    {
        return (currentTurn < maxTurn) ? true : false;
    }

    void UpdateWinHandler(bool win)
    {
        //int rand = 10;

        string time = minutes.ToString("D2") + ":" + seconds.ToString("D2");

        handler.UpdateText(win, currentTurn, time, CalculateScore());
        handler.gameObject.SetActive(true);
        StopCoroutine("PlayTime");
    }

    int CalculateScore()
    {
        int currentScore = 0;
        currentScore = maxScore=currentTurn * 900;
        currentScore -= playTime*2;
        if (currentScore < 0)
        {
            currentScore = 0;
        }
        return currentScore;
    }
    IEnumerator PlayTime()
    {
        while (true)
        {
            playTime++;
            seconds = playTime % 60;
            minutes = playTime / 60 % 60;

            timerText.text= minutes.ToString("D2") + ":" + seconds.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }
}

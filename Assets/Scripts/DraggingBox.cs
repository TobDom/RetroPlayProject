using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DraggingBox : MonoBehaviour
{
    public GameObject protector;

    public bool isActive;

    public Button checkButton;

    public int[] pearlIDs;

    public CreateRiddle2D riddle;

    public GameObject hintBox;
    public GameObject blackHintPearl;
    public GameObject whiteHintPearl;
    //setting first values 
    void Start()
    {
        checkButton.interactable = false;
        pearlIDs = new int[riddle.pearlAmount];
    }
    //checking pearls if is the same
    public void SetID(int slot, int ID)
    {
        pearlIDs[slot] = ID;
        //checkButton.interactable = (!pearlIDs.Contains(0) ? true : false);

        if(pearlIDs.Contains(0))
        {
            checkButton.interactable = false;
        }
        else
        {
            checkButton.interactable = true;
        }
    }

    public void CheckSolution()
    {
        riddle.CheckRiddle(pearlIDs, this);
        checkButton.interactable = false;
    }
    //creating pearls in hintbox
    public void CreateHint(int hint)
    {
        //create black pearl
        if(hint == 2)
        {
            Instantiate(blackHintPearl, hintBox.transform, false);
        }
        //create white pearl
        if (hint == 1)
        {
            Instantiate(whiteHintPearl, hintBox.transform, false);
        }
    }
    //setting protector
    public void SetActive(bool active)
    {
        isActive = (active == true) ? true : false;
        protector.SetActive(!isActive);
    }
}

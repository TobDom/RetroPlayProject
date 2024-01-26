using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRiddle2D : MonoBehaviour
{
    public List<GameObject> pearlList = new List<GameObject>();

    public int pearlAmount = 4;

    List<GameObject> riddleList = new List<GameObject>();

    public List<GameObject> slotList = new List<GameObject>();
    //Hide solution 
    Animator anim;

    public GameObject hideImage;

    //creating hide image animation
    void Start()
    { 
        CreateTheRiddle();
        anim = hideImage.GetComponent<Animator>();
    }
    //creating pearls in solutionbox
    void CreateTheRiddle()
    {
        for (int i = 0; i < pearlAmount; i++)
        {
            int num = Random.Range(0, pearlList.Count);

            riddleList.Add(pearlList[num]);

            GameObject pearl = Instantiate(pearlList[num], slotList[i].transform, false);
            pearl.transform.position = pearl.transform.parent.position;

            pearl.GetComponent<Drag2D>().enabled = false;
        }
    }
    //checkbox filling
    public void CheckRiddle(int[] ids, DraggingBox sender)
    {
        int[] places1=new int[pearlAmount];
        int[] places2 = new int[pearlAmount];
        if (pearlAmount == 4)
        {
            places1 = new int[4] { -1, -1, -1, -1 };
            places2 = new int[4] { -1, -1, -1, -1 };
        }
        if (pearlAmount == 5)
        {
            places1 = new int[5] { -1, -1, -1, -1, -1 };
            places2 = new int[5] { -1, -1, -1, -1, -1 };
        }

        int exactMatches = 0;
        int halfMatches = 0;

        //Black check
        for (int i = 0; i < pearlAmount; i++)
        {
            if(ids[i] == riddleList[i].GetComponent<Drag2D>().pearlID)
            {
                exactMatches++;
                sender.CreateHint(2);
                places1[i] = 1;
                places2[i] = 1;

            }
        }
        //open hide image
        if (exactMatches==pearlAmount)
        {
            anim.SetTrigger("open");
            GameManager.instance.WinCondition();
            return;
        }

        //White check
        for (int i = 0; i < pearlAmount; i++)
        {
            for (int j = 0; j < pearlAmount; j++)
            {
                if(i!=j && (places1[i] !=1) && (places2[j] != 1))
                {
                    if(ids[i] == riddleList[j].GetComponent<Drag2D>().pearlID)
                    {
                        halfMatches++;
                        sender.CreateHint(1);
                        places1[i] = 1;
                        places2[j] = 1;
                        break;
                    }
                }
            }
        }
        //open hide image
        GameManager.instance.SetTrys();
        if (!GameManager.instance.TrysLeft())
        {
            anim.SetTrigger("open");
        }
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class level_manager : MonoBehaviour
{
    //private grid_manager placedTiles; //need to find how many tiles are currently placed

    [SerializeField] private float goalBudget; //set number per each level

    private float currentBudget; //will draw from placedTiles

    public Image budgetBar;


    private void Start()
    {
        currentBudget = 100;//numberOfClassObjects * 10;
    }

    private void Update()
    {
        float numberOfClassObjects = GameObject.FindObjectsOfType(typeof(Prop)).Length;

        

        //Debug.Log(currentBudget);

        budgetBar.fillAmount = currentBudget/1000;

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("pressed");
            currentBudget += 10;
        }

        //Debug.Log(budgetBar.fillAmount);
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class level_manager : MonoBehaviour
{
    [SerializeField] private int goalBudget; //set number per each level. Needs to be reduced beneath this threshold
    [SerializeField] private PropContainer requiredPropContainer;
    [SerializeField] private int propLoadRequirement; // exceed this number
    private Dictionary<HUD_Prop.prop_name,int> required_Props = new Dictionary<HUD_Prop.prop_name, int>(); // force certain items to be required in certain quantities
    
    private int currentBudget;
    private int currentLoad;
    public Image budgetBar;
    private GridManager gm;

    private void Start()
    {
        gm = GridManager.gm;

        // foreach(HUD_Prop p in requiredPropContainer.GetComponentsInChildren<HUD_Prop>()){
        //     if(required_Props.ContainsKey(p.propName)){
        //         required_Props[p.propName] +=1;
        //     }else{
        //         required_Props[p.propName] = 1;
        //     }
        // }
    }

    private void Update()
    {
        currentBudget = 0;
        currentLoad = 0;
        foreach(Prop p in gm.props){
            currentLoad+=p.load;
            currentBudget+=p.cost;
        }
        // Dictionary<HUD_Prop.prop_name, int> currentProps  = new Dictionary<HUD_Prop.prop_name, int>();
        // // get the listings of current props
        // foreach (Prop p in gm.props){
        //     if(currentProps.ContainsKey(p.propName)){
        //         currentProps[p.propName] +=1;
        //     }else{
        //         currentProps[p.propName] =1;
        //     }

        //     currentBudget+=p.cost;
        // }

        budgetBar.fillAmount = ((float)currentBudget)/(goalBudget*4); // convert to int at last possible moment (reduce potential errors)
        

        bool satisfy_prop_require = currentLoad >= propLoadRequirement && currentBudget <= goalBudget;
        // foreach(HUD_Prop.prop_name propName in required_Props.Keys){
        //     // if (!currentProps.ContainsKey(propName) || required_Props[propName] > currentProps[propName]){
        //     if (!currentProps.ContainsKey(propName) || required_Props[propName] > currentProps[propName]){
        //         satisfy_prop_require = false;
        //         break;
        //     }
        // }

        if(currentBudget > goalBudget){
            budgetBar.color = Color.red;
            return;
        }

        if(currentLoad < propLoadRequirement){
            budgetBar.color = Color.magenta;
            return;
        }


        // if you made it here you did it!
        budgetBar.color = Color.green;
        // load the next level

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex+1) % SceneManager.sceneCountInBuildSettings);
    }


}

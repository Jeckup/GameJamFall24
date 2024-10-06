using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Prop : MonoBehaviour
{
    public enum prop_name{
        Janitor,
        L_Desk,
        _3x1,
        T_desk,
        L_Desk_Long,
        File_Cabinet,
        Printer
    }

    public prop_name propName{get;private set;}
    [SerializeField] private Prop prop;

    void Awake(){
        propName = prop.propName;
    }
}

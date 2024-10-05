using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// its getting really annoying having to reference this all the time    
public class SingletonMap : MonoBehaviour{
    public static SingletonMap Instance {get; private set;}

    private Tilemap map;
    public Tilemap Map
    {get{return map;} private set{map = value;}}


    void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        Map = GetComponent<Tilemap>();
    }
}

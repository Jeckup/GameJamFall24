using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager gm {get; private set;}

    public Tilemap map; // place the floor tilemap here. This contains the legal placement area
    public BoundsInt area; // The x,y bounds of the surrounding rectanle around the legal placement area
    [DoNotSerialize] public List<Prop> props;
    public PropContainer PrefilledPropContainer;
    private Prop selected_prop;
    // replace the prop when setting a new one. Remember to delete the old one
    public Prop SelectedProp{
        get{return selected_prop;}
        set{
            if(selected_prop)
                Destroy(selected_prop.gameObject);
            selected_prop = Instantiate(value,value.transform.position,value.transform.rotation);
            // ditching the elements of the selecatble
            Destroy(selected_prop.GetComponent<Selectable>()); 
            Destroy(selected_prop.GetComponent<BoxCollider2D>());
    }}

    void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (gm != null && gm != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            gm = this; 
        } 
    }

    void Start()
    {
        
        // the shape of the level is not guaranteed to be perfectly rectangular, so a dictionary is better
        props = new List<Prop>();

        // for the props already on the screen
        foreach(Prop p in PrefilledPropContainer.GetComponentsInChildren<Prop>()){
            props.Add(p);
        }
    }

    /// <summary>
    /// Find the item located at a position in the tile map
    /// </summary>
    /// <param name="pos">a Vector3Int representing the location to search the tilemap</param>
    /// <returns>The object in the tile, or null if there is nothing</returns>
    public Prop getItemAt(Vector3Int pos){
        foreach(Prop p in props){
            foreach(Vector3Int cell in p.orientedShape){
                if(pos == p.GridLocation + cell){
                    // Debug.DrawRay(map.CellToWorld(p.GridLocation + cell),Vector3.one,Color.red,3f);
                    return p;
                }
            }
        }
        return null;
    } 

    /// <summary>
    /// remove the item in the list located at pos
    /// </summary>
    /// <param name="pos">a Vector3Int representing the location to search the tilemap</param>
    public void deleteItemAt(Vector3Int pos){
        Prop target_prop = getItemAt(pos);
        if(target_prop != null){
            props.Remove(target_prop);
            Destroy(target_prop.gameObject);
        }
    }

    /// <summary>
    /// Creates an item located in pos and adds to the list of items present in the level
    /// </summary>
    /// <param name="pos"></param>
    public void createItemAt(Vector3Int pos){
        // redundant since this condition is checked in Update(), but may be useful to keep it within function?
        if(map.GetSprite(pos) == null || getItemAt(pos) is not null)
                    return;
        Vector3 cell_center = map.GetCellCenterWorld(pos);
        cell_center.z = 0;

        Prop new_prop = Instantiate(selected_prop,cell_center, selected_prop.transform.rotation); // some inefficiency, since logically, the hover prop is already rotated and placed properly
        new_prop.GridLocation = pos; // this may be unnecessary. See Start() in prop.cs
        props.Add(new_prop);
    }

    public void createItem(){
        Vector3Int pos = selected_prop.GridLocation;
        // redundant since this condition is checked in Update(), but may be useful to keep it within function?
        if(map.GetSprite(pos) == null || getItemAt(pos) is not null)
                    return;

        Prop new_prop = Instantiate(selected_prop,selected_prop.transform.position, selected_prop.transform.rotation);
        Debug.Log("created " + new_prop);
        props.Add(new_prop);
    }

    // checks every cell of the shape
    bool validPlacement(){
        Vector3Int pos = selected_prop.GridLocation; 
        foreach(Vector3Int cell in selected_prop.orientedShape){
            Vector3Int shifted_cell = pos+cell;
            if(map.GetSprite(shifted_cell) is null){
                return false;
            }
                
            Prop p = getItemAt(shifted_cell);
            if(p){
                return false;
            }
        }

        return true;
    }

    void Update()
    {
        // get mouse position in 2d plane
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;
        Vector3Int cellPosition = map.WorldToCell(pz); // convert mouse position to Grid position

        if(selected_prop is not null) { // null check
            selected_prop.GridLocation = cellPosition; // mouse hovering 
            bool placeable = validPlacement();

            if(!placeable){
                selected_prop.spriteRenderer.color = Color.red;
            }else{
                selected_prop.spriteRenderer.color = Color.white;
            }


            // rotate
            if (Input.GetButtonDown("Rotate")){
                selected_prop.rotate_90();
            }


            // left click - place down selected prop on tile
            if (Input.GetMouseButtonUp(0))
            {
                // if it's a valid and empty cell, create an object
                if(placeable){
                    // createItemAt(cellPosition);
                    createItem();

                }
                
            } 
        }
        // right click - delete prop on tile
        if (Input.GetMouseButtonUp(1))
        {
            // if it's a valid and filled cell, destroy the prop
            // technically, map.GetSprite() is unnecssary, since the map should never props off the legal tiles
            if(map.GetSprite(cellPosition) is not null  && getItemAt(cellPosition)){
                deleteItemAt(cellPosition);

            }

        }
    }
}

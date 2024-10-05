using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class grid_manager : MonoBehaviour
{
    public Tilemap map; // place the floor tilemap here. This contains the legal placement area
    public BoundsInt area; // The x,y bounds of the surrounding rectanle around the legal placement area
    // public Dictionary<Vector3Int,ClickableTile> slots;
    [DoNotSerialize] public List<Prop> props;
    public PropContainer PrefilledPropContainer;
    public Prop selected_prop;


    private GridLayout grid;
    void Start()
    {
        grid = map.layoutGrid;
        
        // the shape of the level is not guaranteed to be perfectly rectangular, so a dictionary is better
        // slots = new Dictionary<Vector3Int, ClickableTile>();
        props = new List<Prop>();

        // for the props already on the screen
        foreach(Prop p in PrefilledPropContainer.GetComponentsInChildren<Prop>()){
            props.Add(p);
            // Debug.Log("slot added to " + p.gridLocation);
        }
    }

    /// <summary>
    /// Find the item located at a position in the tile map
    /// </summary>
    /// <param name="pos">a Vector3Int representing the location to search the tilemap</param>
    /// <returns>The object in the tile, or null if there is nothing</returns>
    public Prop getItemAt(Vector3Int pos){
        foreach(Prop p in props){
            foreach(Vector3Int cell in p.shape){
                Debug.Log(p.GridLocation + cell);
                if(pos == p.GridLocation + cell){
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
            // Debug.Log("destroyed prop");
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

        Prop new_prop = Instantiate(selected_prop,cell_center, selected_prop.transform.rotation); // some inefficiency, since logically, the hover prop is already rotated and placed properly
        new_prop.GridLocation = pos; // this may be unnecessary. See Start() in prop.cs
        props.Add(new_prop);
        // Debug.Log("set prop to " + new_prop);
    }

    public void createItem(){
        Vector3Int pos = selected_prop.GridLocation;
        // redundant since this condition is checked in Update(), but may be useful to keep it within function?
        if(map.GetSprite(pos) == null || getItemAt(pos) is not null)
                    return;

        Prop new_prop = Instantiate(selected_prop,selected_prop.transform.position, selected_prop.transform.rotation);
        props.Add(new_prop);
    }

    // checks every cell of the shape
    bool validPlacement(){
        Vector3Int pos = selected_prop.GridLocation; 
        foreach(Vector3Int cell in selected_prop.shape){
            Vector3Int shifted_cell = pos+cell;
            if(map.GetSprite(shifted_cell) is null){
                Debug.DrawRay(grid.CellToWorld(shifted_cell),Vector3.one,Color.blue,3f);
                return false;
            }
                
            Prop p = getItemAt(shifted_cell);
            if(p){
                Debug.DrawRay(grid.CellToWorld(shifted_cell),Vector3.one,Color.red,3f);
                p.spriteRenderer.color = (p.spriteRenderer.color == Color.red) ?  Color.white : Color.red;
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
        selected_prop.GridLocation = cellPosition; // mouse hovering 

        bool placeable = validPlacement();

        // rotate
        if (Input.GetButtonDown("Rotate")){
            selected_prop.rotate_90();
            foreach(Vector2Int cell in selected_prop.shape){
                Debug.Log(cell);
            }
        }


        // left click - place down selected prop on tile
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(getItemAt(cellPosition));
            // if it's a valid and empty cell, create an object
            if(placeable){
                // createItemAt(cellPosition);
                createItem();

                // Debug.DrawRay(grid.CellToWorld(cellPosition),Vector3.one,Color.blue,3f);
            }
            
            Debug.Log(cellPosition);
        } 
        // right click - delete prop on tile
        else if (Input.GetMouseButtonUp(1))
        {
            Debug.Log(getItemAt(cellPosition));
            // if it's a valid and filled cell, destroy the prop
            // technically, map.GetSprite() is unnecssary, since the map should never props off the legal tiles
            if(map.GetSprite(cellPosition) is not null  && getItemAt(cellPosition)){
                deleteItemAt(cellPosition);

                // Debug.DrawRay(grid.CellToWorld(cellPosition),Vector3.one,Color.blue,3f);
            }

            // Debug.Log(cellPosition);
        }
    }
}

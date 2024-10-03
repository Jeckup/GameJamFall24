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
    public Prop[] prefilled_props;
    public Prop selected_prop;


    private GridLayout grid;
    void Start()
    {
        grid = map.layoutGrid;
        
        // the shape of the level is not guaranteed to be perfectly rectangular, so a dictionary is better
        // slots = new Dictionary<Vector3Int, ClickableTile>();
        props = new List<Prop>();

        // for the props already on the screen
        foreach(Prop p in prefilled_props){
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
        cell_center.z-=2;

        Prop new_prop = Instantiate(selected_prop,cell_center, Quaternion.identity);
        new_prop.GridLocation = pos; // this may be unnecessary. See Start() in prop.cs
        props.Add(new_prop);
        // Debug.Log("set prop to " + new_prop);
    }

    void Update()
    {
        // left click - place down selected prop on tile
        if (Input.GetMouseButtonUp(0))
        {
            // get mouse click's position in 2d plane
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;

            // convert mouse click's position to Grid position
            Vector3Int cellPosition = grid.WorldToCell(pz);

            Debug.Log(getItemAt(cellPosition));
            // if it's a valid and empty cell, create an object
            if(map.GetSprite(cellPosition) is not null  && !getItemAt(cellPosition)){
                createItemAt(cellPosition);

                Debug.DrawRay(grid.CellToWorld(cellPosition),Vector3.one,Color.blue,3f);
            }
            
            Debug.Log(cellPosition);
        } 
        // right click - delete prop on tile
        else if (Input.GetMouseButtonUp(1))
        {
            // get mouse click's position in 2d plane
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;

            // convert mouse click's position to Grid position
            Vector3Int cellPosition = grid.WorldToCell(pz);

            Debug.Log(getItemAt(cellPosition));
            // if it's a valid and filled cell, destroy the prop
            if(map.GetSprite(cellPosition) is not null  && getItemAt(cellPosition)){
                deleteItemAt(cellPosition);

                Debug.DrawRay(grid.CellToWorld(cellPosition),Vector3.one,Color.blue,3f);
            }

            Debug.Log(cellPosition);
        }
    }
}

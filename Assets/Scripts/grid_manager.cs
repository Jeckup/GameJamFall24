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
    private GridLayout grid;

    public Dictionary<Vector3Int,ClickableTile> slots;
    public ClickableTile[] prefilled_tiles;
    public ClickableTile ctile_template;

    void Start()
    {
        grid = map.layoutGrid;
        
        // the shape of the level is not guaranteed to be perfectly rectangular, so a dictionary is better
        slots = new Dictionary<Vector3Int, ClickableTile>();

        foreach(ClickableTile t in prefilled_tiles){
            slots.Add(t.gridLocation, t);
            Debug.Log("slot added to " + t.gridLocation);
        }

        for(int i = area.min.x; i < area.max.x; i++){
            for(int j = area.min.y; j < area.max.y; j++){
                Vector3Int pos = new Vector3Int(i,j);
                // this makes sure to ignore tiles that are not rendered. In other words, illegal locations
                if(map.GetSprite(pos) == null || slots.ContainsKey(pos))
                    continue;
                Vector3 cell_center = map.GetCellCenterWorld(pos);
                cell_center.z-=2;

                ClickableTile ctile = Instantiate(ctile_template,cell_center, Quaternion.identity);
                slots[pos] = ctile;
            }
        }
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

            // if it's a valid and empty cell, create an object
            if(slots.ContainsKey(cellPosition) && slots[cellPosition].Prop == null){
                ClickableTile targetTile = slots[cellPosition];

                targetTile.Prop = ctile_template.Prop;

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

            // if it's a valid and filled cell, destroy the prop
            if(slots.ContainsKey(cellPosition) && slots[cellPosition].Prop != null){
                ClickableTile targetTile = slots[cellPosition];

                targetTile.Prop = null;

                Debug.DrawRay(grid.CellToWorld(cellPosition),Vector3.one,Color.blue,3f);
            }

            Debug.Log(cellPosition);
        }
    }
}

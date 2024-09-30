using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickableTile : MonoBehaviour
{
    // Right now, tiles and boxes are separate objects in the Unity editor. But do they really need to be?

    public Tilemap map;

    [SerializeField] private Prop prop; 
    [DoNotSerialize] public Vector3Int gridLocation; // currently, this is only used for when the item is already on the map. Otherwise, don't touch this

    void Start()
    {
        if (map != null)
        {
            gridLocation = map.WorldToCell(transform.position);
            // in case the initial placement is slightly offcenter
            Vector3 cell_center = map.GetCellCenterWorld(gridLocation); 
            cell_center.z-=2;
            transform.position = cell_center;
        }
    }

    private void OnMouseUp()
    {
        // left click - get info from selected tile
        if (Input.GetMouseButtonUp(0))
        {
            // get mouse click's position in 2d plane
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;

            // convert mouse click's position to Grid position
            Vector3Int cellPosition = map.WorldToCell(pz);

            // set selectedUnit to clicked location on grid
            // Instantiate(prop,cellPosition.ConvertTo(Vector3));
            Debug.Log(cellPosition);
        }
    }
    public Prop Prop{
        get{ return prop; }
        set{ 
            if (prop != null){
                Destroy(prop.gameObject);
                Debug.Log("destroyed prop");
            }

            if (value == null){
                prop = null;
            }else{
                prop = Instantiate(value,transform); 
                Debug.Log("set prop to " + prop);
            }
        }
    }
}
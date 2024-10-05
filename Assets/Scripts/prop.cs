using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Prop : MonoBehaviour
{
    // placement logic
    private Tilemap Map;
    private Vector3Int gridLocation;
    // this holds the original shapee. I tried to make this constant but i have no idea what im doing and its 1 AM
    [SerializeField] private List<Vector2Int> shape = new List<Vector2Int>(); 
    // theres probably a way to do this without storing the original and current orientation vectors, but i am cooked rn
    [field: NonSerialized] public List<Vector2Int> orientedShape;
    [field: NonSerialized] public SpriteRenderer spriteRenderer{get;private set;}

    // Stuff for score logic
    public int score = 0;

    void Start()
    {
        Map = SingletonMap.Instance.Map;
        // this code will porobably break if fucked with. so like, don't touch it?
        spriteRenderer = GetComponent<SpriteRenderer>();

        gridLocation = Map.WorldToCell(transform.position);
        // in case the initial placement is slightly offcenter
        Vector3 cell_center = Map.GetCellCenterWorld(gridLocation); 
        // cell_center.z-=2;
        transform.position = cell_center;

        // in case the rotation by default is not 0
        int num_rotations = (int) transform.rotation.eulerAngles.z % 360;
        num_rotations = (num_rotations > 0) ? 360 - num_rotations : -num_rotations;
        num_rotations /= 90;
        transform.rotation = Quaternion.identity;
        orientedShape = new List<Vector2Int>(shape);
        for(int i = 0; i < num_rotations;i++){
            rotate_90();
        }
    }

    /// <summary>
    /// A pre-defined rotation of 90 degrees. This is important because the <shape> needs to change too, so that hitbox/cell checking
    /// can be done dynamically
    /// </summary>
    public void rotate_90(){
        transform.Rotate(0,0,-90); // rotate the image

        // rotate the shape list. This makes sure that checking cells works no matter the orientation 
        for(int i = 0; i < orientedShape.Count; i++){
            Vector2Int cell = orientedShape[i];
            // to understand more of whats happening here, look up rotation matrices
            int new_x = cell.y;
            int new_y = -cell.x;
            Vector2Int rotated_cell = new Vector2Int(new_x,new_y);
            orientedShape[i] = rotated_cell;
        }
    }

    public Vector3Int GridLocation{
        get{return gridLocation;}
        set{
            gridLocation = value;
            // Vector3 cell_center = Map.GetCellCenterWorld(gridLocation); 
            transform.position = Map.GetCellCenterWorld(gridLocation);
        }
    }

    // this may be more computationally efficient. Instead of updating every frame, only check on mouse click. But eh, whatever.
    // private void OnMouseUp()
    // {
    //     // left click - get info from selected tile
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         // get mouse click's position in 2d plane
    //         Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         pz.z = 0;

    //         // convert mouse click's position to Grid position
    //         Vector3Int cellPosition = map.WorldToCell(pz);

    //         // set selectedUnit to clicked location on grid
    //         Debug.Log("hi" + cellPosition);
    //     }
    // }
}
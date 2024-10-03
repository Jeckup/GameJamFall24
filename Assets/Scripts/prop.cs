using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Prop : MonoBehaviour
{
    // placement logic
    public Tilemap map;
    private Vector3Int gridLocation;
    public Vector2Int[] shape = {Vector2Int.zero};
    public SpriteRenderer spriteRenderer;

    // Stuff for score logic
    public int score = 0;

    void Start()
    {
        // this code will porobably break if fucked with. so like, don't touch it?
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        spriteRenderer = sprites[sprites.Length-1];


        if (map != null)
        {
            gridLocation = map.WorldToCell(transform.position);
            // in case the initial placement is slightly offcenter
            Vector3 cell_center = map.GetCellCenterWorld(gridLocation); 
            // cell_center.z-=2;
            transform.position = cell_center;

            // in case the rotation by default is not 0
            int num_rotations = (int) transform.rotation.eulerAngles.z % 360;
            num_rotations = (num_rotations > 0) ? 360 - num_rotations : num_rotations;
            num_rotations /= 90;
            transform.rotation = Quaternion.identity;
            for(int i = 0; i < num_rotations;i++){
                rotate_90();
            }
        }
    }

    /// <summary>
    /// A pre-defined rotation of 90 degrees. This is important because the <shape> needs to change too, so that hitbox/cell checking
    /// can be done dynamically
    /// </summary>
    public void rotate_90(){
        transform.Rotate(0,0,-90); // rotate the image

        // rotate the shape list. This makes sure that checking cells works no matter the orientation 
        for(int i = 0; i < shape.Length; i++){
            Vector2Int cell = shape[i];
            // to understand more of whats happening here, look up rotation matrices
            int new_x = cell.y;
            int new_y = -cell.x;
            Vector2Int rotated_cell = new Vector2Int(new_x,new_y);
            shape[i] = rotated_cell;
        }
    }

    public Vector3Int GridLocation{
        get{return gridLocation;}
        set{
            gridLocation = value;
            Vector3 cell_center = map.GetCellCenterWorld(gridLocation); 
            // cell_center.z-=2;
            transform.position = cell_center;
        }
    }

    // this may be more computationally efficient. Instead of updating every frame, only check on mouse click. But eh, whatever.
    private void OnMouseUp()
    {
        return; // For now, just don't do anything
        // left click - get info from selected tile
        if (Input.GetMouseButtonUp(0))
        {
            // get mouse click's position in 2d plane
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;

            // convert mouse click's position to Grid position
            Vector3Int cellPosition = map.WorldToCell(pz);

            // set selectedUnit to clicked location on grid
            Debug.Log("hi" + cellPosition);
        }
    }
}
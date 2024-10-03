using UnityEngine;
using UnityEngine.Tilemaps;

public class Prop : MonoBehaviour
{
    // placement logic
    public Tilemap map;
    public Vector3Int gridLocation;
    public Vector2Int[] shape = {Vector2Int.zero};

    // Stuff for score logic
    public int score = 0;

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

    // this may be more computationally efficient. Instead of updating every frame, only check on mouse click. But eh, whatever.
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
            Debug.Log("hi" + cellPosition);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_rotate : MonoBehaviour
{
    public Prop prop;

    // Update is called once per frame
    void Update()
    {
        // set prop to mouse position
        // get mouse click's position in 2d plane
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;

        // convert mouse click's position to Grid position
        Vector3Int cellPosition = prop.map.WorldToCell(pz);
        prop.GridLocation = cellPosition;

        // rotate
        if (Input.GetButtonDown("Rotate")){
            prop.rotate_90();
            foreach(Vector2Int cell in prop.shape){
                Debug.Log(cell);
            }
        }
    }
}

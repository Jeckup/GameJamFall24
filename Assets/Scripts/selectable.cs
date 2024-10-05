using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class selectable : MonoBehaviour
{
    private Prop prop;
    private SpriteRenderer sp;
    private GridManager gm;
    public Color highlight_color = new Color(255f,253f,160f,255f);
    void Start()
    {
        gm = GridManager.gm;
        prop = GetComponent<Prop>();
        sp = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter(){
        sp.color = highlight_color;
    }

    void OnMouseExit(){
        sp.color = Color.white;
    }

    void OnMouseUp(){
        if (Input.GetMouseButtonUp(0))
        {
            // set selectedUnit to clicked location on grid
            gm.SelectedProp = prop;
        }
    }
}

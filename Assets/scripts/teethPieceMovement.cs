using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class teethPieceMovement : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Camera mainCam;
    public bool isThere;
    public bool gotime;
    public bool inCorrectPositon;
    Vector2 correctPosition;
    public void init(Vector2 correctPosition){
        isThere=false;
        this.correctPosition=correctPosition;
        inCorrectPositon=false;
        gotime=true;
    }

    // Update is called once per frame
    void Update(){
        if(gotime){
            if(!inCorrectPositon){
            if(almostThere(transform.position,destination.position,0.1f)) {
                isThere=true;
                gotime=false;
            }
            else transform.position=Vector2.Lerp(transform.position,destination.position,Time.deltaTime*10f);
            }
        }
    }

    bool almostThere(Vector2 p1, Vector2 d,float threshold){ //p=pos1, d=destination
        return math.abs(d.y-p1.y)<threshold&&math.abs(d.x-p1.x)<threshold;
    }

     void OnMouseDrag(){
        if(!inCorrectPositon){
        gotime=false;
        Vector2 mousePos=Input.mousePosition;
        Vector2 screenPoint=mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCam.nearClipPlane));
        transform.position=screenPoint;
        if(almostThere(transform.position,correctPosition,0.05f)) {
            inCorrectPositon=true;
            transform.position=correctPosition;

        }
    }}
    void OnMouseUp(){
        gotime=true;
        //Debug.Log("done");
    }
}

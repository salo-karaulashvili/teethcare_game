using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class teethPieceMovement : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] Camera mainCam;
    public bool isThere;
    public bool gotime,incorrectAnimation1,incorrectAnimation2;
    public bool inCorrectPositon;
    Vector2 correctPosition;
    Quaternion dest1,dest2;
    public void init(Vector2 correctPosition){
        isThere=false;
        this.correctPosition=correctPosition;
        inCorrectPositon=false;
        gotime=true;
        incorrectAnimation1=false;
        dest1=new Quaternion(0,0,-0.0871557817f,0.99619472f);
        dest2=new Quaternion(0,0,0.0871557817f,0.99619472f);
    }

    // Update is called once per frame
    void Update(){
        if(gotime){
            if(!inCorrectPositon){
            if(almostThere(transform.position,destination.position,0.1f)) {
                isThere=true;
                gotime=false;
                GetComponent<Collider2D>().enabled=true;
            }
            else transform.position=Vector2.Lerp(transform.position,destination.position,Time.deltaTime*10f);
            }
        }else if(incorrectAnimation1){
            transform.rotation=Quaternion.Lerp(transform.rotation,dest1,Time.deltaTime*20f);
            if(almostRotated(transform.rotation,dest1)) {
                incorrectAnimation1=false;
                incorrectAnimation2=true;
            }
        }
        else if(incorrectAnimation2){
            transform.rotation=Quaternion.Lerp(transform.rotation,dest2,Time.deltaTime*20f);
            if(almostRotated(transform.rotation,dest2)) {
                incorrectAnimation1=false;
                incorrectAnimation2=false;
                transform.rotation=new Quaternion(0,0,0,1);
                GetComponent<Collider2D>().enabled=true;
                gotime=true;
            }
        }
    }

    private bool almostRotated(Quaternion r, Quaternion d ){
        return math.abs(r.x-d.x)<0.0001&&math.abs(r.y-d.y)<0.0001&&math.abs(r.z-d.z)<0.0001&&math.abs(r.w-d.w)<0.0001;
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
        if(almostThere(transform.position,correctPosition,0.1f)) {
            inCorrectPositon=true;
            transform.position=correctPosition;
        }
    }}
    void OnMouseUp(){
        if(!inCorrectPositon){
            incorrectAnimation1=true;
            GetComponent<Collider2D>().enabled=false;
        }
        else gotime=true;
    }
}

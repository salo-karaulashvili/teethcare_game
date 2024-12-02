using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class toothbrushScript : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    private float timer=0f;
    public int cleanteeth=0;
    void OnMouseDrag(){
        Vector2 mousePos=Input.mousePosition;
        Vector2 screenPoint=mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCam.nearClipPlane));
        transform.position=screenPoint;
    }
    private void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag=="lowertooth") GetComponent<SpriteRenderer>().flipY=true;
        else GetComponent<SpriteRenderer>().flipY=false;
        SpriteResolver sp=other.GetComponent<SpriteResolver>();
        if(sp!=null){
        if(timer>2f&&!other.GetComponent<SpriteResolver>().GetLabel().Contains("clean")){
            //other.gameObject.SetActive(false);
            //Debug.Log(other.GetComponent<SpriteResolver>().GetLabel());
            if(other.GetComponent<SpriteResolver>().GetLabel().Contains("broken")){
                other.GetComponent<SpriteResolver>().SetCategoryAndLabel("textures","broken_clean");
            }else{
                other.GetComponent<SpriteResolver>().SetCategoryAndLabel("textures","clean");
                other.GetComponentInChildren<Animator>().SetTrigger("happy");
            }
            timer=0;
            cleanteeth++;
            Transform foamChild=other.gameObject.transform.Find("Foam").transform.GetChild(0);
            var main1=foamChild.GetComponent<ParticleSystem>().main;
            var main2=foamChild.GetChild(0).GetComponent<ParticleSystem>().main;
            main1.loop=false;
            main2.loop=false;
            main1.startLifetime=0.5f;
            main2.startLifetime=0.5f;
        }
        else{
            timer+=Time.deltaTime;
            //SpriteResolver sp=other.GetComponent<SpriteResolver>();
            //if(sp!=null){
                if(!sp.GetLabel().Contains("clean")) {
                    other.gameObject.transform.Find("Foam").gameObject.SetActive(true);
                }
           // }
        } 
    }}
}

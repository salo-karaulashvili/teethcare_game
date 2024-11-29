using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class bacteriaManager : MonoBehaviour
{
    private bool neg;
    private bool stop;
    public bool gameOn;
    private float totaladded;
    private int index;
    public int deadBacteriaCount;
    public void init(int index)
    {
        neg=index<4;
        gameOn=true;
        gameObject.SetActive(true);
        totaladded=0;
        this.index=index;
        stop=false;
             Vector2 scale=transform.localScale;
        if(neg){
             scale.y=-1;
             scale.x=-1;
        }else{
             scale.y=1;
                scale.x=1;
        }
             transform.localScale=scale;
        //int skinNum=UnityEngine.Random.Range(0,3);
//        GetComponent<SpriteResolver>().SetCategoryAndLabel("textures",skinNum+"");
    }

    void OnMouseDown()
    {
        gameOn=false;
        gameObject.SetActive(false);
        deadBacteriaCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOn){
           /* if(!stop){
                checkOrientation();
                Vector2 pos=transform.localPosition;
                if(neg) {
                    float delta=1f*Time.deltaTime;
                    pos.y-=delta;
                    totaladded-=delta;
                }
                else {
                    float delta=1f*Time.deltaTime;
                    pos.y+=delta;
                    totaladded+=delta;
                }
                transform.localPosition=pos;
            }*/
        }
        
    }

    private void checkOrientation()
    {
        if(index<4){
            if(totaladded<-1.5f) {
                neg=false;
                stop=true;
                Invoke("unstop",5f);

            }
            if(totaladded>0) {
                neg=true;
                stop=true;
                gameOn=false;
            }
        }else{
            if(totaladded>1.5f) {
                neg=true;
                stop=true;
                Invoke("unstop",5f);
            }
            if(totaladded<0) {
                neg=false;
                stop=true;
                gameOn=false;
            }
        }
    }
    private void unstop(){stop=false;}
}

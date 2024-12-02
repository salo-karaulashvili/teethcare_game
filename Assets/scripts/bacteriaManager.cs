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
    private static List<String> trigNames=new List<string>{"orange","green","pink","purple"};
    private static List<String> trigNamesDeath=new List<string>{"odie","gdie","pdie","pudie"};
    private int deathAnimIndx;
    public void init(int index)
    {
        gameObject.SetActive(true);
        neg=index<4;
        gameOn=true;
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
        int skinNum=UnityEngine.Random.Range(0,4);
        transform.GetComponent<Animator>().SetTrigger(trigNames[skinNum]);
        deathAnimIndx=skinNum;
    }

    void OnMouseDown()
    {
        Invoke("falseGameon",1f);
        transform.GetComponent<Animator>().SetTrigger(trigNamesDeath[deathAnimIndx]);
        deadBacteriaCount++;
    }
    void falseGameon(){gameOn=false;}

    // Update is called once per frame

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

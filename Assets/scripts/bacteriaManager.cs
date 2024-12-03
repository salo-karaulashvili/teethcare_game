using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class bacteriaManager : MonoBehaviour
{
    private static float IDLE_TIME=5f;
    private bool neg;
    public bool gameOn;
    private float totaladded;
    public int deadBacteriaCount;
    private static List<String> trigNames=new List<string>{"orange","green","pink","purple"};
    private static List<String> trigNamesDeath=new List<string>{"odie","gdie","pdie","pudie"};
    private int deathAnimIndx;
    public void init(int index,int length){
        gameObject.SetActive(true);
        if(length%2==0) neg=index<(length/2);
        else neg=index<((length+1)/2);
        gameOn=true;
        totaladded=0;
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
    void Update(){
        if(gameOn){
        totaladded+=Time.deltaTime;
        if(totaladded>IDLE_TIME){
            transform.GetComponent<Animator>().SetTrigger(trigNamesDeath[deathAnimIndx]);
            Invoke("falseGameon",1f);
            totaladded=0f;
        }
        }else totaladded=0;
    }

    void OnMouseDown()
    {
        Invoke("falseGameon",1f);
        transform.GetComponent<Animator>().SetTrigger(trigNamesDeath[deathAnimIndx]);
        deadBacteriaCount++;
    }
    void falseGameon(){gameOn=false;}
}

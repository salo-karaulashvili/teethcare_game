using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class gameController : MonoBehaviour
{
    [SerializeField] GameObject[] humanTeeth,humanToothPieces;
    [SerializeField] GameObject bacteria,toothbrush,background;
    [SerializeField] GameObject happyFacehuman,humanMoutharea,humanBrokenToothArea;
    [SerializeField] Camera maincam;
    [SerializeField] GameObject[] crocTeeth,crocToothPieces;
    [SerializeField] GameObject crocMoutharea,crocBrokenToothArea,crocHappyFace;
    [SerializeField] GameObject[] sharkTeeth,sharkToothPieces;
    [SerializeField] GameObject sharkMouthArea,sharkBrokenToothArea,sharkHappyFace;
    private List<Transform> teethCorrectPositions;
    private GameObject curBacteria,curTooth;
    private Vector2 ToothBrushInitPosition;
    private int index;
    private int brokenTeethIndex;
    private bool human,crocodile,shark;
    private GameObject[] teeth,toothPieces;
    private GameObject happyFace,brokenToothArea,mouthArea;
    private Color32 backColor;
    void Start(){
        teethCorrectPositions=new List<Transform>();
        if(human){
            backColor=new Color32(255,204,171,255);
            teeth=humanTeeth;
            for(int i=0;i<humanToothPieces.Length;i++){teethCorrectPositions.Add(humanToothPieces[i].gameObject.transform);}
            brokenTeethIndex=7;
            toothPieces=humanToothPieces;
            happyFace=happyFacehuman;
            mouthArea=humanMoutharea;
            brokenToothArea=humanBrokenToothArea;
        }
        else if (crocodile){
           // curBacteria.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
            backColor=new Color32(79,131,62,255);
            teeth=crocTeeth;
            for(int i=0;i<crocToothPieces.Length;i++){teethCorrectPositions.Add(crocToothPieces[i].gameObject.transform);}
            brokenTeethIndex=8;
            toothPieces=crocToothPieces;
            happyFace=crocHappyFace;
            mouthArea=crocMoutharea;
            brokenToothArea=crocBrokenToothArea;
        }
        else if(shark||true){
            backColor=new Color32(68,127,195,255);
            teeth=sharkTeeth;
            for(int i=0;i<sharkToothPieces.Length;i++){teethCorrectPositions.Add(sharkToothPieces[i].gameObject.transform);}
            brokenTeethIndex=1;
            toothPieces=sharkToothPieces;
            happyFace=sharkHappyFace;
            mouthArea=sharkMouthArea;
            brokenToothArea=sharkBrokenToothArea;
        }
        curBacteria=bacteria;
        spawnBacteria();
        ToothBrushInitPosition=toothbrush.transform.position;
        curTooth=null;
        index=0;
    }

    // Update is called once per frame
    void Update(){
        if(!curBacteria.GetComponent<bacteriaManager>().gameOn&&curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount<6){
            spawnBacteria();
        }
        else if(!curBacteria.GetComponent<bacteriaManager>().gameOn&&curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount==6){
            toothbrush.gameObject.SetActive(true);
            curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount++;
        }
        if(toothbrush.GetComponent<toothbrushScript>().cleanteeth==teeth.Length){
            toothbrush.GetComponent<Collider2D>().enabled=false;
            toothbrush.transform.position=Vector2.Lerp(toothbrush.transform.position,ToothBrushInitPosition,2f*Time.deltaTime);
            if(almostThere(toothbrush.transform.position,ToothBrushInitPosition,0.2f)){
                toothbrush.GetComponent<toothbrushScript>().cleanteeth++;
                toothbrush.transform.position=ToothBrushInitPosition;
                Invoke("repairBrokenTooth",5f);//aq xma ismis romelic instruqcias idzleva
            }
        }
        if(curTooth!=null&&index<5){
            if(curTooth.GetComponent<teethPieceMovement>().isThere){
                curTooth=toothPieces[index];
                curTooth.GetComponent<teethPieceMovement>().init(teethCorrectPositions[index].position);
                index++;
            }
        }else if(index==5){
            bool correct=true;
            for(int i=0;i<toothPieces.Length;i++){
                correct=correct&&toothPieces[i].GetComponent<teethPieceMovement>().inCorrectPositon;
            }
            if(correct) {
                index++;
                happyFace.gameObject.SetActive(true);
                Invoke("showPrettyTeeth",2f);
            }
        }
    }

    private void showPrettyTeeth(){
        happyFace.gameObject.SetActive(false);
        brokenToothArea.gameObject.SetActive(false);
        maincam.backgroundColor=backColor;
        background.GetComponent<SpriteRenderer>().color=backColor;
        mouthArea.gameObject.SetActive(true);
        teeth[brokenTeethIndex].GetComponent<SpriteResolver>().SetCategoryAndLabel("textures","clean");
        for(int i=0;i<teeth.Length;i++){
            teeth[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            teeth[i].GetComponentInChildren<Animator>().SetTrigger("happy");
        }
    }

    bool almostThere(Vector2 p1, Vector2 d,float threshold){ //p=pos1, d=destination
        return math.abs(d.y-p1.y)<threshold&&math.abs(d.x-p1.x)<threshold;
    }


    private void repairBrokenTooth(){
        toothbrush.gameObject.SetActive(false);
        mouthArea.SetActive(false);
        maincam.backgroundColor=new Color32(95,156,246,255);
        background.GetComponent<SpriteRenderer>().color=new Color32(95,156,246,255);
        brokenToothArea.gameObject.SetActive(true);
        curTooth=toothPieces[index];
        Invoke("movePieces",2f);

    }
    void movePieces(){
        curTooth.GetComponent<teethPieceMovement>().init(teethCorrectPositions[index].position);
        index++;
    }

    private void spawnBacteria(){
        int teethnum=UnityEngine.Random.Range(0,teeth.Length);
        if(crocodile||true){
            while(new List<int>{0,5,6,11}.Contains(teethnum)) teethnum=UnityEngine.Random.Range(0,teeth.Length);
        }
        Quaternion rot=teeth[teethnum].transform.rotation;
        curBacteria.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
        curBacteria.transform.parent=teeth[teethnum].transform;
        curBacteria.transform.localPosition=new Vector2(0,0);
        curBacteria.transform.rotation=rot;
        curBacteria.GetComponent<bacteriaManager>().init(teethnum,teeth.Length);
    }
}

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class gameController : MonoBehaviour
{
    private List<int> bacteriaSpawnPositions;
    [SerializeField] GameObject[] teeth,toothPieces;
    [SerializeField] GameObject bacteria;
    [SerializeField] GameObject toothbrush;
    [SerializeField] GameObject moutharea,brokenToothArea;
    [SerializeField] GameObject background;
    private List<Transform> teethCorrectPositions;
    private GameObject curBacteria;
    private int bacteriaCount;
    private Vector2 ToothBrushInitPosition;
    private GameObject curTooth;
    private int index;
    private int brokenTeethIndex=7;
    void Start(){
        bacteriaCount=5;
        bacteriaSpawnPositions=new List<int> {0, 6,3,7,2};
        curBacteria=bacteria;
        spawnBacteria();
        ToothBrushInitPosition=toothbrush.transform.position;
        teethCorrectPositions=new List<Transform>();
        for(int i=0;i<toothPieces.Length;i++){
            teethCorrectPositions.Add(toothPieces[i].gameObject.transform);
        }
        curTooth=null;
        index=0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!curBacteria.GetComponent<bacteriaManager>().gameOn&&curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount<6){
            spawnBacteria();
        }
        else if(!curBacteria.GetComponent<bacteriaManager>().gameOn&&curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount==6){
            toothbrush.gameObject.SetActive(true);
            curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount++;
        }
        if(toothbrush.GetComponent<toothbrushScript>().cleanteeth==8){
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
                Invoke("showPrettyTeeth",2f);
            }
        }
    }

    private void showPrettyTeeth(){
        brokenToothArea.gameObject.SetActive(false);
        background.GetComponent<SpriteRenderer>().color=new Color32(255,204,171,255);
        moutharea.gameObject.SetActive(true);
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
        teeth[brokenTeethIndex].GetComponentInChildren<Animator>().SetTrigger("happy");
        moutharea.SetActive(false);
        background.GetComponent<SpriteRenderer>().color=new Color32(95,156,246,255);
        brokenToothArea.gameObject.SetActive(true);
        curTooth=toothPieces[index];
        Invoke("movePieces",2f);

    }
    void movePieces(){
        curTooth.GetComponent<teethPieceMovement>().init(teethCorrectPositions[index].position);
        index++;
    }

    private void spawnBacteria()
    {
        int teethnum=UnityEngine.Random.Range(0,teeth.Length);
        Quaternion rot=teeth[teethnum].transform.rotation;
        Vector2 pos=new Vector2(0,0);
        curBacteria.transform.parent=teeth[teethnum].transform;
        curBacteria.transform.localPosition=new Vector2(0,0);
        curBacteria.transform.rotation=rot;
        bacteriaCount--;
        curBacteria.GetComponent<bacteriaManager>().init(teethnum);
    }
}

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private List<int> bacteriaSpawnPositions;
    [SerializeField] GameObject[] teeth;
    [SerializeField] GameObject bacteria;
    [SerializeField] GameObject toothbrush;
    [SerializeField] GameObject moutharea;
    [SerializeField] GameObject background;
    private GameObject curBacteria;
    private int bacteriaCount;
    private Tuple<int, int> brokenteethIndexes = new Tuple<int,int>( 0, 7 );
    void Start(){
        bacteriaCount=5;
        bacteriaSpawnPositions=new List<int> {0, 6,3,7,2};
        curBacteria=Instantiate(bacteria);
        curBacteria.transform.localScale=new Vector3(curBacteria.transform.localScale.x/3f,curBacteria.transform.localScale.y/3f,curBacteria.transform.localScale.z);
        spawnBacteria();
    }

    // Update is called once per frame
    void Update()
    {
        if(!curBacteria.GetComponent<bacteriaManager>().gameOn&&curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount!=5){
            spawnBacteria();
        }
        else if(curBacteria.GetComponent<bacteriaManager>().deadBacteriaCount==5){
            toothbrush.gameObject.SetActive(true);
        }
        if(toothbrush.GetComponent<toothbrushScript>().cleanteeth==8){
            toothbrush.GetComponent<toothbrushScript>().cleanteeth++;
            repairBrokenTooth();
        }
    }


    private void repairBrokenTooth(){
        //toothbrush.SetActive(false);
        //moutharea.SetActive(false);
        //background.GetComponent<SpriteRenderer>().color=new Color32()

    }

    private void spawnBacteria()
    {
        //int teethnum=bacteriaSpawnPositions[bacteriaCount-1];
        int teethnum=UnityEngine.Random.Range(0,teeth.Length);
        Quaternion rot=teeth[teethnum].transform.rotation;
        Vector2 pos=teeth[teethnum].transform.position;
        curBacteria.transform.parent=teeth[teethnum].transform.parent;
        curBacteria.transform.position=pos;
        curBacteria.transform.rotation=rot;
        bacteriaCount--;
        curBacteria.GetComponent<bacteriaManager>().init(teethnum);
    }
}

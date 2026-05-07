using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public GameObject CameraObject;


    private  charactor charactorClass;
    private EnemyAgentSight enemyClass;

    private bool StartFlag = false;
    public GameObject OperateKey;
    public GameObject[] StartText = new GameObject[2];

    public GameObject VictoryImg;
    public GameObject DefeatImg;

    private bool flag;//0:èüóòÅ@1 îsñk

    private float timer;

    // Update is called once per frame
    private void Start()
    {
        VictoryImg.SetActive(false);
        DefeatImg.SetActive(false);

        charactorClass = Player.GetComponent<charactor>();
        enemyClass = Enemy.GetComponent<EnemyAgentSight>();

        CameraObject.GetComponent<Camera>().enabled = false;

        charactorClass.enabled = false;
        enemyClass.enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            charactorClass.enabled = true;
            enemyClass.enabled = true;
            CameraObject.GetComponent<Camera>().enabled = true;
            OperateKey.SetActive(false);
            StartText[0].SetActive(false);
            StartText[1].SetActive(false);
        }

        if(charactorClass.HP <= 0)
        {
            timer += Time.deltaTime;
            flag = false;

        }
        if(enemyClass.HP <= 0)
        {
            timer += Time.deltaTime;
            flag = true;

        }


        if(timer >= 3.0f)
        {
            if (flag)
            {
                VictoryImg.SetActive(true);
                charactorClass.enabled = false;
                enemyClass.enabled = false;
                Destroy(Player.GetComponent<Rigidbody>());
                Destroy(Enemy.GetComponent<Rigidbody>());
            }
            else
            {
                DefeatImg.SetActive(true);
                charactorClass.enabled = false;
                enemyClass.enabled = false;
                Destroy(Player.GetComponent<Rigidbody>());
                Destroy(Enemy.GetComponent<Rigidbody>());
            }
        }

    }
}

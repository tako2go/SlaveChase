using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMapItem : MonoBehaviour
{
    public GameObject treasureBox;
    GameObject[] treasureBoxArr = new GameObject[ItemNum];

    const int ItemPosArrSize = 10;
    const int ItemNum = 3;
    
    bool OverLapFlag = false;
    Vector3[] ItemSpornPos = new Vector3[ItemPosArrSize];
    int[] BoxPos = new int[ItemNum];

    // Start is called before the first frame update
    void Start()
    {
        ItemSpornPos[0] = new Vector3(50, 0.5f, 96);
        ItemSpornPos[1] = new Vector3(43, 0.5f, 75);
        ItemSpornPos[2] = new Vector3(44, 0.5f, -32);
        ItemSpornPos[3] = new Vector3(68, 0.5f, -67);
        ItemSpornPos[4] = new Vector3(88, 0.5f, -32);
        ItemSpornPos[5] = new Vector3(-25, 0.4f, -23);
        ItemSpornPos[6] = new Vector3(-39, 2, -58);
        ItemSpornPos[7] = new Vector3(-47, 2.5f, -73);
        ItemSpornPos[8] = new Vector3(-47, 0.5f, 60);
        ItemSpornPos[9] = new Vector3(-70, 2.3f, 71);

        for (int i = 0; i <= ItemNum - 1; i++)
        {
            do
            {
                OverLapFlag = false;
                BoxPos[i] = Random.Range(0, 10);
                for (int j = i; j > 0; j--)//èdï°îrèú
                {
                    if (BoxPos[i] == BoxPos[j - 1])//â∫Ç…1Ç¬Ç∏Ç¬Ç∏ÇÁÇµÇ»Ç™ÇÁèdï°ä«óùÇ∑ÇÈ
                    {
                        OverLapFlag = true;
                    }

                }
            } while (OverLapFlag == true);
        }


        for (int i = 0;i <= ItemNum - 1; i++)
        {
            treasureBoxArr[i] = Instantiate(treasureBox, ItemSpornPos[BoxPos[i]], Quaternion.Euler(0, Random.Range(0,360), 0));
            treasureBoxArr[i].GetComponent<TreasureBox>().BoxNumber = i;
        }



        for (int i = 0; i <= ItemNum - 1; i++)
        {
            //Debug.Log(BoxPos[i]);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rapierUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    charactor charactor;
    bool Flag = false;
    void Start()
    {
        charactor = Player.GetComponent<charactor>();
        GetComponent<RawImage>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Flag == false)
        {
            if (charactor.PlayerItem[2] == true)
            {
                GetComponent<RawImage>().enabled = true;
                Flag = true;
            }
        }
    }
}

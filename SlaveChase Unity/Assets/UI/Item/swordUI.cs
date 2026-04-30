using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swordUI : MonoBehaviour
{
    public GameObject Player;
    charactor charactor;
    bool Flag = false;
    // Start is called before the first frame update

    private void Start()
    {
        charactor = Player.GetComponent<charactor>();
        GetComponent<RawImage>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Flag == false)
        {
            if (charactor.PlayerItem[1] == true)
            {
                GetComponent<RawImage>().enabled = true;
                Flag = true;
            }
        }

    }
}

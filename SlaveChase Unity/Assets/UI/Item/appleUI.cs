using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class appleUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    charactor charactor;
    void Start()
    {
        charactor = Player.GetComponent<charactor>();
        GetComponent<RawImage>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
                GetComponent<RawImage>().enabled = charactor.PlayerItem[3] == true;
    }
   }

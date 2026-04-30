using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TreasureBox : MonoBehaviour
{
    public int BoxNumber;
    private Animator BoxAnimation;
    //public 
    void Start()
    {
        BoxAnimation = GetComponent<Animator>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           if (Input.GetKey(KeyCode.F))
            {
                GameObject.Find("Player").GetComponent<charactor>().PlayerItem[BoxNumber + 1] = true;//BoxNumberÇ0Ç©ÇÁçÏÇ¡ÇƒÇµÇ‹Ç¡ÇΩÇΩÇﬂ+1ÇÇ∑ÇÈ
                BoxAnimation.SetBool("Open",true);
                //ItemImage[BoxNumber].GetComponent<RawImage>().enabled = true;
                //Debug.Log(GameObject.Find("Player").GetComponent<charactor>().PlayerItem[BoxNumber]);
            }
        }

    }
}

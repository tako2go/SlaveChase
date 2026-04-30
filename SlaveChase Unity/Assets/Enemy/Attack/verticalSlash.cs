using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verticalSlash : MonoBehaviour
{
    public bool verticalCollisionFlag = false;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//verticalSlash‚ª“–‚½‚é”ÍˆÍ‚Åtrue
        {
            verticalCollisionFlag = true;
            Debug.Log("verticalCollisionFlag:true");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")//verticalSlash‚ª“–‚½‚ç‚È‚¢”ÍˆÍ‚Åfalse
        {
            verticalCollisionFlag = false;
            Debug.Log("verticalCollisionFlag:false");
        }
    }
}

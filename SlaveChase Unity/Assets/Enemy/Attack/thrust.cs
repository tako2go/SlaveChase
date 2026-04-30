using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrust : MonoBehaviour
{
    public bool thrustCollisionFlag = false;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//thrust‚ª“–‚½‚é”ÍˆÍ‚Åtrue
        {
            thrustCollisionFlag = true;
            Debug.Log("thrustCollisionFlag:true");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//thrust‚ª“–‚½‚ç‚È‚¢”ÍˆÍ‚Åfalse
        {
            thrustCollisionFlag = false;
            Debug.Log("thrustCollisionFlag:false");
        }
    }
}

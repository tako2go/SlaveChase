using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class besideSlash : MonoBehaviour
{
    public bool besideCollisionFlag = false;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//besideSlash‚ª“–‚½‚é”ÍˆÍ‚Åtrue
        {
            besideCollisionFlag = true;
            Debug.Log("besideCollisionFlag:true");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//besideSlash‚ª“–‚½‚ç‚È‚¢”ÍˆÍ‚Åfalse
        {
            besideCollisionFlag = false;
            Debug.Log("besideCollisionFlag:false");
        }
    }
}

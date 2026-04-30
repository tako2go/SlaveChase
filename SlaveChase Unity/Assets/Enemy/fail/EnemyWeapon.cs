using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public bool EWCollisionFlag = false;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//敵オブジェクトに剣が衝突した瞬間FlagをTrue
        {
            EWCollisionFlag = true;
        }
        //Debug.Log(collision.gameObject.tag);
    }

    void OnCollisionExit(Collision collision)//敵オブジェクトから剣が離れた瞬間FlagをFalse
    {
        if (collision.gameObject.tag == "Player")//敵オブジェクトに剣が衝突した瞬間FlagをTrue
        {
            EWCollisionFlag = false;
        }
    }
}

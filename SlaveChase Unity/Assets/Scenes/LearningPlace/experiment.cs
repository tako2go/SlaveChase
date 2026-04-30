using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experiment : MonoBehaviour
{
    // Start is called before the first frame update
    public  Transform Player;
    void Start()
    {
        Player.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 distance = (Player.localPosition - this.transform.localPosition).normalized;
        float Cos = Vector3.Dot(distance, this.transform.forward);

        Debug.Log(Cos);
    }
   
}

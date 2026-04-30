using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanceCreateSide : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fance;
    public Transform FanceTramsForm;
    public Transform parent;
    float fanceTransformVal;
    const float fanceSpan = -1.29f;
    // Start is called before the first frame update
    void Start()
    {
        fanceTransformVal = fance.transform.position.z;
        for (float i = 1; i < 25; i++)
        {
            Instantiate(fance, new Vector3(fance.transform.position.x, fance.transform.position.y, fanceTransformVal + (i * fanceSpan)), Quaternion.Euler(-90, 0, 180), parent);
        }
    }

}

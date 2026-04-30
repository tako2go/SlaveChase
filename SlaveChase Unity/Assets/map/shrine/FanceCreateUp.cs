using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanceCreateUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fance;
    public Transform FanceTramsForm;
    public Transform parent;
    float fanceTransformVal;
    const float fanceSpan = 1.29f;
    // Start is called before the first frame update
    void Start()
    {
        fanceTransformVal = fance.transform.position.x;
        for (float i = 1; i < 40; i++)
        {
            Instantiate(fance, new Vector3(fanceTransformVal + (i * fanceSpan), fance.transform.position.y, fance.transform.position.z), Quaternion.Euler(-90, 0, -90), parent);
        }
    }

}

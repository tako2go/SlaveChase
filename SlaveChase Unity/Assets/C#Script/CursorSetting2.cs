using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetting2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

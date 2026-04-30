using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPScript : MonoBehaviour
{
    //public GameObject Player;
   public charactor charactor;
   public Slider slider;
    // Start is called before the first frame update
    private void Start()
    {
        slider = GetComponent<Slider>();
    }


    // Update is called once per frame
    void Update()
    {
        slider.value = charactor.HP;
    }
}

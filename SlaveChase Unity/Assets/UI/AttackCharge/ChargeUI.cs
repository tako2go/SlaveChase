using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Attack Player;
    public Slider slider;
    // Start is called before the first frame update
    private void Start()
    {
        slider = GetComponent<Slider>();
    }


    // Update is called once per frame
    void Update()
    {
        slider.value = Player.ChargeingTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera : MonoBehaviour
{
    //?p?x?????
    float angleUp = 30f;
    float angleDown = -30f;

    [SerializeField] GameObject player;
    [SerializeField] Camera cam;

    //Axis???u???w??????
    [SerializeField] Vector3 axisPos;

    const float rotate_speed = 3;

    // Start is called before the first frame update
    void Start()
    {

        //Camera??Axis????ƒ¡I???u??localPosition??w??
        cam.transform.localPosition = new Vector3(0, -1, -2);
        //Camera??Axis????????????????????
        cam.transform.localRotation = transform.rotation;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
    }
   
    // Update is called once per frame
    void Update()
    {
        //PlayerPos = Player.transform.position;
        //MoveCamera();
        //CaneraRota();
        //Debug.Log(MouseInput);

        if(this.transform.eulerAngles.z != 0)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0);
        }

            transform.position = player.transform.position + axisPos;

        //Camera??p?x??}?E?X?????????l??????
        transform.eulerAngles += new Vector3(
            Input.GetAxis("MouseY") * -rotate_speed,
            Input.GetAxis("MouseX") * rotate_speed, 
            0);

        //X????p?x
        float angleX = transform.eulerAngles.x;

        //X????l??180?x????????360??????????????????????
        if (angleX >= 180)
        {
            angleX = angleX - 360;
        }

        //Mathf.Clamp(?l?A????l?A???l?j??X????l???????
        transform.eulerAngles = new Vector3(
            Mathf.Clamp(angleX, angleDown, angleUp),
            transform.eulerAngles.y,
            transform.eulerAngles.z);
    }


    //void MoveCamera()
    //{
    //    PlayerPosDifference = PlayerPos - KeepPlayerPos;
    //    this.transform.position = transform.position + PlayerPosDifference;
    //    KeepPlayerPos = PlayerPos;
    //}

    //void CaneraRota()
    //{
    //    MouseInputX = Input.GetAxis("MouseX") * CameraSpeed;//?J????????]???
    //    transform.RotateAround(PlayerPos, Vector3.up, MouseInputX * Time.deltaTime);
    //    MouseInputY = Input.GetAxis("MouseY") * CameraSpeed;
    //    transform.RotateAround(PlayerPos, Vector3.right, MouseInputY * Time.deltaTime); 
    //    //transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
    //}
}

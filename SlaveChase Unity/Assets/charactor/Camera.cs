using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera : MonoBehaviour
{
    //角度の制御
    float angleUp = 30f;
    float angleDown = -30f;

    [SerializeField] GameObject player;
    [SerializeField] Camera cam;

    //Axisの位置を指定する変数
    [SerializeField] Vector3 axisPos;

    const float rotate_speed = 3;

    // Start is called before the first frame update
    void Start()
    {

        //CameraのAxisに相対的な位置をlocalPositionで指定
        cam.transform.localPosition = new Vector3(0, -1, -2);
        //CameraとAxisの向きを最初だけそろえる
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

        //Cameraの角度にマウスからとった値を入れる
        transform.eulerAngles += new Vector3(
            Input.GetAxis("MouseY") * -rotate_speed,
            Input.GetAxis("MouseX") * rotate_speed, 
            0);

        //X軸の角度
        float angleX = transform.eulerAngles.x;

        //X軸の値を180度超えたら360引くことで制限しやすくする
        if (angleX >= 180)
        {
            angleX = angleX - 360;
        }

        //Mathf.Clamp(値、最小値、最大値）でX軸の値を制限する
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
    //    MouseInputX = Input.GetAxis("MouseX") * CameraSpeed;//カメラ横回転移動
    //    transform.RotateAround(PlayerPos, Vector3.up, MouseInputX * Time.deltaTime);
    //    MouseInputY = Input.GetAxis("MouseY") * CameraSpeed;
    //    transform.RotateAround(PlayerPos, Vector3.right, MouseInputY * Time.deltaTime); 
    //    //transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,0);
    //}
}

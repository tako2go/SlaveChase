using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class charactor : MonoBehaviour
{

    private const int Sword = 1;
    private const int Reypier = 2;
    private const int Apple = 3;
    // Start is called before the first frame update
    public Transform Camera;//向いている向きを取得
    private Rigidbody rb;
    SwordCollision swordCollision;
    //EnemyAgentAttack enemyAgent;

    //プレイヤーのパラメータ
    public int HP = 100;
    public float SP = 100;
    public bool[] PlayerItem = new bool[4];
    public int NowHave = 0;
    public GameObject[] Item = new GameObject[3];

    //y軸方向に関するもの
    public float UpDownSpeed = 0;
    private int Gravity = -20;//重力加速度

    //SPに関するもの
    const float RunDe = 20.0f; //SP decrease
    const float SPIn = 15.0f; //SP Increase


    //プレイヤーの速さに関するもの
    private int charactorSpeed;//歩く速さ
    const int normalCharaSpeed = 5;
    const int DashCharSpeed = 15;


    //基本的なフラグ
    private bool MoveFlag;
    private bool RunFlag;
    private bool SPEnableFlag = false;
    private float SPHealTime = 0;

    private bool LeftMouseFlag = false;
   
    private bool AttackFlag = false;
    //private const float AttackTime = 1.0f;
    //private float AttackTimePro = 0;


    //回避に関するもの
    private bool RightStepFlag = false;
    private bool LeftStepFlag = false;
    private bool BackStepFlag = false;
    private bool StepFlag = false;
    private float StepTimer = 0;
    private const int StepSpeed = 5;
    private const int StepDe = 15;

    //攻撃に関するもの
    public bool ActionFlag = false;
    public bool StanFlag = false;
    public float StanTimer;
    private const float StanTime = 0.2f;

    private bool DeadFlag = false;

    private Animator PlayerAnimation;

    private Attack attackSctipt;

    private Vector3[] PlayerPos = new Vector3[4];

    void Start()
    {
         rb = this.GetComponent<Rigidbody>();
        PlayerAnimation = this.GetComponent<Animator>();
        //swordCollision = GameObject.Find("SwordCollision").GetComponent<SwordCollision>();
        attackSctipt = this.GetComponent<Attack>();
        //enemyAgent = GameObject.Find("Enemy").GetComponent<EnemyAgentAttack>();

        for (int i = 0; i < 3; i++)
        {
            Item[i].SetActive(false);
        }
        PlayerPos[0] =new Vector3(-86, 5, -83);
        PlayerPos[1] = new Vector3(88, 5, -83);
        PlayerPos[2] = new Vector3(88, 5, 95);
        PlayerPos[3] = new Vector3(-86, 5, 95);
        this.transform.position = PlayerPos[Random.Range(0, PlayerPos.Length)];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackFlag = attackSctipt.AttackFlag;

        //if (!flyFlag && !AttackFlag && !StepFlag)
        //{
        //    ActionFlag = false;
        //}
        if (StanFlag)
        {
            StanTimer += Time.deltaTime;
            if (StanTimer >= StanTime)
            {
                StanFlag = false;
                PlayerAnimation.SetBool("StanFlag", false);
            }
        }
        if(HP <= 0)
        {
            DeadFlag = true;
            PlayerAnimation.SetBool("Dead", true);
        }
        Move();
        Jump();
        Fly();
        ItemChange();
        avoid();
        SPma();
        ActionFlag = flyFlag | AttackFlag | StepFlag | attackSctipt.ChargeFlag | StanFlag | DeadFlag;
    }

    void Move()
    {
        Vector3 PlayerVel = Vector3.zero;
         MoveFlag = false;


        if (Input.GetKey(KeyCode.W))
        {
            PlayerVel += Camera.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerVel += -Camera.transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerVel += Camera.transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerVel += -Camera.transform.right;
        }

        if (PlayerVel != Vector3.zero)
        {
            if(attackSctipt.AttackFlag == false)
            {
                MoveFlag = true;
            } 
        }
        //transform.Translate(MoveCharactor);
        //characterController.SimpleMove(MoveCharactor);
        //PlayerAnimation.SetBool("walk", MoveFlag);

        if (MoveFlag)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if(flyFlag == false)
                {
                    if (SPEnableFlag == true)
                    {
                        RunFlag = true;
                    }
                    else { 
                        RunFlag = false;
                    }
                }  
            }
            else
            {
                RunFlag = false;
            }
        }
        else
        {
            RunFlag = false;
        }



        if (RunFlag)
        {
            charactorSpeed = DashCharSpeed;
            SP -= Time.deltaTime * RunDe;
        }
        else
        {
            charactorSpeed = normalCharaSpeed;
        }

        if(MoveFlag && !RunFlag)
        {
            PlayerAnimation.SetBool("run", false);
            PlayerAnimation.SetBool("walk", true);
        }
        if(MoveFlag && RunFlag)
        {

            PlayerAnimation.SetBool("run", true);
            PlayerAnimation.SetBool("walk", false);
        }
        if (!MoveFlag)
        {
            PlayerAnimation.SetBool("run", false);
            PlayerAnimation.SetBool("walk", false);
        }



        if (ActionFlag && !flyFlag)//ジャンプ以外でアクションをしたら足を止める
        {
            MoveFlag = false;
            RunFlag = false;
        }




        if (MoveFlag)
        {
            PlayerVel = new Vector3 (PlayerVel.x, 0, PlayerVel.z);
            PlayerVel = PlayerVel.normalized;
            rb.velocity = PlayerVel * charactorSpeed + transform.up * UpDownSpeed;
            transform.rotation =  Quaternion.LookRotation(PlayerVel);
        }
        else
        {
            rb.velocity = transform.up * UpDownSpeed;
            //SP += Time.deltaTime * SPIn;
        }
    }
    private bool JumpFlag = false;
    private static float JumfirstSpeed = 6;
    void Jump()
    {
        
        if(Input.GetKey(KeyCode.Space) == true)
        {
            if(ActionFlag == false)
            {
                JumpFlag = true;
                UpDownSpeed = JumfirstSpeed;//初速度の追加
                this.transform.Translate(0,0.1f,0);//FlyFlagが一瞬でfalseになることを防止
                PlayerAnimation.SetBool("JumpFlag", true);
            }
        }
    }
    void Fly()
    {
        if (CurrentlyFlyFlag)//前回フレームがfalseならばFlyしているとみなす
        {
            flyFlag = false;
        }
        else
        {
            flyFlag = true;
        }
        
        CurrentlyFlyFlag = false;
        if (flyFlag)
        {
          UpDownSpeed += Gravity * Time.deltaTime;//加速度を足す
        }
        //ActionFlag |= flyFlag;
    }

    static public bool flyFlag = false;
    private bool CurrentlyFlyFlag = false;

    private void OnCollisionStay(Collision Map)
    {
        if (Map.gameObject.tag == "Map")
        {
           JumpFlag = false;//足がついているときはfalse
           CurrentlyFlyFlag = true;
           UpDownSpeed = 0;
           PlayerAnimation.SetBool("JumpFlag", false);
        }
    }

    private bool CollisionFirstFlag = false;
    //void attack()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        if(LeftMouseFlag == false)
    //        {
    //            if (!ActionFlag)
    //            {
    //                PlayerAnimation.SetBool("attack", true);
    //                AttackFlag = true;
    //            }
    //        }
    //        LeftMouseFlag = true; 
    //    }
    //    else
    //    {
    //        LeftMouseFlag = false;
    //    }

    //    if (AttackFlag)
    //    {
    //        AttackTimePro += Time.deltaTime;//攻撃し始めからの時間経過
    //        if (AttackTimePro >= AttackTime)//経過時間が設定時間を超えたら攻撃終了
    //        {
    //            AttackFlag = false;
    //            CollisionFirstFlag = false;
    //            AttackTimePro = 0;
    //            PlayerAnimation.SetBool("attack", false);
    //        }
    //    }

    //    if(AttackFlag == true)//攻撃モーション中に
    //    {
    //        if(swordCollision.SwordCollisionFlag == true)//剣が攻撃が当たったら
    //        {
    //            if(CollisionFirstFlag == false)//最初の一回だけ
    //            {
    //                enemyAgent.DamageEnemy(5);//enemyAgent.DamageEnemy(2);//敵の体力が減る関数実行
    //                CollisionFirstFlag = true;
    //            }
    //        }
    //    }
    //    ActionFlag |= AttackFlag;
    //}
    void ItemChange()
    {
        if (!attackSctipt.ChargeFlag && !AttackFlag)//攻撃、攻撃チャージ中はダメ
        {
            if (Input.GetKey(KeyCode.Alpha4))
            {
                NowHave = 0;
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                NowHave = 1;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                NowHave = 2;
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                NowHave = 3;
            }
        }


        if(NowHave == 0)
        {
            Item[Sword - 1].SetActive(false);
            Item[Reypier - 1].SetActive(false);
            Item[Apple - 1].SetActive(false);
        }

        if(NowHave == Sword && PlayerItem[Sword] == true)
        {
            Item[Sword - 1].SetActive(true);
            Item[Reypier - 1].SetActive(false);
            Item[Apple - 1].SetActive(false);
        }
        if (NowHave == Reypier && PlayerItem[Reypier] == true)
        {
            Item[Sword - 1].SetActive(false);
            Item[Reypier - 1].SetActive(true);
            Item[Apple - 1].SetActive(false);
        }
        if (NowHave == Apple && PlayerItem[Apple] == true)
        {
            Item[Sword - 1].SetActive(false);
            Item[Reypier - 1].SetActive(false);
            Item[Apple - 1].SetActive(true);
        }

    }
    void avoid()
    {
        StepFlag = RightStepFlag || LeftStepFlag || BackStepFlag;
        ActionFlag |= StepFlag;

        if (StepFlag)
        {
            StepTimer += Time.deltaTime;
        }
        else
        {
            StepTimer = 0;
        }

        if (!ActionFlag)
        {
            if (SPEnableFlag)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    RightStepFlag = true;
                    PlayerAnimation.SetBool("RightStep", true);
                    SP -= StepDe;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    LeftStepFlag = true;
                    PlayerAnimation.SetBool("LeftStep", true);
                    SP -= StepDe;
                }
                if (Input.GetKey(KeyCode.X))
                {
                    BackStepFlag = true;
                    PlayerAnimation.SetBool("BackStep", true);
                     SP -= StepDe;
                }
            }
        }

            if (RightStepFlag)
            {
                rb.velocity = transform.right * StepSpeed;
               
            }

            if (LeftStepFlag)
            {
                rb.velocity = transform.right * (-StepSpeed);
            }

            if (BackStepFlag)
            {
                rb.velocity = transform.forward * (-StepSpeed);
            }


        if (StepTimer >= 0.8f)
        {
            RightStepFlag = false;
            LeftStepFlag = false;
            BackStepFlag = false;
            PlayerAnimation.SetBool("RightStep", false);
            PlayerAnimation.SetBool("LeftStep", false);
            PlayerAnimation.SetBool("BackStep", false);
        }
    }
    void SPma()//SPマネジメント
    {
        SPHealTime += Time.deltaTime;
        if(RunFlag || StepFlag)
        {
            SPHealTime = 0;
        }

        if(SPHealTime >= 1.5f)
        {
            SP += Time.deltaTime * SPIn;
        }

        if (SP > 100)
        {
            SP = 100;
            SPEnableFlag = true;
        }
        if (SP < 0)
        {
            SP = 0;
            SPEnableFlag = false;
        }
    }
    //private void Heal()
    //{
    //    if (NowHave == Apple && PlayerItem[Apple] && Input.GetMouseButton(1))
    //    {
    //        HP += 30;
    //        if (HP > 100)
    //        {
    //            HP = 100;
    //        }
    //        PlayerItem[Apple] = false;
    //    }
    //}
}



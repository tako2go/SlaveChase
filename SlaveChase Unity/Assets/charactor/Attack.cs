using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Attack : MonoBehaviour
{
    public bool AttackFlag;

    private const int Sword = 1;
    private const int Reypier = 2;
    private const int Apple = 3;

    charactor Player;
    Animator PlayerAnimation;
    public SwordCollision WeaponCollision;
    public Transform Weapon;
    public GameObject EnemyAgent;
    private bool AttackEnable = true;
    private bool LeftMouseFlag;
    private bool RightMouseFlag;
    private float AttackTime = 0;
    private const float PunchTime = 0.3f;
    private const int PunchDamage = 1;
    private const float SwordTime = 0.8f;
    private const float ChargeSwordTime = 1.3f;
    private const int SwordDamage = 5;
    private const int SwordChargeDamage = 15;
    private const float RaypierTime = 0.6f;
    private const float ChargeRaypierTime = 0.6f;
    private const int RaypierDamage = 3;

    public  bool ChargeFlag = false;
    private const float ChargeTime = 1.0f;
    public float ChargeingTime = 0;

    private bool FirstTouchFlag = false;
    private int AttackDamage = 0;
    private float CoolTime = 0;
    private int pattern = 1;

    public BoxCollider boxCollider;

    private Vector3 NomalPosition = new Vector3(-0.01f, 0.01f, -0.02f);
    private Quaternion NomalRotation = Quaternion.Euler(-25.3f, 15.22f, 0);
    private Vector3 SwordAttackPosition = new Vector3(-0.004f, 0.007f, -0.02f);
    private Quaternion SwordAttackRotation = Quaternion.Euler(12.3f, -34.34f, -21.6f);
    private Vector3 SwordChargePosition = new Vector3(-0.01f, -0.01f, -0.003f);
    private Quaternion SwordChargeRotation = Quaternion.Euler(-63.9f, 96.25f, -156.2f);
    private Vector3 SwordChargeAttackPosition = new Vector3(-0.002f, -0.003f, -0.01f);
    private Quaternion SwordChargeAttackRotation = Quaternion.Euler(4.95f, 30.2f, -182.9f);
    private Vector3 RaypierAttackPosition = new Vector3(-0.012f, 0.02f, -0.006f);
    private Quaternion RaypierAttackRotation = Quaternion.Euler(52.22f, 79.54f, -177.33f);
    // Start is called before the first frame update


    void Start()
    {
        Player = this.gameObject.GetComponent<charactor>();
        PlayerAnimation = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider.enabled = AttackFlag;


        AttackEnable = !Player.ActionFlag;
        AttackTime += Time.deltaTime;



        if (!AttackFlag)
        {
            PlayerAnimation.SetBool("Punch", false);
            PlayerAnimation.SetBool("NomalSword", false);
            PlayerAnimation.SetBool("NomalRaypier", false);
            PlayerAnimation.SetBool("SwordChargeAttack", false);
            PlayerAnimation.SetBool("RaypierChargeAttack", false);
            FirstTouchFlag = false;
            if (!ChargeFlag)//攻撃もチャージもしていなかったら
            {
                Weapon.localPosition = NomalPosition;
                Weapon.localRotation = NomalRotation;
            }
        }

        if (AttackFlag)//攻撃中に
        {
            if (WeaponCollision.WeaponCollisionFlag)//初めて攻撃が当たった時に
            {
                if (!FirstTouchFlag)
                {
                    EnemyHit();
                    FirstTouchFlag = true;
                }
            }
        }

        if (AttackTime >= CoolTime)
        {
            AttackFlag = false;
            if(pattern == 3)
            {
                pattern = 1;
            }
        }
        if (Input.GetMouseButton(0))
        {
            LeftMouseFlag = true;
        }
        else
        {
            LeftMouseFlag = false;
        }

        if (Input.GetMouseButton(1))
        {
            RightMouseFlag = true;
        }
        else
        {
            RightMouseFlag = false;
        }

        if(ChargeFlag == false)
        {
            ChargeingTime = 0;
        }

        switch (pattern)
        {
            case 1:
              if (AttackEnable)
              {
                    if (LeftMouseFlag)
                    {
                        if (Player.NowHave == Sword && Player.PlayerItem[Sword] == true)
                        {
                            SwordAttack();
                        }
                        else if (Player.NowHave == Reypier && Player.PlayerItem[Reypier] == true)
                        {
                            RaypierAttack();
                        }
                        else if (Player.NowHave == Apple && Player.PlayerItem[Apple] == true)
                        {
                            Player.HP += 30;
                            if(Player.HP > 100) { 
                                Player.HP = 100;
                            }
                            Player.PlayerItem[Apple] = false;
                            Player.Item[Apple - 1].SetActive(false);
                        }
                        else
                        {
                            Punch();
                        }
                    }

                    if (RightMouseFlag)
                    {
                        if ((Player.NowHave == Sword && Player.PlayerItem[Sword] == true))
                        {
                            pattern = 2;
                            ChargeingTime = 0;
                            ChargeFlag = true;
                            PlayerAnimation.SetBool("SwordCharge", true);
                            Weapon.localPosition = SwordChargePosition;
                            Weapon.localRotation = SwordChargeRotation;
                        }
                        //if (Player.NowHave == Reypier && Player.PlayerItem[Reypier] == true)
                        //{
                        //    pattern = 2;
                        //    ChargeingTime = 0;
                        //    ChargeFlag = true;
                        //}
                    }
                }
                break;

            case 2://チャージ中
                ChargeingTime += Time.deltaTime;
                if (!RightMouseFlag)
                {
                    if(ChargeingTime >= ChargeTime)
                    {
                        pattern = 3;
                        AttackTime = 0;
                    }
                    else
                    {
                        ChargeFlag = false;
                        pattern = 1;
                        PlayerAnimation.SetBool("SwordCharge", false);
                    }

                }
                break;

            case 3:
                if (Player.NowHave == Sword && Player.PlayerItem[Sword] == true)
                {
                    SwordChargeAttack();
                }
                if (Player.NowHave == Reypier && Player.PlayerItem[Reypier] == true)
                {
                    pattern = 4;
                    ChargeFlag = false;
                }
                break;
                case 4:
                pattern = 1;
                break;
        }
     
    }


    void Punch()
    {
        AttackTime = 0;
        CoolTime = PunchTime;
        AttackFlag = true;
        AttackDamage = PunchDamage;
        PlayerAnimation.SetBool("Punch", true);
    }
    void SwordAttack()
    {
        AttackTime = 0;
        CoolTime = SwordTime;
        AttackFlag = true;
        AttackDamage = SwordDamage;
        PlayerAnimation.SetBool("NomalSword", true);
        Weapon.localPosition = SwordAttackPosition;
        Weapon.localRotation = SwordAttackRotation;
    }
    void SwordChargeAttack()
    {
        CoolTime = ChargeSwordTime;
        ChargeFlag = false;
        AttackFlag = true;
        AttackDamage = SwordChargeDamage;
        PlayerAnimation.SetBool("SwordChargeAttack", true);
        PlayerAnimation.SetBool("SwordCharge", false);
        Weapon.localPosition = SwordChargeAttackPosition;
        Weapon.localRotation = SwordChargeAttackRotation;
    }

    void RaypierAttack()
    {
        AttackTime = 0;
        CoolTime = RaypierTime;
        ChargeFlag = false;
        AttackFlag = true;
        AttackDamage = RaypierDamage;
        PlayerAnimation.SetBool("NomalRaypier", true);
        Weapon.localPosition = RaypierAttackPosition;
        Weapon.localRotation = RaypierAttackRotation;
    }
    void RaypierChargeAttack()
    {
        AttackTime = 0;
        CoolTime = RaypierTime;
        ChargeFlag = false;
        AttackFlag = true;
        AttackDamage = RaypierDamage;
        PlayerAnimation.SetBool("NomalRaypier", true);
    }

    void EnemyHit()
    {
        EnemyAgent.GetComponent<EnemyAgentSight>().HP -= AttackDamage;
        EnemyAgent.GetComponent<Animator>().SetBool("StanFlag", true);
        EnemyAgent.GetComponent<EnemyAgentSight>().StanFlag = true;
        EnemyAgent.GetComponent<EnemyAgentSight>().StanTimer= 0;
        //Debug.Log("EnemyHit実行");
    }
}

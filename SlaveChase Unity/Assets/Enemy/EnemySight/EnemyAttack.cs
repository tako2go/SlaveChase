using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttack : MonoBehaviour
{

    public GameObject EnemyWeapon;
    EnemyAgentSight EnemyAgent;
    Animator EnemyAnimator;
    public GameObject Player;

    public bool AttackFlag = false;
    private bool AttackEnableFlag = true;
    private bool ActionFlag = false;
    private float AttackTimer = 0;
    private int AttackDamage = 0;
    public bool CoolFlag = false;
    private const float CoolTime = 2.0f;
    private float CoolTimer = 0;
    private float AttackEndTime = 0.1f;//バグ防止　これをしないとAttackTimer >= AttackEndTimeが最初 0 >= 0で常にtrue
    private const float SlashTime = 1.3f;
    private const int SlashDamage = 7;
    private const float RollSlashTime = 1.3f;
    private const int RollSlashDamage = 5;
    private const float ThrustTime = 0.8f;
    private const int ThrustDamage = 10;

    private bool Flag = false;

    //武器transform
    //通常    transform position:x:-0.01 y:-0.025 z:-0.082  rotation:x:236 y:45 z:-36
    //攻撃通常transform position:x:-0.012 y:0.046 z:-0.036  rotation:x:302 y:50 z:-9
    //　　回転transform position:x:-0.023 y:-0.074 z:-0.055  rotation:x:236 y:45 z:-36
    //　　突きtransform position:x:-0.06 y:0.06 z:-0.07  rotation:x:320 y:-12 z:35

    private Vector3 NomalPosition = new Vector3(-0.01f, -0.025f, -0.082f);
    private Quaternion NomalRotation = Quaternion.Euler(236, 45, -36);
    private Vector3 SlashPosition = new Vector3(-0.012f, 0.046f, -0.036f);
    private Quaternion SlashRotation = Quaternion.Euler(302, 50, -9);
    private Vector3 RollPosition = new Vector3(-0.023f, -0.074f, -0.055f);
    private Quaternion RollRotation = Quaternion.Euler(236, 45, -36);
    private Vector3 ThrustPosition = new Vector3(-0.06f, 0.06f, -0.07f);
    private Quaternion ThrustRotation = Quaternion.Euler(320, -12, 35);


    // Start is called before the first frame update
    void Start()
    {
        EnemyAgent = GetComponent<EnemyAgentSight>();
        EnemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ActionFlag = AttackFlag | EnemyAgent.StanFlag | CoolFlag;
        EnemyWeapon.GetComponent<CapsuleCollider>().enabled = AttackFlag;

        if (AttackTimer >= AttackEndTime)
        {
            AttackFlag = false;
            AttackTimer = 0;
            CoolTimer = 0;
            CoolFlag = true;
            ResetAttackAni();
        }
        CoolTimer++;
        if (CoolTimer >= CoolTime)
        {
            CoolFlag = false;
            CoolTimer = 0;
        }

        if (AttackFlag)
        {
            AttackTimer += Time.deltaTime;
            EnemyAnimator.SetBool("Walk", false);
            EnemyAnimator.SetBool("Run", false);
            if (EnemyWeapon.GetComponent<EnemyWeapon>().EWCollisionFlag)
            {
                if (!Flag)
                {
                    PlayerHit();
                    Flag = true;
                }
            }
        }

        if (!AttackFlag)
        {
            ResetAttackAni();
            EnemyWeapon.transform.localPosition = NomalPosition;
            EnemyWeapon.transform.localRotation = NomalRotation;
            Flag = false;
        }

        if (!ActionFlag)//アクションを何もしていなくて
        {
            if (EnemyAgent.action == 1)
            {
                Slash();
            }
            if (EnemyAgent.action == 2)
            {
                RollSlash();
            }
            if (EnemyAgent.action == 3)
            {
                Thrust();
            }
        }
    }

    void Slash()
    {
        AttackFlag = true;
        AttackEndTime = SlashTime;
        AttackTimer = 0;
        AttackDamage = SlashDamage;
        EnemyAnimator.SetBool("SlashFlag", true);
        EnemyWeapon.transform.localPosition = SlashPosition;
        EnemyWeapon.transform.localRotation = SlashRotation;
    }
    void RollSlash()
    {
        AttackFlag = true;
        AttackEndTime = RollSlashTime;
        AttackTimer = 0;
        AttackDamage = RollSlashDamage;
        EnemyAnimator.SetBool("RollFlag", true);
        EnemyWeapon.transform.localPosition = RollPosition;
        EnemyWeapon.transform.localRotation = RollRotation;
    }
    void Thrust()
    {
        AttackFlag = true;
        AttackEndTime = ThrustTime;
        AttackTimer = 0;
        AttackDamage = ThrustDamage;
        EnemyAnimator.SetBool("ThrustFlag", true);
        EnemyWeapon.transform.localPosition = ThrustPosition;
        EnemyWeapon.transform.localRotation = ThrustRotation;
    }

    void PlayerHit()
    {
        Player.GetComponent<charactor>().HP -= AttackDamage;
        Player.GetComponent<charactor>().StanFlag = true;
        Player.GetComponent<charactor>().StanTimer = 0;
        Player.GetComponent<Animator>().SetBool("StanFlag", true);
    }

    void ResetAttackAni()
    {
        EnemyAnimator.SetBool("SlashFlag", false);
        EnemyAnimator.SetBool("RollFlag", false);
        EnemyAnimator.SetBool("ThrustFlag", false);
    }
}
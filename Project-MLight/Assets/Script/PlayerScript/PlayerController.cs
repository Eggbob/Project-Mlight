using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : LivingEntity
{
    private Status status;
    [SerializeField]
    private PlayerMoveController pmanager;
    public GameObject levelUp;
    public Animator anim; //애니메이터 컴포넌트
    public static PlayerController instance; //싱글톤을 위한 instance
    public Skill pAttack;
    public MeleeWeaponTrail trail;

    public enum PlayerState { Idle, Move, Attack, Skill, Drop, Die }
    public PlayerState pState; //플레이어 상태 변수

    private bool isMove; // 움직임 관련 불값
    private bool isInter; // 오브젝트 상호작용 관련 불값
    private bool isAttack; // 공격하는지
    private int sid; //스킬 아이디를 저장할 변수

    private void Awake()
    {
        status = this.GetComponent<Status>();
        pmanager = this.GetComponent<PlayerMoveController>();
        anim = this.GetComponent<Animator>();
        pState = PlayerState.Idle;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
  
    }

    void CheckAnimations()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                isMove = false;
                isAttack = false;
                isInter = false;
                break;
            case PlayerState.Move:
                isMove = true;
                isAttack = false;
                isInter = false;      
                break;
            case PlayerState.Attack:
                isMove = false;
                isInter = false;
                isAttack = true;
                break;
            case PlayerState.Drop:
                isMove = false;
                isAttack = false;
                isInter = true;
                DropUpdate();
                break;
            case PlayerState.Die:
                break;
        }
    }

    void CheckStatus()
    {
        switch (pState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:               
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Skill:
                break;
            case PlayerState.Drop:
              //  DropUpdate();
                break;
            case PlayerState.Die:
                break;
        }
    }

    public void SkillUpdate(int sId)
    {
        pState = PlayerState.Skill;
      
        anim.SetTrigger("isSkill");
        anim.SetInteger("Skill", sId);
        
    }

   
    void AttackUpdate()
    {
        
    }


    void AttackCheck()
    {
        LivingEntity enemytarget = target.GetComponent<LivingEntity>();

        trail.Emit = true;
        if (enemytarget != null)
        {            
            if (enemytarget.dead)
            {            
                pmanager.curtarget = PlayerMoveController.TargetLayer.None;
                pState = PlayerState.Idle;
                pmanager.nav.isStopped = true;
                return;
            }
            else
            {
                StartCoroutine(DamageUpdate(enemytarget));
            }
        }
    }
    

    IEnumerator DamageUpdate(LivingEntity enemyTarget)
    {       
        yield return new WaitForSeconds(0.7f);
      
        if(pmanager.isCollision)
        {
            pAttack.ActiveAction();
           
        }
        trail.Emit = false;
    }

    void DropUpdate()
    {
        target = pmanager.target;
        StartCoroutine("DropRoutine");      
    }

    IEnumerator DropRoutine()
    {              
        
        yield return new WaitForSeconds(2f);
        isInter = false;
        target.GetComponent<Object>().Drop();
        pState = PlayerState.Idle;
    }

    public override void OnDamage(Skill skill)
    {
        base.OnDamage(skill);
    }

    private void Update()
    {
        target = pmanager.target;
        CheckAnimations();
        anim.SetBool("isRun",isMove);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isInter", isInter);
 
    }


    public override void LvUp(int totalexp)
    {
        GameObject pre = Instantiate(levelUp, this.transform.position, Quaternion.identity);
        Destroy(pre, 1f);

        base.LvUp(totalexp);
    }


}

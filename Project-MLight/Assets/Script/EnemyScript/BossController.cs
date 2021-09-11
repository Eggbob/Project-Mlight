using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public enum BossState
{
    None = 0,
    Idle,
    Enable,
    NormalAttack,
    PowerAttack,
    Spell,
    Move,
    Die,
    End
}

public class BossController : Enemy
{
    private BossState bState = BossState.None;

    private int randState;

    public Text hpTxt;

    public GameObject bossUI;
    public GameObject sword;
    public GameObject shield;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
       
        nav.updateRotation = false;
        nav.speed = moveSpeed;
        nameTxt.text = enemyName;
    }

    private void OnEnable()
    {
        bState = BossState.Idle;
        nav.isStopped = true;

        bossUI.SetActive(false);
        hpBar.fillAmount = 1;

        StringBuilder hpstring = new StringBuilder();

        hpstring.Append(_hp);
        hpstring.Append("/");
        hpstring.Append(_maxHP);

        hpTxt.text = hpstring.ToString();

    }

    private void FixedUpdate()
    {
        if (hasTarget)//타겟의 위치 지속적으로 업데이트
        {
            targetPos = target.gameObject.transform.position;
            sectorCheck();
        }
    }

    //첫 시작 모션
    private IEnumerator Enable()
    {
        target = GameManager.Instance.Player.gameObject;
        enemyTarget = target.GetComponent<LivingEntity>();

        sword.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);

        bState = BossState.Enable;
        yield return new WaitForSeconds(0.1f);
        ShowAnimation((int)bState);
        yield return new WaitForSeconds(5f);

        sword.gameObject.SetActive(true);
        shield.gameObject.SetActive(true);


        bossUI.SetActive(true);
        StartCoroutine(SetBossState());
    }

    //이동
    private IEnumerator MoveToTarget() 
    {
        bState = BossState.Move;
        nav.isStopped = false;

        move = true;
        ShowAnimation((int)bState);

        yield return new WaitForSeconds(0.1f);

        while(!isCollision)
        {
            if(isCollision)
            {
                break;
            }
            nav.SetDestination(targetPos);
            transform.LookAt(targetPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        move = false;
        ShowAnimation((int)bState);

        nav.isStopped = true;
        nav.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(NormalAttack());
    }
    
    //일반 공격
    private IEnumerator NormalAttack()
    {
        bState = BossState.NormalAttack;
        nav.isStopped = true;
        ShowAnimation((int)bState);
    

        yield return new WaitForSeconds(0.5f);

        if (isCollision)
        {
            EnemyNormalAttack.ActiveAction();
        }

        yield return new WaitForSeconds(0.7f);

        StartCoroutine(SetBossState());
    }

    //강공격
    private IEnumerator PowerAttack()
    {
        bState = BossState.PowerAttack;

        nav.isStopped = true;
        transform.LookAt(targetPos);

        ShowAnimation((int)bState);

        yield return new WaitForSeconds(0.3f);

        var pAttack = BossObjectPool.GetPowerAttack();

        pAttack.transform.position = this.transform.position;
        pAttack.transform.rotation = Quaternion.Euler(new Vector3(0f, this.transform.eulerAngles.y, this.transform.eulerAngles.z));
        pAttack.gameObject.SetActive(true);
        pAttack.ActiveAction();

        yield return new WaitForSeconds(0.6f);
    
        StartCoroutine(SetBossState());
    }

    //주문 사용
    private IEnumerator UseSpell()
    {
        bState = BossState.Spell;
        nav.isStopped = true;

        ShowAnimation((int)bState);

        sword.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        sword.gameObject.SetActive(true);
        shield.gameObject.SetActive(true);

        for(int i = 0; i< 5; i++)
        {
            Vector3 spawnPos = Random.insideUnitCircle * 20f;
            spawnPos.x += this.transform.position.x;
            spawnPos.z = spawnPos.y + this.transform.position.z;
            spawnPos.y += 25f;

            var fireball = BossObjectPool.GetFireBall();
            fireball.CreaeteFire(spawnPos);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(SetBossState());
    }


    private void ShowAnimation(int state)
    {
        switch(state)
        {
            case (int)BossState.Enable:
                anim.SetTrigger("isEnable");
                break;

            case (int)BossState.Move:
                if(!isCollision)
                {
                    move = true;
                }
                else
                {
                    move = false;
                }
                anim.SetTrigger("MoveToTarget");
                anim.SetBool("isMove", move);
                break;

            case (int)BossState.NormalAttack:
                anim.SetTrigger("isAttack");
                break;

            case (int)BossState.PowerAttack:
                anim.SetTrigger("isPowerAttack");
                break;

            case (int)BossState.Spell:
                anim.SetTrigger("isSpell");
                break;

            case (int)BossState.Die:
                anim.SetTrigger("Die");
                break;
        }
    }

    //보스 패턴 정하기
    private IEnumerator SetBossState()
    {
        yield return new WaitForSeconds(1f);

        randState = Random.Range(0, 7);

        if(direction.magnitude <= attackRange + 5)
        {
            switch(randState)
            {
                case 0:
                case 1:
                case 2:
                    StartCoroutine(NormalAttack());
                    break;              
                case 3:
                case 4:
                    StartCoroutine(PowerAttack());
                    break;

                case 5:
                case 6:
                case 7:
                    StartCoroutine(UseSpell());
                    break;
            }
        }
        else
        {
            switch (randState)
            {
                case 0:
                case 1:
                case 2:
                    StartCoroutine(MoveToTarget());
                    break;
                case 3:
                case 4:
                    StartCoroutine(PowerAttack());
                    break;

                case 5:
                case 6:
                case 7:
                    StartCoroutine(UseSpell());
                    break;
            }
        }
    }

  
    protected override void Die()
    {
        base.Die();
        StartCoroutine(DieRoutine());
    }

    protected override void sectorCheck()
    {
        base.sectorCheck();
    }

    public void StartRoutine()
    {
        StartCoroutine(Enable());
    }

    private IEnumerator DieRoutine() // 사망상태일시
    {
        anim.SetTrigger("Die"); // 트리거 활성화
        bState = BossState.Die;
        nav.enabled = false; // 네비 비활성화

        bossUI.SetActive(false);
        target.GetComponent<LivingEntity>().ExpGetRoutine(enemyExp);
        Collider[] enemyColliders = GetComponents<Collider>();

        // 콜라이더 다끄기
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        rigid.isKinematic = true;

        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }

    public override void OnDamage(Skill skill)
    {
        if (dead) return;

        Hp -= (int)skill.SkillPower;  // 스킬의 위력만큼 HP 감소

        if (Hp <= 0 && !dead)
        {
            Hp = 0;
            StopAllCoroutines();

            Die();
        }

        var dTxt = ObjectPool.GetDTxt();
        dTxt.SetText((int)skill.SkillPower);
        dTxt.transform.position = dTxtPos.position;
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, _hp/_maxHP, 4f * Time.deltaTime);
        hpTxt.text = _hp.ToString() + "/" + _maxHP.ToString();

    }

}

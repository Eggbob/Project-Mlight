using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthUp : ActiveSkill
{
    public GameObject EffectPrefab;
    private GameObject effect;
    

    public override void ActiveAction()
    {
        Vector3 ePos = LCon.transform.position;
        ePos.y += 4f;

        //이펙트 생성
        effect = Instantiate(EffectPrefab, ePos, this.transform.rotation);
        Destroy(effect, 1f);

        Damage = LCon.Power * this.SkillPower / 100;

        //버프 적용
        LCon.buffManager.CreateBuff(BuffManager.BuffType.Atk, this.ActTime, this.Damage);
    }

    public override void Init(LivingEntity _LCon)
    {
        LCon = _LCon;
     
        this._description = "자신에게 아래 효과를 부여합니다 \n" + 
            "- 300초동안 공격력이 " +  this._skillPower+"% 증가";  

        this.sAttr = SkillAttr.Buff;
    }

    //IEnumerator BuffRoutine()
    //{
    //    LCon.Power += (int)this.SkillPower;
   
    //    yield return new WaitForSeconds(this.ActTime);
    //    LCon.Power -= (int)this.SkillPower;
      
    //}

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;

        _skillPower += 5;
        _skillLevel++;
        _maxSkillExp *= 2;

        this._description = "자신에게 아래 효과를 부여합니다 \n" +
           "- 300초동안 공격력이 " + this._skillPower + "% 증가";
    }

}

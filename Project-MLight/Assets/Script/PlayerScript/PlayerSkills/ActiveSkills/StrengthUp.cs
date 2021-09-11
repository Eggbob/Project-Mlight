using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthUp : ActiveSkill
{
    private PlayerController pCon;


    public override void ActiveAction()
    {
        Vector3 ePos = pCon.transform.position;
        ePos.y += 5f;

        StartCoroutine(BuffRoutine(ePos));
    }

    public override void Init(LivingEntity _LCon)
    {
        pCon = _LCon as PlayerController;
     
        this._description = "자신에게 아래 효과를 부여합니다 \n" + 
            "- 300초동안 공격력이 " + Damage + "% 증가";  

        this.sAttr = SkillAttr.Buff;

        effect = Instantiate(effectPrefab, this.transform);
        effect.gameObject.SetActive(false);

        base.Init(pCon);
    }

    IEnumerator BuffRoutine(Vector3 _ePos)
    {
        this._skillPower = (int)(Damage * pCon.Power / 100);
        //버프 적용
        pCon.buffManager.CreateBuff(BuffManager.BuffType.Atk, this.ActTime, this._skillPower);

        effect.transform.position = _ePos;
        effect.transform.rotation = this.transform.rotation;
        effect.gameObject.SetActive(true);

        //pAudio.PlayOneShot(effectSound);
        BgmManager.Instance.PlayCharacterSound(effectSound);
        yield return new WaitForSeconds(1f);

        effect.gameObject.SetActive(false);
    }

    protected override void SkillLevelUp()
    {
        float remainSkillExp = SkillExp - MaxSkillExp;
        _skillExp = remainSkillExp;

        Damage += 5;
        _skillLevel++;
        _maxSkillExp *= 2;

        this._description = "자신에게 아래 효과를 부여합니다 \n" +
           "- 300초동안 공격력이 " + this._skillPower + "% 증가";
    }

}

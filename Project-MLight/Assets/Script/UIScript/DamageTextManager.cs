using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    private TextMesh tMesh;

    [SerializeField] //위로 이동할 속도
    private float moveSpeed;

    [SerializeField] //사라질 시간
    private float removeTime;

    private void Awake()
    {
        tMesh = GetComponent<TextMesh>(); 
    }

    private void Update()
    {
        this.transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        Invoke("MoveRoutine", removeTime);
    }

    public void SetText(int damage)//텍스트 설정하기
    {
        tMesh.text = damage.ToString();
    }

    private void MoveRoutine()
    {       
        ObjectPool.ReturnDTxt(this);
    }

}

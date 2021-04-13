using UnityEngine;

public class Spin : MonoBehaviour {
    public float turnDegPerSec = 360;
    public Vector3 localAxis;
    protected float turnDegPerFrame;

    public Vector3 InitScale;
    float time;

    public void Start()
    {
        turnDegPerFrame = turnDegPerSec * Time.deltaTime;
    }

    public void Update()
    {
        transform.Rotate(localAxis, turnDegPerFrame);
        transform.localScale = InitScale * (1 - time);
        if(time > 1f)
        {
            time = 0;
            //gameObject.SetActive(false);
            ObjectPool.ReturnObject(this);
        }
        time += Time.deltaTime;
    }

    public void resetScale()
    {
        time = 0;
        transform.localScale = InitScale;
    }

}

using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    ParticleSystem prt;
    void Start()
    {
         prt = gameObject.GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (prt.isStopped) gameObject.SetActive(false);
    }
}

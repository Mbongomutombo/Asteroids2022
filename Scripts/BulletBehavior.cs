using UnityEngine;

public class BulletBehavior : MonoBehaviour, IDamageable
{
    [Header("Bullet speed")]
    [SerializeField] float speed;
    public int Health { get ; set ; }
    public int Power { get ; set ; }
    public bool Scoreable { get; set; }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    //When the bullet leave cameras frustrum
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    public void RecieveDamage(int damage)
    {
        //we may to set inactive trough interface
        //Health -= damage;
        //if (Health <= 0) gameObject.SetActive(false);
        //or...
    }
    private  void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<IDamageable>().RecieveDamage(1);
        // or we may make inactive when collide + increase scores
        gameObject.SetActive(false);
        if (Scoreable)  GameManager.Instance.Scores += 1;
    }
}

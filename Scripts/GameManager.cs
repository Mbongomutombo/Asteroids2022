using UnityEngine;
using TMPro;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Player")]
    [SerializeField] GameObject player;

    [Space]
    [Header("Maximal buffer zone length for spawn asteroids")]
    [SerializeField] float bufferMax = 10f;

    [Space]
    [Header("Minimal buffer zone length for spawn asteroids")]
    [SerializeField] float bufferMin = 5f;

    [Space]
    [Header("Time between spawn asteroids")]
    [SerializeField] float spawnInterval = 0.2f;

    [Space]
    [SerializeField] TextMeshProUGUI scores;
    [SerializeField] TextMeshProUGUI hiScores;

    [Space]
    [SerializeField] GameObject enemy;
    [Header("Set to Enemy HP")]
    [SerializeField] int enemyMaxHP;

    Vector3 spawnPoint;
    Vector3 destination;
    float minX, maxX, minY, maxY;
    float spawnPointX, spawnPointY;
    float time, time2;
    int side;
    int timeToEnemy;

    //support massive for spawn triple asteroids
    Vector3[] threeDots = { new Vector3(0,1,0), new Vector3(1,-1,0), new Vector3(-1,-1,0) };
    public Vector3 ScreenCenter { get; private set; }
    public float ScreenDimension { get; private set; }
    public int Scores { get; set; }
    private int HighScores { get; set; }

    void Start()
    {
        if (PlayerPrefs.HasKey("HighScores")) HighScores = PlayerPrefs.GetInt("HighScores");
        hiScores.text = "High Scores: " + HighScores;

        timeToEnemy = Random.Range(10, 30); //time to spawn enemy

        ScreenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); //precalculate center of screen

        minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        Debug.Log(minX);
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;
        Debug.Log(maxX);
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        Debug.Log(minY);
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -Camera.main.transform.position.z)).y;
        Debug.Log(maxY);

        ScreenDimension = (ScreenCenter - (new Vector3(minX, minY, 0))).magnitude;

        for (int i = 0; i < 10; i++)
        {
            LaunchAsteroid();
        }
        
    }
    void Update()
    {
        //launch new asteroids
        time += Time.deltaTime;

        if (time >= spawnInterval)
        {
            time = time - spawnInterval;

            LaunchAsteroid();
        }

        //rotating the SkyBox
        RenderSettings.skybox.SetFloat("_Rotation", Time.time*0.3f);
        scores.text = "Scores: " + Scores;

        //launch the enemy
        if (!enemy.activeInHierarchy)
        {
            time2 += Time.deltaTime;
            if (time2 >= timeToEnemy)
            {
                time2 = time2 - timeToEnemy;
                timeToEnemy = Random.Range(10, 20);
                SummonEnemy();
            }
        }
    }

    private void LateUpdate()
    {
        //check  Player position on screen
        if (player.transform.position.x < minX * 1.01f || player.transform.position.x > maxX * 1.01f)
        {
            player.transform.position =  ChangePositionX(player.transform.position);
        }
        if(player.transform.position.y < minY * 1.01f || player.transform.position.y > maxY * 1.01f)
        {
            player.transform.position = ChangePositionY(player.transform.position);
        }

    }
    Vector3 ChangePositionX(Vector3 position)
    {
        return new Vector3(- position.x,  position.y, 0);
    }
    Vector3 ChangePositionY(Vector3 position)
    {
        return new Vector3(position.x, - position.y, 0);
    }

    void LaunchAsteroid()
    {
        //get free asteroid from the asteroid pool
        //set maximum size and HP to the asteroid
        GameObject asteroid =  AsteroidPoolManager.Instance.RequestAsteroid(3);

        //put the asteroid on random point of the launch zone
        //set direction and velocity to him
        side = Random.Range(0, 4);
        Debug.Log(side);
        if (side == 0)
        {
            //from left side to right side
            spawnPointX = Random.Range(minX - bufferMax, minX-bufferMin);
            spawnPointY = Random.Range(minY, maxY);
            destination = new Vector3(maxX, Random.Range(minY, maxY), 0);
        }
        else if (side == 1)
        {
            //from upper side to lower side
            spawnPointX = Random.Range(minX, maxX);
            spawnPointY = Random.Range(maxY + bufferMin, maxY + bufferMax);
            destination = new Vector3(Random.Range(minX, maxX), minY, 0);
        }
        else if( side ==2)
        {
            //from right side to left side
            spawnPointX = Random.Range(maxX + bufferMin, maxX  +bufferMax);
            spawnPointY = Random.Range(minY, maxY);
            destination = new Vector3(minX, Random.Range (minY, maxY), 0);
        }
        else if(side == 3)
        {
            //from lower side to upper side
            spawnPointX = Random.Range(minX, maxX);
            spawnPointY = Random.Range(minY - bufferMax, minY - bufferMin);
            destination = new Vector3(Random.Range(minX, maxX), maxY, 0);
        }

        spawnPoint = new Vector3(spawnPointX, spawnPointY, 0);
        asteroid.transform.position = spawnPoint;
        asteroid.transform.GetComponent<Collider>().enabled = true;
        //kick the asteroid
        asteroid.GetComponent<Rigidbody>().AddForce(destination, ForceMode.Impulse);
    }

    public void TripleSpawn(Transform _transform, int size)
    {
        GameObject _explosion = ExplosionPoolManager.Instance.RequestExplosion();
        _explosion.transform.position = _transform.position;
        _explosion.SetActive(true);
        _explosion.GetComponent<ParticleSystem>().Play();

        if (size <= 1) return;

        for (int i = 0; i < 3; i++)
        {
            GameObject asteroid = AsteroidPoolManager.Instance.RequestAsteroid(size - 1);
            asteroid.transform.position = _transform.position + threeDots[i];
            if(size == 3) asteroid.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            else asteroid.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            asteroid.GetComponent<Rigidbody>().AddExplosionForce(4, _transform.position, 2f);
        }
    }
    void SummonEnemy()
    {
        enemy.SetActive(true);
        enemy.transform.rotation = Quaternion.identity;
        enemy.GetComponentInChildren<IDamageable>().Health = enemyMaxHP;
        
        //create random spawn point
        Vector2 rnd =  Random.insideUnitCircle.normalized;
        Vector3 rndV3 = new Vector3(rnd.x, rnd.y);
        enemy.transform.position = ScreenCenter + rndV3 * ScreenDimension;
    }
    public void SaveHiScore()
    {
        if (Scores > HighScores)
        PlayerPrefs.SetInt("HighScores", Scores);
    }

}

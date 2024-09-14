using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTime = 1.5f;
    public float time = 0.0f;
    public TMP_Text liveText;
    public PlayerBhv player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CreateEnemy();
        UpdateCanvas();
    }
    public void CreateEnemy()
    {
        time += Time.deltaTime;
        if (time > spawnTime)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0), Quaternion.identity);
            time = 0.0f;
        }
    }
    void UpdateCanvas()
    {
        liveText.text = "vidas :" + player.lives;
    }
}

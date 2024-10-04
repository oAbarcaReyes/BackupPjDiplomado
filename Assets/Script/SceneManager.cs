using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void LoasfirstScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

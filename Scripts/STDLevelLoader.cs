using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for smooth change scenes
/// </summary>
public class STDLevelLoader : MonoSingleton<STDLevelLoader>
{
    [SerializeField] Animator crossfadeController;
    [Space]
    [SerializeField] float transitionDelay = 1f;

    /// <summary>
    /// Calls from PlayMaker FSM
    /// </summary>
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadLevel( int levelIndex)
    {
        crossfadeController.SetTrigger("Start");
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(levelIndex);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Animator SwipeAnimator;
    public void SceneLoader(int value)
    {
        StartCoroutine(Swipe(value));
       
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Swipe(int value)
    {
        SwipeAnimator.SetTrigger("Change");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(value);
    }
}

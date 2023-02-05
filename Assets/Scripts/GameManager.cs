using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    public bool isSlowMo;
    public IEnumerator SlowMo()
    {
        isSlowMo = true;
        LeanTween.value(gameObject, 1, 0, 4).setIgnoreTimeScale(true).setOnUpdate((float val) => { Time.timeScale = val; });

        yield return new WaitForSecondsRealtime(5);

        LeanTween.value(gameObject, 0, 1, 1).setIgnoreTimeScale(true).setOnUpdate((float val) => { Time.timeScale = val; });
        yield return new WaitForSecondsRealtime(1);

        isSlowMo = false;
    }

    public void WinGame()
    {
        SceneManager.LoadScene(0);
    }


}

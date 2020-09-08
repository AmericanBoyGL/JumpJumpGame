using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResartLvl : MonoBehaviour
{
    public void RestartLvl()
    {
        /*Узнаем последний уровень который мы играли и загружаем его*/
        string rest = PlayerPrefs.GetString("LastSceneToPlay");
        SceneManager.LoadScene(rest);
    }
}

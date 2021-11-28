using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
}

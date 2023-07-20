using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*using Photon.Pun;
using Photon.Realtime;
*/
public class SplashMenu : MonoBehaviour
{
    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3);

            SceneManager.LoadScene("AllGames");

    }

    private void Start()
    {
        StartCoroutine(WaitForSceneLoad());
    }
}

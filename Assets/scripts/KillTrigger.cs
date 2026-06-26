using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject splash;   //水花动画
    public AudioClip splashVoice;
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        AudioSource.PlayClipAtPoint(splashVoice, col.transform.position);
        // 如果是主角碰到killtrigger
        if (col.gameObject.tag == "Player")
        {
            

            // 停止血条跟随
            if (GameObject.FindGameObjectWithTag("HealthBar").activeSelf)
            {
                GameObject.FindGameObjectWithTag("HealthBar").SetActive(false);
            }

            // 实列化水花动画
            Instantiate(splash, col.transform.position, transform.rotation);
            // 销毁主角.
            Destroy(col.gameObject);
            //重启程序
            StartCoroutine("ReloadGame");
        }
        else
        {
            // 实列化水花动画
            Instantiate(splash, col.transform.position, transform.rotation);
            // 销毁敌人.
            Destroy(col.gameObject);
        }

    }

    IEnumerator ReloadGame()
    {
        // 延迟两秒
        yield return new WaitForSeconds(2);
        // 重启游戏.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}

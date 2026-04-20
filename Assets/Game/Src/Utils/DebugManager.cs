using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public float CoinsIncrement = 100f;
    void Start()
    {
        #if !Unity_Editor
        gameObject.SetActive(false);
        #endif
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            G.main.ReceiveCoins(CoinsIncrement, Vector3.zero);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            G.main.ReceiveCoins(-CoinsIncrement, Vector3.zero);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

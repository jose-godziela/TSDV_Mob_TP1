using UnityEngine;

public class SceneRef : MonoBehaviour
{
    [SerializeField] SceneLoader sceneMg;

    public void CallLoadLevel()
    {
        sceneMg.LoadScene();
    }
}

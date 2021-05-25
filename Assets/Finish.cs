using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Camera.main.GetComponent<UIManagerController>().Win();
    }
}

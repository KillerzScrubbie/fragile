using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelChanger levelChanger = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            levelChanger.NextLevel();
        }
    }
}

using UnityEngine;

public class Spike : MonoBehaviour
{
    // [SerializeField] private BoxCollider2D spikeCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelChanger levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<LevelChanger>();
            levelChanger.RestartLevel();
        }
    }
}

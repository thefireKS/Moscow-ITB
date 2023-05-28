using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTower : MonoBehaviour
{
    [SerializeField] private int health = 2;
    [SerializeField] private GameObject explosion;

    public static Action<GameReplaySystem.result> setWinResult, setTieResult;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            health--;
            var explosionObject = Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
            Destroy(explosionObject,0.5f);
            Destroy(collision.gameObject);
        }

        if (collision.transform.CompareTag("Player"))
        {
            health = 0;
            setTieResult?.Invoke(GameReplaySystem.result.Tie);
            Destroy(gameObject,0.1f);
            return;
        }

        if (health > 0) return;
        
        setWinResult?.Invoke(GameReplaySystem.result.Win);
        Destroy(gameObject,0.1f);
    }

    private void OnDestroy()
    {
        for (var i = 0; i < 5; i++)
        {
            var rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
            var instantiate = Instantiate(explosion, rndPosWithin, transform.rotation);
            instantiate.transform.localScale =
                new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
            instantiate.transform.rotation = Quaternion.Euler(Random.Range(0f, 180f),Random.Range(0f, 180f), Random.Range(0f, 180f));
            Destroy(instantiate,0.5f);
        }
    }
}

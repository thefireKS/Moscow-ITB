using UnityEngine;

public class BulletLaser : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Start()
    {
        Destroy(gameObject,2f);
    }

    private void Update()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }
}

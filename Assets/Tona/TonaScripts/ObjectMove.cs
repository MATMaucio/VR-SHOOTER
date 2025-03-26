using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public PoolObject cactusPool;

    void Update()
    {
        if (gameObject.activeInHierarchy)  // Solo mueve el cactus si está activo
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Debug.Log("Cactus colisionó con Respawn");
            cactusPool.Returncactus(gameObject);
        }
    }
}

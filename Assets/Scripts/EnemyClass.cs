using UnityEngine;
using System.Collections;

public class EnemyClass : MonoBehaviour
{
    private Transform target;
    private float speed;
    private bool isHit = false;
    private Renderer rend;

    void Start()
    {
        // หา Player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        // สุ่มตำแหน่งและความเร็ว
        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(1f, 7f);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY / Camera.main.pixelHeight, 20f));
        transform.position = new Vector3(worldPos.x, randomY, 20f);

        speed = Random.Range(2f, 6f);

        // Get Renderer for color effect
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!isHit && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit && other.CompareTag("Bullet"))
        {
            FindAnyObjectByType<MainLogic>()?.AddScore();
            isHit = true;
            StartCoroutine(FlashRedAndDestroy());
        }

        //Remove enemy when touched with player to prevent Player colliding with Enemy
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<MainLogic>()?.GetDamage();
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRedAndDestroy()
    {
        float duration = 0.9f;
        float timer = 0f;
        bool toggle = false;

        FindAnyObjectByType<MainLogic>()?.GetDamage();

        while (timer < duration)
        {
            if (rend != null)
            {
                rend.material.color = toggle ? Color.red : Color.white;
            }

            toggle = !toggle;
            timer += 0.15f;
            yield return new WaitForSeconds(0.15f);
        }

        

        Destroy(gameObject);
    }
}

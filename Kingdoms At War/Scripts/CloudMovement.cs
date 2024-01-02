using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    Collider cloudLimits;
    Vector3 previousPosition;
    Vector3 nextPosition;
    [SerializeField] private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        cloudLimits = GetComponentInParent<Collider>();
        transform.position = RandomPointInBounds(cloudLimits.bounds);
        nextPosition = RandomPointInBounds(cloudLimits.bounds);
        speed = Random.Range(10,100);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        if (transform.position == nextPosition)
        {
            nextPosition = RandomPointInBounds(cloudLimits.bounds);
            speed = Random.Range(10, 100);
        }

    }

    private IEnumerator MoveCloud()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        yield return new WaitForEndOfFrame();

    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(nextPosition, 10);
    }

}

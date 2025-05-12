using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour
{
    public LineRenderer line;
    public float lifetime = 2f;

    public void Init(Vector3 start, Vector3 end)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject); // Or pool if you want to optimize later
    }
}

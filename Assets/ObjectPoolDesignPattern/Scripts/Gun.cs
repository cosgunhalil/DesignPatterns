using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    private Transform _transform;
	// Use this for initialization
	void Start ()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine("ShootTheGun");
	}

    private IEnumerator ShootTheGun()
    {
        var wait = new WaitForSeconds(.01f);

        while (true)
        {
            var bullet = BulletPool.Instance.GetBullet();
            bullet.SetPosition(_transform.position);
            bullet.SetMovementDirection(new Vector2(UnityEngine.Random.Range(-180,180f), UnityEngine.Random.Range(-180,180f)).normalized);
            BulletEventManager.Instance.BulletFired(bullet);
            yield return wait;
        }
    }
	
}

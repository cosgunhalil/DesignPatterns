using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    [SerializeField]
    private List<Bullet> _bulletsOnScene;

	void Start () {

        _bulletsOnScene = new List<Bullet>();
        BulletEventManager.Instance.OnBulletFired += BulletFired;
        BulletEventManager.Instance.OnBulletDeleted += DeleteBullet;
	}

    private void OnDestroy()
    {
        BulletEventManager.Instance.OnBulletFired -= BulletFired;
    }

    void Update ()
    {
        for (int i = 0; i < _bulletsOnScene.Count; i++)
        {
            _bulletsOnScene[i].Move();
        }
	}

    private void BulletFired(Bullet bullet)
    {
        _bulletsOnScene.Add(bullet);
    }

    private void DeleteBullet(Bullet bullet)
    {
        _bulletsOnScene.Remove(bullet);
    }
}

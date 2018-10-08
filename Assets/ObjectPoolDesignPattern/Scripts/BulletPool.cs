using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    private static BulletPool instance;
    public static BulletPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(BulletPool)) as BulletPool;
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public GameObject BulletPrefab;

    private Stack<Bullet> _bulletStack;

    private void Awake()
    {
        if (instance == null)
        {
            Instance = this;
        }

        SetupPool(10);
        BulletEventManager.Instance.OnBulletDeleted += PoolBullet;

    }

    private void SetupPool(int count)
    {
        _bulletStack = new Stack<Bullet>();

        for (int i = 0; i < count; i++)
        {
            GenerateBullet();
        }
    }

    private void GenerateBullet()
    {
        var bullet = Instantiate(BulletPrefab).GetComponent<Bullet>();
        bullet.Init();
        bullet.ActivateBullet(false);
        _bulletStack.Push(bullet);
    }

    public Bullet GetBullet()
    {
        if (_bulletStack.Count == 0)
        {
            GenerateBullet();
        }
       
        var bullet = _bulletStack.Pop();
        bullet.ActivateBullet(true);
        return bullet;
    }

    public void PoolBullet(Bullet bullet)
    {
        bullet.ActivateBullet(false);
        _bulletStack.Push(bullet);
    }
}

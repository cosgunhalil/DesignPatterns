using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEventManager
{

    public delegate void BulletFiredDelegate(Bullet bullet);
    public event BulletFiredDelegate OnBulletFired;

    public delegate void BulletDeletedDelegate(Bullet bullet);
    public event BulletDeletedDelegate OnBulletDeleted;

    private static readonly BulletEventManager instance = new BulletEventManager();

    static BulletEventManager()
    {
    }

    private BulletEventManager()
    {
    }

    public static BulletEventManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void BulletFired(Bullet bullet)
    {
        if (OnBulletFired != null)
        {
            OnBulletFired(bullet);
        }
    }

    public void BulletDeleted(Bullet bullet)
    {
        if (OnBulletDeleted != null)
        {
            OnBulletDeleted(bullet);
        }
    }

}

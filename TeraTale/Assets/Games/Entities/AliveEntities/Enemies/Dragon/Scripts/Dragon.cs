﻿using System.Collections.Generic;
using TeraTaleNet;
using UnityEngine;

public class Dragon : Enemy
{
    public Projectile pfMeteor;
    public FireBreath fireBreath;

    Collider _ardColl;
    public override void OnAttackAnimationEnd(Collider ardColl)
    {
        _ardColl = ardColl;
        Invoke("ARDCollEnable", Random.Range(2, 8));
    }

    void ARDCollEnable()
    {
        _ardColl.enabled = true;
    }

    public override void Attack()
    {
        base.Attack();
        _animator.SetInteger("AttackType", Random.Range(0, 2));
    }

    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        if (mainTarget)
        {
            var vec = mainTarget.transform.position - transform.position;
            transform.LookAt(Vector3.Slerp(transform.forward, vec.normalized, 0.3f) + transform.position);
        }
    }

    void MeteorStart()
    {
        for (int i = 0; i < 8; i++)
            Invoke("CastMeteor", Random.Range(0f, 2f));
    }

    void CastMeteor()
    {
        var meteor = Instantiate(pfMeteor);
        var xzSeed = Random.Range(0, Mathf.PI * 2);
        meteor.transform.position = transform.position + new Vector3(Mathf.Sin(xzSeed), 0, Mathf.Cos(xzSeed)) * Random.Range(0f, 10f) + Vector3.up * 50;
        meteor.direction = new Vector3(0, -1, 0);
    }

    void FireBreathBegin()
    {
        fireBreath.On();
    }

    void FireBreathEnd()
    {
        fireBreath.Off();
    }

    protected override List<Item> itemsForDrop
    {
        get
        {
            List<Item> ret = new List<Item>();
            if (Random.Range(0, 2) == 0)
                ret.Add(new HpPotion());
            ret.Add(new Apple());
            return ret;
        }
    }

    protected override float levelForDrop
    { get { return 10; } }
}
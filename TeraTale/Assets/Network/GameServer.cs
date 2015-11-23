﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TeraTaleNet;

public class GameServer : UnityServer
{
    GameServerHandler _handler;
    Messenger _messenger;
    Dictionary<string, HashSet<string>> playersByWorld;
    bool _disposed = false;

    protected override void OnStart()
    {
        _handler = new GameServerHandler();
        _messenger = new Messenger(_handler);

        _messenger.Register("Database", Connect("127.0.0.1", Port.DatabaseForGameServer));
        Debug.Log("Database connected.");
        _messenger.Register("Login", Connect("127.0.0.1", Port.LoginForGameServer));
        Debug.Log("Login connected.");

        Bind("127.0.0.1", Port.GameServer, 1);
        _messenger.Register("Proxy", Listen());
        Debug.Log("Proxy connected.");
        
        StartCoroutine(Dispatcher("Login"));
        
        StartCoroutine(Dispatcher("Proxy"));

        FindObjectOfType<Certificator>().enabled = true;

        _messenger.Start();
    }

    protected override void OnEnd()
    {
        StopAllCoroutines();
        _messenger.Dispose();
    }

    IEnumerator Dispatcher(string key)
    {
        while (true)
        {
            _messenger.DispatcherCoroutine(key);
            yield return new WaitForSeconds(0);
        }
    }

    protected override void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            try
            {
                if (disposing)
                {
                    _messenger.Join();
                }
                _disposed = true;
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
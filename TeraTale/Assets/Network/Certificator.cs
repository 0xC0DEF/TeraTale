﻿using UnityEngine;
using System;
using System.Collections;
using TeraTaleNet;

public class Certificator : NetworkScript, MessageHandler, IDisposable
{
    public Player pfPlayer;
    NetworkAgent _agent = new NetworkAgent();
    Messenger _messenger;
    object _locker = new object();
    int _confirmID;

    protected override void OnStart()
    {
        _messenger = new Messenger(this);
        
        _messenger.Register("Proxy", _agent.Connect("127.0.0.1", Port.Proxy));
        Console.WriteLine("Proxy connected.");

        foreach (var key in _messenger.Keys)
            StartCoroutine(Dispatcher(key));

        _messenger.Start();
    }

    IEnumerator Dispatcher(string key)
    {
        while (true)
        {
            lock (_locker)
                _messenger.DispatcherCoroutine(key);
            yield return new WaitForSeconds(0);
        }
    }

    protected override void OnEnd()
    {
        StopAllCoroutines();
        _messenger.Join();
    }

    protected override void OnUpdate()
    {
        if (Console.KeyAvailable)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                Stop();
        }
    }

    public void SendLoginRequest(string id, string pw)
    {
        _messenger.Send("Proxy", new LoginQuery(id, pw, _confirmID));
    }

    public void Dispose()
    {
        _messenger.Join();
        _agent.Dispose();
        GC.SuppressFinalize(this);
    }

    void ConfirmID(Messenger messenger, string key, ConfirmID confirmID)
    {
        _confirmID = confirmID.id;
    }

    void LoginAnswer(Messenger messenger, string key, LoginAnswer answer)
    {
        //If failed, Show Failed Message.
        if (answer.accepted)
        {
            var net = FindObjectOfType<Client>();
            lock (_locker)
                net.stream = messenger.Unregister("Proxy");
            net.enabled = true;

            Application.LoadLevel(answer.world);

            Player player = Instantiate(pfPlayer);
            DontDestroyOnLoad(player.gameObject);
        }
    }
}
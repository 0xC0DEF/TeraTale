﻿using System;
using System.Threading;
using LoboNet;

namespace TeraTaleNet
{
    public abstract class Server : IServer
    {
        bool _stopped = false;

        public void Execute()
        {
            OnStart();
            try
            {
                while (_stopped == false)
                {
                    OnUpdate();
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            OnEnd();
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnEnd();

        protected void Stop()
        {
            _stopped = true;
        }

        protected PacketStream Listen(string ip, Port port, int backlog)
        {
            var _listener = new TcpListener(ip, (ushort)port, backlog);
            var connection = _listener.Accept();
            _listener.Dispose();

            return new PacketStream(connection);
        }

        protected PacketStream Connect(string ip, Port port)
        {
            var _connecter = new TcpConnecter();
            var connection = _connecter.Connect(ip, (ushort)port);
            _connecter.Dispose();

            return new PacketStream(connection);
        }
    }
}
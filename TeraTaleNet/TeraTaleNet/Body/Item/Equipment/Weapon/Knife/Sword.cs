﻿namespace TeraTaleNet
{
    public class Sword : Weapon
    {
        public sealed override Type weaponType { get { return Type.knife; } }

        public Sword()
        { }

        public Sword(byte[] data)
            : base(data)
        { }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyDroneState 
{
    public enum StateEnum
    {
        none, Idle, Forward, ForwardEx
    }
    //triggers
    public struct Triggers_Movement
    {
        public event Action OnSpeedStart;
        public event Action OnOutOfControl;
       
    }
    public struct Triggers_CombatNEffects
    {
        public event Action OnRecoil;
        public event Action OnDamaged1;
        public event Action OnDamaged2;
        public event Action OnCrash;
    }
	
}

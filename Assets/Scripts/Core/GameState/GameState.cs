using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    [Serializable]
    public enum GameStateEnum
    {
        MainMenu = 0,
        Initialization,
        Reinitialization,
        Process,
        AnimatePlayer,
        AnimateEnemy,
        Transit,
        Termination,
    }
}
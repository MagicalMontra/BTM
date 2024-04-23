using System;
using Rewired;
using UnityEngine;
using Zenject;
using RePlayer = Rewired.Player;

namespace HotPlay.BoosterMath.Core.UI
{
    public class RewirdInputController : IInitializable, ITickable
    {
        public event Action BackKeyDown;
        public event Action BackKey;
        public event Action BackKeyUp;

        public event Action SubmitKeyDown;
        public event Action SubmitKey;
        public event Action SubmitKeyUp;

        public event Action LeftKeyDown;
        public event Action LeftKey;
        public event Action LeftKeyUp;

        public event Action RightKeyDown;
        public event Action RightKey;
        public event Action RightKeyUp;

        public event Action UpKeyDown;
        public event Action UpKey;
        public event Action UpKeyUp;

        public event Action DownKeyDown;
        public event Action DownKey;
        public event Action DownKeyUp;
        
        private bool backKeyPress = false;
        private bool submitKeyPress = false;
        private bool leftKeyPress = false;
        private bool rightKeyPress = false;
        private bool upKeyPress = false;
        private bool downKeyPress = false;

        private RePlayer rePlayer;

        public void Initialize()
        {
            rePlayer ??= ReInput.players.GetPlayer(0);
        }

        public void Tick()
        {
            RaisePressEvent(ref backKeyPress, rePlayer.GetButton("Cancel"), BackKeyDown, BackKey, BackKeyUp);
            RaisePressEvent(ref submitKeyPress, rePlayer.GetButton("Submit"), SubmitKeyDown, SubmitKey, SubmitKeyUp);
            RaiseThresholdEvent(ref rightKeyPress, ref leftKeyPress, rePlayer.GetAxis("Horizontal"), 
                RightKeyDown, 
                RightKey, 
                RightKeyUp,
                LeftKeyDown,
                LeftKey,
                LeftKeyUp
            );

            RaiseThresholdEvent(ref upKeyPress, ref downKeyPress, rePlayer.GetAxis("Vertical"),
                UpKeyDown,
                UpKey,
                UpKeyUp,
                DownKeyDown,
                DownKey,
                DownKeyUp
            );
        }
        
        private void RaisePressEvent(ref bool flag, bool isPressed, Action keyDown, Action key, Action keyUp)
        {
            if (!flag && isPressed)
            {
                flag = true;
                keyDown?.Invoke();
            }
            if (flag && isPressed)
            {
                key?.Invoke();
            }
            if (flag && !isPressed)
            {
                flag = false;
                keyUp?.Invoke();
            }
        }

        private void RaiseThresholdEvent(ref bool positive, ref bool negative, float axis, 
            Action positiveKeyDown, Action positiveKey, Action positiveKeyUp,
            Action negativeKeyDown, Action negativeKey, Action negativeKeyUp)
        {
            if (!positive && axis > 0)
            {
                positive = true;
                positiveKeyDown?.Invoke();
            }
            if (positive && axis > 0)
            {
                positiveKey?.Invoke();
            }
            if (positive && Mathf.Approximately(axis, 0))
            {
                positive = false;
                positiveKeyUp?.Invoke();
            }

            if (!negative && axis < 0) 
            {
                negative = true;
                negativeKeyDown?.Invoke();
            }
            if (negative && axis < 0)
            {
                negativeKey?.Invoke();
            }
            if (negative && Mathf.Approximately(axis, 0))
            {
                negative = false;
                negativeKeyUp?.Invoke();
            }
        }
    }
}
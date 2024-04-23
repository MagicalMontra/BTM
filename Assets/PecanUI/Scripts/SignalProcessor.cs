using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class SignalProcessor : MonoBehaviour
    {
        [SerializeField]
        private SignalProcessorDatabase database;

        private bool isTransiting;
        private bool isInitialized;
        private string openingDialogName;

        private void Awake()
        {
            if (isInitialized)
                return;
            
            foreach (var signal in database.Data)
            {
                signal.Initialize(OnRequest);
            }

            isInitialized = true;
        }

        private void OnDestroy()
        {
            foreach (var signal in database.Data)
            {
                signal.Dispose();
            }
        }

        private bool OnRequest(string dialogName)
        {
            if (string.IsNullOrEmpty(openingDialogName))
                openingDialogName = dialogName;
            
            var allowTransition = !isTransiting && openingDialogName == dialogName;
            openingDialogName = dialogName;
            isTransiting = true;
            return allowTransition && isInitialized;
        }

        public void OnResponse(string dialogName)
        {
            if (string.IsNullOrEmpty(dialogName))
                return;
            
            if (openingDialogName != dialogName)
                return;
            
            if (!isTransiting)
                return;

            openingDialogName = "";
            isTransiting = false;
        }
    }
}
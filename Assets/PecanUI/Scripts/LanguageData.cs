#nullable enable
using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    [Serializable]
    public class LanguageData
    {
        [SerializeField, Tooltip("Assign localize key for localization")]
        private string languageName = default!;
        public string LanguageName => languageName;

        [SerializeField]
        private string languageCode = default!;
        public string LanguageCode => languageCode;
    }
}
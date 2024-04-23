using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    public partial class SkinConfigs
    {
        [TabGroup("Settings")]
        [Title("Language Selection", subtitle: "Key: setting_lang_selection", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData settingLanguageSelection;
        public PanelSkinData SettingLanguageSelection => settingLanguageSelection;

        [TabGroup("Settings")]
        [Title("Language Selection label", subtitle: "Key: setting_lang_selection_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData settingLanguageSelectionLabel;
        public TMPSkinData SettingLanguageSelectionLabel => settingLanguageSelectionLabel;

        [TabGroup("Settings")]
        [Title("Support Button", subtitle: "Key: support_button", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ButtonSkinData supportButton;
        public ButtonSkinData SupportButton => supportButton;

        [TabGroup("Settings")]
        [Title("Support Button Label", subtitle: "Key: support_button_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData supportButtonLabel;
        public TMPSkinData SupportButtonLabel => supportButtonLabel;
    }
}

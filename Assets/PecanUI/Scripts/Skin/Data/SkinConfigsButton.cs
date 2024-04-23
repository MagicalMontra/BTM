using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    public partial class SkinConfigs
    {
        [TabGroup("Buttons")]
        [Title("Close Button", subtitle: "Key: close_button", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ButtonSkinData closeButton;
        public ButtonSkinData CloseButton => closeButton;

        [Space(30)]

        [TabGroup("Buttons")]
        [Title("Big Button", subtitle: "Key: big_button", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ButtonSkinData bigButton;
        public ButtonSkinData BigButton => bigButton;

        [TabGroup("Buttons")]
        [Title("Big Label", subtitle: "Key: big_button_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData bigButtonLabel;
        public TMPSkinData BigButtonLabel => bigButtonLabel;

        [Space(30)]

        [TabGroup("Buttons")]
        [Title("Common Button", subtitle: "Key: common_button", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ButtonSkinData commonButton;
        public ButtonSkinData CommonButton => commonButton;

        [TabGroup("Buttons")]
        [Title("Common Label", subtitle: "Key: common_button_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData commonButtonLabel;
        public TMPSkinData CommonButtonLabel => commonButtonLabel;

        [Space(30)]

        [TabGroup("Buttons")]
        [Title("Positive Button", subtitle: "Key: positive_button", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ButtonSkinData positiveButton;
        public ButtonSkinData PositiveButton => positiveButton;

        [TabGroup("Buttons")]
        [Title("Positive Label", subtitle: "Key: positive_button_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData positiveButtonLabel;
        public TMPSkinData PositiveButtonLabel => positiveButtonLabel;

        [Space(30)]

        [TabGroup("Buttons")]
        [Title("Negative Button", subtitle: "Key: negative_button", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private ButtonSkinData negativeButton;
        public ButtonSkinData NegativeButton => negativeButton;

        [TabGroup("Buttons")]
        [Title("Negative Label", subtitle: "Key: negative_button_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData negativeButtonLabel;
        public TMPSkinData NegativeButtonLabel => negativeButtonLabel;
    }
}

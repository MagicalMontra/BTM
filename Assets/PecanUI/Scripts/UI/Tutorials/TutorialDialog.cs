using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.Analytic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI
{
    public class TutorialDialog : BaseDialog
    {
        [SerializeField]
        private TutorialContainer[] tutorialContainers;

        private Dictionary<TutorialContainerType, TutorialContainer> containerLookUp;

        private TutorialData[] tutorialData;

        private TutorialContainer currentContainer;

        protected override void Awake()
        {
            base.Awake();
            
            containerLookUp = new Dictionary<TutorialContainerType, TutorialContainer>();

            foreach (var container in tutorialContainers)
            {
                containerLookUp.Add(container.ContainerType, container);
            }
        }

        public void SetupData(TutorialData tutorialData)
        {
            SetupData(new TutorialData[] { tutorialData });
        }

        public void SetupData(TutorialData[] tutorialData)
        {
            this.tutorialData = tutorialData;
        }

        protected override void OnShowing()
        {
            base.OnShowing();

            Debug.Assert(tutorialData != null & tutorialData.Length > 0, $"Missing tutorial data");

            TutorialContainerType targetType = TutorialContainerType.Wizard;

            if (tutorialData.Length == 1)
            {
                targetType = TutorialContainerType.Float;
            }

            foreach (var lookup in containerLookUp)
            {
                if (lookup.Key == targetType)
                {
                    lookup.Value.Container.Show();
                    var targetContainer = lookup.Value;

                    if (targetContainer is TutorialWizardContainer)
                    {
                        (targetContainer as TutorialWizardContainer).Setup(tutorialData);
                    }
                    else
                    {
                        (targetContainer as TutorialFloatContainer).Setup(tutorialData[0]);
                    }

                    currentContainer = targetContainer;
                    currentContainer.OnShowing();
                }
                else
                {
                    lookup.Value.Container.InstantHide();
                }
            }
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            if (currentContainer != null)
            {
                currentContainer.OnVisible();
            }
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            
            if (currentContainer != null)
            {
                currentContainer.OnHidden();
            }

            currentContainer = null;

            foreach (var lookup in containerLookUp)
            {
                lookup.Value.Container.InstantHide();
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            if (currentContainer != null)
            {
                currentContainer.OnHide();
            }

            PecanServices.Instance.Signals.SendTutorialClosedSignal();
        }

        protected override void RecoveryNavigator()
        {
            if (currentContainer != null)
            {
                currentContainer.RecoveryNavigator();
            }
        }
    }
}

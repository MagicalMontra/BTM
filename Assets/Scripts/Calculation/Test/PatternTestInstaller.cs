using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation.Test
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/UniTest/Create PatternTestInstaller", fileName = "PatternTestInstaller", order = 0)]
    public class PatternTestInstaller : ScriptableObjectInstaller<PatternTestInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IQuestionPattern>().To<XDivideYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYYZZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMinusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMultiplyYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMultiplyYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYPlusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYMinusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYYMinusZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYMinusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYYPlusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYDivideZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XxDivideYPlusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYDivideZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYYMinusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMultiplyYMultiplyZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMultiplyYYZZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYYMinusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYYMultiplyZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYZZQuestionPattern>().AsTransient();
        }
    }
}
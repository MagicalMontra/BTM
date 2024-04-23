using HotPlay.QuickMath.Calculation;

namespace HotPlay.BoosterMath.Core.UI
{
    public class HardModeInstaller : ModeInstallerBase
    {
        internal override void PatternBind()
        {
            Container.Bind<IQuestionPattern>().To<XXPlusYMinusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XPlusYYMinusZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XxDivideYPlusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYYMultiplyZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYDivideZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMultiplyYMultiplyZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYYMinusZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYDivideZQuestionPattern>().AsTransient();
        }
    }
}
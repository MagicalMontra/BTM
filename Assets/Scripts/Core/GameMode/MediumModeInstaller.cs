using HotPlay.QuickMath.Calculation;

namespace HotPlay.BoosterMath.Core.UI
{
    public class MediumModeInstaller : ModeInstallerBase
    {
        internal override void PatternBind()
        {
            Container.Bind<IQuestionPattern>().To<XXPlusYYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXPlusYYZZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMultiplyYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMultiplyYMultiplyZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYYZQuestionPattern>().AsTransient();
        }
    }
}
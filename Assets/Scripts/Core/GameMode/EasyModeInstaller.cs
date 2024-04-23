using System.Collections.Generic;
using HotPlay.QuickMath.Calculation;

namespace HotPlay.BoosterMath.Core.UI
{
    public class EasyModeInstaller : ModeInstallerBase
    {
        internal override void PatternBind()
        {
            Container.Bind<IQuestionPattern>().To<XPlusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMinusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXMinusYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMultiplyYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XMultiplyYZZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XDivideYZQuestionPattern>().AsTransient();
            Container.Bind<IQuestionPattern>().To<XXDivideYZQuestionPattern>().AsTransient();
        }
    }
}
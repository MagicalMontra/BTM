using System.Collections.Generic;
using System.Linq;
using Zenject;
using NUnit.Framework;
using UnityEngine;

namespace HotPlay.QuickMath.Calculation.Test
{
    [TestFixture]
    public class EasyModeTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Install()
        {
            PatternTestInstaller.InstallFromResource(Container);
        }

        [Test]
        public void TestInstaller()
        {
            var list = Container.Resolve<List<IQuestionPattern>>();
            Assert.NotNull(list);
            Assert.Greater(list.Count, 0);
        }

        [TestCase(9, 1, TestName = "X+Y=Zx1")]
        [TestCase(9, 50, TestName = "X+Y=Zx50")]
        public void TestXPlusYEqualZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XPlusYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XPlusYZQuestionPattern), pattern.GetType());

            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(1, TestName = "X+Y=ZZx1")]
        [TestCase(50, TestName = "X+Y=ZZx50")]
        public void TestXPlusYEqualZZ(int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XPlusYZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XPlusYZZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(0);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(1, TestName = "X-Y=Zx1")]
        [TestCase(50, TestName = "X-Y=Zx50")]
        public void TestXMinusYEqualZ(int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XMinusYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XMinusYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(0);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(9, 1, TestName = "XX-Y=Zx1")]
        [TestCase(9, 50, TestName = "XX-Y=Zx50")]
        public void TestXXMinusYEqualZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMinusYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMinusYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(1, TestName = "XxY=Zx1")]
        [TestCase(50, TestName = "XxY=Zx50")]
        public void TestXMultiplyYEqualZ(int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XMultiplyYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XMultiplyYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(9);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Multiply);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(20, 1, TestName = "XxY=ZZx1")]
        [TestCase(20, 50, TestName = "XxY=ZZx50")]
        public void TestXMultiplyYEqualZZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XMultiplyYZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XMultiplyYZZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Multiply);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(1, TestName = "X÷Y=Zx1")]
        [TestCase(50, TestName = "X÷Y=Zx50")]
        public void TestXDivideYEqualZ(int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XDivideYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XDivideYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(9);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(1, TestName = "XX÷Y=Zx1")]
        [TestCase(50, TestName = "XX÷Y=Zx50")]
        public void TestXXDivideYEqualZ(int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXDivideYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXDivideYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(9);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
    }
}
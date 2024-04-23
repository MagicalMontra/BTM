using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation.Test
{
    [TestFixture]
    public class MediumModeTest : ZenjectUnitTestFixture
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
        
        [TestCase(50, 1, TestName = "XX+Y=ZZx1")]
        [TestCase(50, 50, TestName = "XX+Y=ZZx50")]
        public void TestXXPlusYEqualZZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXPlusYZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXPlusYZZQuestionPattern), pattern.GetType());

            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, 51);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX+YY=ZZx1")]
        [TestCase(50, 50, TestName = "XX+YY=ZZx50")]
        public void TestXXPlusYYEqualZZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXPlusYYZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXPlusYYZZQuestionPattern), pattern.GetType());

            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, 51);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, 51);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX+YY=ZZZx1")]
        [TestCase(50, 50, TestName = "XX+YY=ZZZx50")]
        public void TestXXPlusYYEqualZZZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXPlusYYZZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXPlusYYZZZQuestionPattern), pattern.GetType());

            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, 100);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, 51);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Assert.AreEqual(data.Result, data.Pairs[0].Number + data.Pairs[1].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(1, TestName = "XX-Y=Zx1")]
        [TestCase(50, TestName = "XX-Y=Zx50")]
        public void TestXXMinusYEqualZ(int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMinusYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMinusYZQuestionPattern), pattern.GetType());

            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(0);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, 50);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX-YY=ZZx1")]
        [TestCase(50, 50, TestName = "XX-YY=ZZx50")]
        public void TestXXMinusYYEqualZZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMinusYYZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMinusYYZZQuestionPattern), pattern.GetType());

            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, 51);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Minus);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, 51);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 9);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }

        [TestCase(50, 1, TestName = "XxY=ZZx1")]
        [TestCase(50, 50, TestName = "XxY=ZZx50")]
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
        
        [TestCase(50, 1, TestName = "XXxYY=ZZZx1")]
        [TestCase(50, 50, TestName = "XXxYY=ZZZx50")]
        [TestCase(50, 500, TestName = "XXxYY=ZZZx500")]
        [TestCase(50, 1000, TestName = "XXxYY=ZZZx1000")]
        public void TestXXMultiplyYYEqualZZZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMultiplyYYZZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMultiplyYYZZZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Multiply);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, maxNumber + 1);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Greater(data.Result, 99);
                Assert.Less(data.Result, 1000);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX÷Y=Zx1")]
        [TestCase(50, 50, TestName = "XX÷Y=Zx50")]
        public void TestXXDivideYEqualZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXDivideYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXDivideYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX÷YY=Zx1")]
        [TestCase(50, 50, TestName = "XX÷YY=Zx50")]
        public void TestXXDivideYYqualZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXDivideYYZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXDivideYYZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Divide);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, maxNumber + 1);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 10);
                Assert.GreaterOrEqual(data.Result, 0);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "X+Y-Z=Ax1")]
        [TestCase(50, 50, TestName = "X+Y-Z=Ax50")]
        public void TestXPlusYMinusZEqualA(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XPlusYMinusZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XPlusYMinusZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.GreaterOrEqual(data.Result, 0);
                Assert.AreEqual(data.Result, data.Pairs[0].Number + data.Pairs[1].Number - data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "X+YY-Z=Ax1")]
        [TestCase(50, 50, TestName = "X+YY-Z=Ax50")]
        public void TestXPlusYYMinusZEqualA(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XPlusYYMinusZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XPlusYYMinusZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, maxNumber + 1);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.GreaterOrEqual(data.Result, 0);
                Assert.AreEqual(data.Result, data.Pairs[0].Number + data.Pairs[1].Number - data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
    }
}
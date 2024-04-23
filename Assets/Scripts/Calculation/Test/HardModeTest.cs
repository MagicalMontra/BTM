using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace HotPlay.QuickMath.Calculation.Test
{
    [TestFixture]
    public class HardModeTest : ZenjectUnitTestFixture
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
        
        [TestCase(50, 1, TestName = "XX+Y-Z=Ax1")]
        [TestCase(50, 50, TestName = "XX+Y-Z=Ax50")]
        public void TestXXPlusYMinusZEqualA(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXPlusYMinusZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXPlusYMinusZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
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
                Assert.AreEqual(data.Result, data.Pairs[0].Number + data.Pairs[1].Number - data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "X+YY-ZZ=Ax1")]
        [TestCase(50, 50, TestName = "X+YY-ZZ=Ax50")]
        public void TestXPlusYYMinusZZEqualA(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XPlusYYMinusZZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XPlusYYMinusZZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, 10);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[1].Number, maxNumber + 1);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Minus);
                Assert.Greater(data.Pairs[2].Number, 9);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.AreEqual(data.Result, data.Pairs[0].Number + data.Pairs[1].Number - data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX+Y+Z=Ax1")]
        [TestCase(50, 50, TestName = "XX+Y+Z=Ax50")]
        public void TestXXDivideYPlusZEqualA(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XxDivideYPlusZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XxDivideYPlusZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Plus);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.AreEqual(data.Result, data.Pairs[0].Number / data.Pairs[1].Number + data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX+YYxZ=Ax1")]
        [TestCase(50, 50, TestName = "XX+YYxZ=Ax50")]
        public void TestXXPlusYYMultiplyZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXPlusYYMultiplyZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXPlusYYMultiplyZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Plus);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, maxNumber + 1);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Multiply);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.AreEqual(data.Result, data.Pairs[0].Number + data.Pairs[1].Number * data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX-Y÷Z=Ax1")]
        [TestCase(50, 50, TestName = "XX-Y÷Z=Ax50")]
        public void TestXXMinusYYMultiplyZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMinusYDivideZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMinusYDivideZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.AreEqual(data.Result, data.Pairs[0].Number - data.Pairs[1].Number / data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XXxYxZ=Ax1")]
        [TestCase(50, 50, TestName = "XXxYxZ=Ax50")]
        public void TestXXMultiplyYMultiplyZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMultiplyYMultiplyZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMultiplyYMultiplyZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Multiply);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Multiply);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.Less(data.Result, 1000);
                Assert.AreEqual(data.Result, data.Pairs[0].Number * data.Pairs[1].Number * data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX-YY-Z=Ax1")]
        [TestCase(50, 50, TestName = "XX-YY-Z=Ax50")]
        public void TestXXMinusYMinusZ(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXMinusYYMinusZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXMinusYYMinusZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Minus);
                Assert.Greater(data.Pairs[1].Number, 9);
                Assert.Less(data.Pairs[1].Number, maxNumber + 1);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Minus);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.AreEqual(data.Result, data.Pairs[0].Number - data.Pairs[1].Number - data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
        
        [TestCase(50, 1, TestName = "XX÷Y÷Z=Ax1")]
        [TestCase(50, 50, TestName = "XX÷Y÷Z=Ax50")]
        public void TestXXDivideYDivideZQuestionPattern(int maxNumber, int loopTime)
        {
            var patterns = Container.Resolve<List<IQuestionPattern>>();
            var pattern = patterns.First(p => p.GetType() == typeof(XXDivideYDivideZQuestionPattern));
            Assert.NotNull(pattern);
            Assert.AreEqual(typeof(XXDivideYDivideZQuestionPattern), pattern.GetType());
            
            int count = 0;
            while (count < loopTime)
            {
                var data = pattern.GetQuestion(maxNumber);
                Assert.Greater(data.Pairs[0].Number, 9);
                Assert.Less(data.Pairs[0].Number, maxNumber + 1);
                Assert.That(data.Pairs[0].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[1].Number, 10);
                Assert.That(data.Pairs[1].Symbol == OperatorEnum.Divide);
                Assert.Less(data.Pairs[2].Number, 10);
                Assert.That(data.Pairs[2].Symbol == OperatorEnum.Equal);
                Assert.AreEqual(data.Result, data.Pairs[0].Number / data.Pairs[1].Number / data.Pairs[2].Number);
                Debug.Log($"{data.Pairs[0].Number} {data.Pairs[0].Symbol} {data.Pairs[1].Number} {data.Pairs[1].Symbol} {data.Pairs[2].Number} {data.Pairs[2].Symbol} {data.Result}");
                count++;
            }
        }
    }
}
using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            var firstTimer = RunTimerTest(3);
            var secondTimer = RunTimerTest(100);
            var thirdTimer = RunTimerTest(250);

            Assert.IsFalse(firstTimer.Synced > firstTimer.Paralleled);
            Assert.IsFalse(secondTimer.Synced > secondTimer.Paralleled);
            Assert.IsTrue(thirdTimer.Synced > thirdTimer.Paralleled);
        }

        #region private methods

        private (long Synced, long Paralleled) RunTimerTest(int matrixSize)
        {
            var syncedMatrixMult = new MatricesMultiplier();
            var paralleledMatrixMult = new MatricesMultiplierParallel();

            var firstTestMatrix = CreateTestMatrix(matrixSize);
            var secondTestMatrix = CreateTestMatrix(matrixSize);

            var watch1 = Stopwatch.StartNew();
            syncedMatrixMult.Multiply(firstTestMatrix, secondTestMatrix);
            watch1.Stop();

            var watch2 = Stopwatch.StartNew();
            paralleledMatrixMult.Multiply(firstTestMatrix, secondTestMatrix);
            watch2.Stop();

            return (watch1.ElapsedTicks, watch2.ElapsedTicks);
        }

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        private IMatrix CreateTestMatrix(int size)
        {
            return new Matrix(size, size, true);
        }

        #endregion
    }
}

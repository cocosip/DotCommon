using DotCommon.Threading.Timers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class DotCommonTimerTest
    {
        private int _testValue = 1;
        [Fact]
        public void DotCommonTimer_Test()
        {
            var timer = new DotCommonTimer(100, false);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Thread.Sleep(300);
            timer.Stop();
            Assert.Equal(2, _testValue);
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            _testValue = 2;
        }
    }
}

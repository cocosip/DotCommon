//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using DotCommon.Extensions;
//using System.Threading;

//namespace DotCommon.Test.Extensions
//{
//    public class TaskFactoryExtensionsTest
//    {
//        [Fact]
//        public void StartDelayedTask_Test()
//        {
//            TaskFactory taskFactory1 = null;

//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                taskFactory1.StartDelayedTask(10, () => { });
//            });

//            Assert.Throws<ArgumentOutOfRangeException>(() =>
//            {
//                Task.Factory.StartDelayedTask(-1, () => { });
//            });

//            Action action1 = null;
//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                Task.Factory.StartDelayedTask(10, action1);
//            });

//            TaskFactory taskFactory2 = new TaskFactory(CancellationToken.None);
//            var t1 = taskFactory2.StartDelayedTask(10, () => { });
//            Assert.Equal(TaskStatus.WaitingForActivation, t1.Status);


//        }
//    }
//}

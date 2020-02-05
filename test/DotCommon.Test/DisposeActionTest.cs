using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test
{
    public class DisposeActionTest
    {
        [Fact]
        public void Dispose_Test()
        {
            var id = 0;
            var disposeAction = new DisposeAction(()=>
            {
                id = 100;
            });
            disposeAction.Dispose();
            Assert.Equal(100, id);


            var disposeAction2 = DisposeAction.Empty;
            disposeAction2.Dispose();


        }
    }
}

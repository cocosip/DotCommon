using DotCommon.Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using Quartz;
using System.Threading.Tasks;

namespace DotCommon.ConsoleTest
{
    public class TestQuartzJob : JobBase
    {

        public override Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Quartz:{DateTime.Now.ToLongTimeString()}");
            return Task.FromResult(0);
        }
    }
}

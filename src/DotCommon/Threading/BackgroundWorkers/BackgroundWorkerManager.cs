﻿using System;
using System.Collections.Generic;

namespace DotCommon.Threading.BackgroundWorkers
{
    public class BackgroundWorkerManager : RunnableBase, IBackgroundWorkerManager, IDisposable
    {
        private readonly List<IBackgroundWorker> _backgroundJobs=new List<IBackgroundWorker>();

        public BackgroundWorkerManager()
        {
        }

        public override void Start()
        {
            base.Start();
            _backgroundJobs.ForEach(job => job.Start());
        }

        public override void Stop()
        {
            _backgroundJobs.ForEach(job => job.Stop());
            base.Stop();
        }

        public override void WaitToStop()
        {
            _backgroundJobs.ForEach(job => job.WaitToStop());
            base.WaitToStop();
        }

        public void Add(IBackgroundWorker worker)
        {
            _backgroundJobs.Add(worker);

            if (IsRunning)
            {
                worker.Start();
            }
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            //释放对象
            // _backgroundJobs.ForEach();
            _backgroundJobs.Clear();
        }
    }
}

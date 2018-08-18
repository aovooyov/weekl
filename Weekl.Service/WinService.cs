using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using NLog;
using Quartz;
using Quartz.Impl;
using Weekl.Service.Jobs;

namespace Weekl.Service
{
    public partial class WinService : ServiceBase
    {
        private ILogger _logger;
        private IScheduler _scheduler;

        public WinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AsyncContext.Run(StartAsync);
        }

        public async Task StartAsync()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            _logger.Info("Start");

            try
            {
                var syncFeedJob = JobBuilder
                    .Create<SyncFeedJob>()
                    .WithDescription("Sync feed")
                    .Build();

                var syncFeedTrigger = TriggerBuilder
                    .Create()
                    .WithCronSchedule(ConfigurationManager.AppSettings.Get("Job.SyncFeed"))
                    .StartNow()
                    .Build();

                await _scheduler.ScheduleJob(syncFeedJob, syncFeedTrigger);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            try
            {
                await _scheduler.Start();
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        protected override void OnStop()
        { 
            if (_scheduler == null)
            {
                return;
            }

            var task = _scheduler.Shutdown(true);

            _logger.Info("Shutdown");

            task.Wait(TimeSpan.FromSeconds(10));
        }
    }
}

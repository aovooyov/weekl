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
using Ninject;
using Nito.AsyncEx;
using NLog;
using Quartz;
using Quartz.Impl;
using Weekl.Core.Repository.Feed.Abstract;
using Weekl.Service.Jobs;
using Weekl.Service.Worker;

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
            AsyncContext.Run(RunByTasksAsync);
        }

        public async Task RunAsync()
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
                    .ForJob(syncFeedJob)
                    .WithIdentity(syncFeedJob.Key.Name)
                    //.WithCronSchedule(ConfigurationManager.AppSettings.Get("Job.SyncFeed"))
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

        public async Task RunByTasksAsync()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            _logger.Info("Start");

            try
            {
                var syncFeedJob = JobBuilder
                    .Create<SyncFeedAsyncJob>()
                    .WithDescription("SyncFeedAsyncJob")
                    .Build();

                var syncFeedTrigger = TriggerBuilder
                    .Create()
                    .ForJob(syncFeedJob)
                    .WithIdentity(syncFeedJob.Key.Name)
                    //.WithCronSchedule(ConfigurationManager.AppSettings.Get("Job.SyncFeed"))
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

        public async Task RunByChannelAsync()
        {
            var sourceRepository = WorkerContainer.Current.Get<ISourceRepository>();

            _logger = LogManager.GetCurrentClassLogger();
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            _logger.Info("Start");

            var sources = sourceRepository.List();

            foreach (var source in sources)
            {
                try
                {
                    var job = JobBuilder
                        .Create<SyncFeedJob>()
                        .UsingJobData(new JobDataMap
                        {
                            {"SourceId", source.Id}
                        })
                        .WithDescription($"Sync feed source {source.Unique}")
                        .Build();

                    var trigger = TriggerBuilder
                        .Create()
                        .ForJob(job)
                        .WithIdentity(job.Key.Name)
                        .StartNow()
                        .Build();

                    if (await _scheduler.CheckExists(job.Key))
                    {
                        await _scheduler.DeleteJob(job.Key);
                    }

                    await _scheduler.ScheduleJob(job, trigger);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
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

﻿using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Controller;
using WoWonder.Helpers.Utils;
using WoWonder.SQLite;
using WoWonderClient;
using Exception = Java.Lang.Exception;
using Handler = Android.OS.Handler;
using Object = Java.Lang.Object;

namespace WoWonder.Services
{
    [Service(Exported = true, ForegroundServiceType = ForegroundService.TypeRemoteMessaging, Permission = "android.permission.BIND_JOB_SERVICE")]
    public class AppApiService : JobService
    {
        private static AppApiService Instance;

        public AppApiService()
        {
            Instance = this;
        }

        public static AppApiService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AppApiService();
            }
            return Instance;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            try
            {
                base.OnStartCommand(intent, flags, startId);

                // Perform your background task here
                ThreadPool.RunOnUiThread(new AppUpdaterHelper(Application.Context));

                return StartCommandResult.Sticky;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return StartCommandResult.NotSticky;
            }
        }

        public override bool OnStartJob(JobParameters jobParams)
        {
            //Toast.MakeText(Application.Context, "On Start Job " + Methods.AppLifecycleObserver.AppState, ToastLength.Short)?.Show();

            // Perform your background task here
            ThreadPool.RunOnUiThread(new AppUpdaterHelper(Application.Context));

            // IMPORTANT: Call jobFinished() when your background task is complete
            JobFinished(jobParams, true);

            // Our task will run in background, we will take care of notifying the finish.
            return true;
        }

        public override bool OnStopJob(JobParameters jobParams)
        {
            //Toast.MakeText(Application.Context, "On Stop Job 321" + Methods.AppLifecycleObserver.AppState, ToastLength.Short)?.Show();
            // I want it to reschedule so returned true, if we would have returned false, then job would have ended here.
            // It would not fire onStartJob() when constraints are re satisfied.

            return true;
        }

        public void StartForegroundService(Context context)
        {
            try
            {
                Intent serviceIntent = new Intent(context, typeof(AppApiService));
                serviceIntent.PutExtra("inputExtra", "Foreground Service");
                context.StartService(serviceIntent);

                ScheduleJob(context);
            }
            catch (ForegroundServiceStartNotAllowedException e)
            {
                Methods.DisplayReportResultTrack(e);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void ScheduleJob(Context context)
        {
            try
            {
                ComponentName serviceComponent = new ComponentName(context, Class.FromType(typeof(AppApiService)));

                //JobScheduler js = (JobScheduler)context.GetSystemService(Context.JobSchedulerService);
                //JobInfo.Builder builder = new JobInfo.Builder(0, serviceComponent);
                //builder.AddTriggerContentUri(new JobInfo.TriggerContentUri(MediaStore.Images.Media.ExternalContentUri, TriggerContentUriFlags.NotifyForDescendants));
                //js.Schedule(builder.Build());

                JobInfo jobInfo;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    JobInfo.Builder builder = new JobInfo.Builder(1021, serviceComponent);

                    // Set the job parameters
                    builder.SetRequiredNetworkType(NetworkType.Any);  // Requires network connectivity
                    //builder.SetRequiresCharging(false);  // Requires device to be charging
                    // builder.SetPeriodic(15 * 60 * 1000, 60 * 1000);  // Repeat every 15 minutes
                    builder.SetMinimumLatency(3600000);  //5 * 1000 Minimum latency of 5 minutes
                    //builder.SetRequiresDeviceIdle(false);  // the device should be idle

                    //builder.SetPersisted(true);  // Keep the job after a device reboot
                    jobInfo = builder?.Build();
                }
                else
                {
                    jobInfo = new JobInfo.Builder(1021, serviceComponent)?.SetPeriodic(3600000)?.Build();
                }

                var jobScheduler = (JobScheduler)context.GetSystemService(JobSchedulerService);
                if (jobInfo != null)
                {
                    var resultCode = jobScheduler?.Schedule(jobInfo);
                    if (resultCode == JobScheduler.ResultSuccess)
                    {
                        Console.WriteLine("MyJobService scheduled!");
                    }
                    else
                    {
                        Console.WriteLine("MyJobService not scheduled!");
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void StopJob(Context context)
        {
            try
            {
                var jobScheduler = (JobScheduler)context.GetSystemService(JobSchedulerService);
                jobScheduler?.CancelAll();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class AppUpdaterHelper : Object, IRunnable
    {
        private static Context Context;

        public AppUpdaterHelper(Context context)
        {
            Context = context;
        }

        public void Run()
        {
            try
            {
                if (string.IsNullOrEmpty(Methods.AppLifecycleObserver.AppState))
                    Methods.AppLifecycleObserver.AppState = "Background";

                //ToastUtils.ShowToast(Context, "AppState " + Methods.AppLifecycleObserver.AppState, ToastLength.Short);

                if (string.IsNullOrEmpty(InitializeWoWonder.WebsiteUrl))
                {
                    InitializeWoWonder.Initialize(AppSettings.TripleDesAppServiceProvider, Context.PackageName, AppSettings.TurnTrustFailureOnWebException, MyReportModeApp.CreateInstance());
                    var sqLiteDatabase = new SqLiteDatabase();
                    sqLiteDatabase.CheckTablesStatus();
                }

                if (Methods.AppLifecycleObserver.AppState == "Background" || string.IsNullOrEmpty(Current.AccessToken))
                {
                    SqLiteDatabase dbDatabase = new SqLiteDatabase();
                    var login = dbDatabase.Get_data_Login_Credentials();
                    Console.WriteLine(login);
                }

                if (string.IsNullOrEmpty(Current.AccessToken))
                    return;

                if (Methods.CheckConnectivity())
                {
                    var instance = TabbedMainActivity.GetInstance();
                    if (Methods.AppLifecycleObserver.AppState == "Foreground" && instance != null)
                    {
                        if (instance.NotificationsTab != null) PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => instance.Get_Notifications() });
                    }
                }

                ThreadPool.RunOnUiThread(this);
            }
            catch (Exception e)
            {
                ThreadPool.RunOnUiThread(this);

                //ToastUtils.ShowToast(Context, "ResultSender failed",ToastLength.Short); 
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public abstract class ThreadPool
    {
        private static Handler SUiThreadHandler;

        private ThreadPool()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runnable"></param>
        public static void RunOnUiThread(AppUpdaterHelper runnable)
        {
            try
            {
                SUiThreadHandler ??= new Handler(Looper.MainLooper);
                SUiThreadHandler.PostDelayed(runnable, 6000);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }
}
using Autofac;
using Caliburn.Micro;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using youtube_dl.WPF.Core.Services;
using youtube_dl.WPF.Presentation.Services;
using youtube_dl.WPF.Presentation.ViewModels;
using youtube_dl.WPF.Presentation.Views;

namespace youtube_dl.WPF.Presentation.Composition.Autofac
{
    // Autofac Documentation:   http://autofac.readthedocs.org/en/latest/index.html
    // Autofac source code:     https://github.com/autofac/Autofac

    // TODO: dispose services when shutting down/no longer needed, maybe in OnExit?
    // TODO: MAKE SURE IDisposables implementations which implement interfaces none of which implements IDisposable GET DISPOSED
    // TODO: separate configuration from DisplayRootViewFor
    // TODO: export shell config
    internal sealed class AutofacBootstrapper : CustomBootstrapperBase<ShellViewModel>
    {
        #region ctor

        public AutofacBootstrapper()
        {
            // TODO settings file
            //this.RootViewDIsplaySettings = new Dictionary<string, object>
            //{
            //    { nameof(Window.Height), 900 },
            //    { nameof(Window.Width), 1440 },
            //    { nameof(Window.WindowState), WindowState.Maximized }
            //};
        }

        #endregion

        #region methods

        #region lifetime

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);
        }

        #endregion

        protected override void RegisterComponents(ContainerBuilder builder)
        {
            base.RegisterComponents(builder);

            // MODULES

            //builder.RegisterModule<EventAggregationAutoSubscriptionModule>(); // TODO: review: automatic behavior with no counterpart for unsubscription

            // CORE COMPONENTS

            builder.Register<IWindowManager>(c => new CustomWindowManager()).InstancePerLifetimeScope();
            
            builder.RegisterType<DownloadQueueService>().As<IDownloadQueueService>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadHistoryService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<YouTubeDLService>().As<IYouTubeDLService>().InstancePerLifetimeScope();

            // ViewModels
            
            builder.RegisterType<AddDownloadQueueEntryViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DownloadQueueViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DownloadViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UtilsViewModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ShellViewModel>().AsSelf().InstancePerLifetimeScope();
            
            // Views

            builder.RegisterType<ShellView>().As<IViewFor<ShellViewModel>>().InstancePerLifetimeScope();
            //builder.RegisterType<DownloadView>().As<IViewFor<DownloadViewModel>>().InstancePerLifetimeScope();
            //builder.RegisterType<ShellView>().As<IViewFor<ShellViewModel>>().InstancePerLifetimeScope();
        }

        private IEnumerable<Assembly> assemblies;
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return this.assemblies ?? (this.assemblies = new[]
            {
                typeof(ShellViewModel).Assembly,
                typeof(ShellView).Assembly
            }
            .Distinct());
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //if (Debugger.IsAttached)
            //    return;

            //e.Handled = true;

            //Application.Current.Shutdown();
        }

        #endregion
    }
}
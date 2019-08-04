using System;
using System.Collections.Generic;

using Caliburn.Micro;

using Microsoft.EntityFrameworkCore;

using uwpUI.Core.Data;
using uwpUI.Services;
using uwpUI.ViewModels;

using Windows.ApplicationModel.Activation;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace uwpUI
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();

            // TODO WTS: Add your app in the app center and set your secret here. More at https://docs.microsoft.com/en-us/appcenter/sdk/getting-started/uwp
            //AppCenter.Start("{Your App Secret}", typeof(Analytics), typeof(Crashes));

            Initialize();
            
            if(AnalyticsInfo.VersionInfo.DeviceFamily == "Xbox")
                App.Current.FocusVisualKind = FocusVisualKind.Reveal;

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);

            using (var db = new BdoContext())
            {
                db.Database.Migrate();
            }

            RequiresPointerMode = Windows.UI.Xaml.ApplicationRequiresPointerMode.WhenRequested;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }

            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode
            (Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private WinRTContainer _container;

        protected override void Configure()
        {
            // This configures the framework to map between MainViewModel and MainPage
            // Normally it would map between MainPageViewModel and MainPage
            var config = new TypeMappingConfiguration
            {
                IncludeViewSuffixInViewModelNames = false
            };

            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container.PerRequest<ShellViewModel>();
            _container.PerRequest<MainViewModel>();
            _container.PerRequest<SecondViewModel>();
            _container.PerRequest<SettingsViewModel>();
            _container.PerRequest<DevViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(_container, typeof(ViewModels.MainViewModel), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            var shellPage = new Views.ShellPage();
            return shellPage;
        }
    }
}

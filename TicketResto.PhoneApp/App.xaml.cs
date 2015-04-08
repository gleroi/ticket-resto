using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Caliburn.Micro;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace TicketResto.PhoneApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : CaliburnApplication
    {
        public App()
        {
            InitializeComponent();
        }

		protected override object GetInstance(Type service, string key)
		{
			if (service == typeof(AppViewModel))
				return new AppViewModel(new Core.TicketsApp());
			return base.GetInstance(service, key);
		}

		protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                return;

            this.DisplayRootViewFor<AppViewModel>();
            base.OnLaunched(args);
        }
    }
}
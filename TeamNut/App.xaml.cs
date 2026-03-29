using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
<<<<<<< HEAD
=======
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TeamNut.ModelViews;
>>>>>>> main
using TeamNut.Views;
using TeamNut.Views.UserView;

namespace TeamNut
{
    public partial class App : Application
    {
        private Window? _window;
        public static UserViewModel MainViewModel { get; } = new UserViewModel();

        public App()
        {
            this.UnhandledException += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"Unhandled: {e.Exception}");
            };
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();

            _window.Content = new UserView();

            _window.Activate();
        }
    }
}
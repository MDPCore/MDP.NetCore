using Microsoft.Maui.Controls;
using System.Reflection;
using System;

namespace MDP.BlazorCore.Maui
{
    public partial class App : Application
    {
        // Constructors
        public App()
        {
            // Initialize
            this.InitializeComponent();

            // Default
            this.MainPage = new MainPage();
        }
    }
}

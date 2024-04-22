using Microsoft.Maui.Controls;
using System.Reflection;
using System;

namespace MDP.BlazorCore.Maui
{
    public partial class App : Application
    {
        // Constructors
        public App(ContentPage contentPage)
        {
            #region Contracts

            if (contentPage == null) throw new InvalidOperationException($"{nameof(contentPage)}=null");

            #endregion

            // Initialize
            this.InitializeComponent();

            // Default
            this.MainPage = contentPage;
        }
    }
}

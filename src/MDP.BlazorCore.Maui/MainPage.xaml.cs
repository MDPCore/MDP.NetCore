using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MDP.BlazorCore.Maui
{
    public partial class MainPage : ContentPage
    {
        // Constructors
        public MainPage(List<RootComponent> rootComponentList)
        {
            #region Contracts

            if (rootComponentList == null) throw new InvalidOperationException($"{nameof(rootComponentList)}=null");
           
            #endregion

            // Initialize
            this.InitializeComponent();

            // RootComponents
            foreach(var rootComponent in rootComponentList)
            {
                // Add
                blazorWebView.RootComponents.Add(rootComponent);
            }
        }
    }
}

using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;

namespace MDP.Firebase.Hosting
{
    public class FirebaseContextModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // EntryDirectory
            var entryDirectory = AppContext.BaseDirectory;
            if (Directory.Exists(entryDirectory) == false) throw new InvalidOperationException($"{nameof(entryDirectory)}=null");

            // FirebaseAdmin
            container.Register<FirebaseApp>(componentContext =>
            {
                // ConfigPath
                var configPath = Path.Combine(entryDirectory, @"MDP.Firebase.Admin.json");
                if (string.IsNullOrEmpty(configPath) == true) throw new InvalidOperationException($"{nameof(configPath)}=null");
                if (File.Exists(configPath) == false) throw new InvalidOperationException($"{configPath} not found");

                // Create
                return FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(configPath)
                });

            }).SingleInstance();

            // FirebaseMessaging
            container.Register<FirebaseMessaging>(componentContext =>
            {
                // Require
                componentContext.Resolve<FirebaseApp>();

                // Default
                return FirebaseMessaging.DefaultInstance;

            }).SingleInstance();
        }
    }
}

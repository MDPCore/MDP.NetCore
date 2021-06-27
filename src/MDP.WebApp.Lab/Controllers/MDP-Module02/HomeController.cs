using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDP.Module01;
using MDP.AspNetCore;
using MDP.Data.SqlClient;
using System.Data;

namespace MDP.WebApp.Lab.Temp
{
    [Module("MDP-Module02")]
    public class HomeController : Controller
    {
        // Fields
        private SettingContext _settingContext = null;

        private SqlClientFactory _sqlClientFactory = null;


        // Constructors
        public HomeController(SettingContext settingContext, SqlClientFactory sqlClientFactory)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));
            if (sqlClientFactory == null) throw new ArgumentException(nameof(sqlClientFactory));

            #endregion

            // Default
            _settingContext = settingContext;
            _sqlClientFactory = sqlClientFactory;

            // Test
            var beginTime = DateTime.Now;
            using (var command = _sqlClientFactory.CreateCommand("Database01"))
            {
                // CommandParameters
                command.AddParameter("@beginTime", beginTime, SqlDbType.DateTime);

                // CommandText
                command.CommandText =
                @"
                    SELECT *
                      FROM [Temp]
                     WHERE @beginTime<=beginTime
                ";

                // Execute
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                    }
                }
            }
        }


        // Methods
        public ActionResult<string> Index()
        {
            // Message
            var message = _settingContext.GetValue();
            if (string.IsNullOrEmpty(message) == true) throw new InvalidOperationException($"{nameof(message)}=null");

            // ViewBag
            this.ViewBag.Message = message;

            // Return
            return View();
        }
    }
}

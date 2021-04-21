using System;
using System.Collections.Generic;
using System.Configuration;
using Lab1CSharp.Domain;
using Lab1CSharp.srv;
using Mono.Data.Sqlite;
using System.IO;
using Lab1CSharp.Repo;
using Gtk;
using Lab1CSharp.GUI;
using Lab1CSharp.Repo.Interfaces;


namespace Lab1CSharp
{

    static class Program
    {
        private static String GetConnectionStringByName(String name)
        {
            String rv = null;
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            try
            {
                configMap.ExeConfigFilename =
                    Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "/bd.config";
                // configMap.ExeConfigFilename = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/bd.config";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var set = ConfigurationManager.OpenMappedExeConfiguration(configMap,
                ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings[name];
            if (set != null)
            {
                rv = set.ConnectionString;
            }

            return rv;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            String connectionString = GetConnectionStringByName("SQLite");
            Transaction.IdListInit();

            // IArtistRepo artistRepo = new ArtistDbRepo(connectionString);
            // IOfficeRepo officeRepo = new OfficeDbRepo(connectionString);
            // IEmployeeRepo employeeRepo = new EmployeesDbRepo(connectionString);
            // IShowRepo showRepo = new ShowDbRepo(connectionString);
            // ITransactionRepo transactionRepo = new TransDbRepo(connectionString);
            
            var srv = new Service(connectionString);

            Application.Init();
            var app = new Application("org.GtkApplication.GtkApplication", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            
            // var win = new MainWindow();
            var win = new Login(srv);
            // var win = new Client();
            
            app.AddWindow(win);
            win.App = app;
            win.ShowAll();
            Application.Run();
        }

    }
}
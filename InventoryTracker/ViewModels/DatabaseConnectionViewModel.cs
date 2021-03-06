﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InventoryTracker.DataContext;
using InventoryTracker.Helpers;
using InventoryTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.ViewModels
{
    public class DatabaseConnectionViewModel : ViewModelBase, IDialogValueProvider
    {
        #region Static and constant members

        private const string ConfigFileName = "connection-info.json";

        #endregion

        #region Private fields

        private bool _createDatabaseIfNotExists = true;

        private bool _enabled = true;

        private bool _saveConnectionInfo = true;

        private bool _showAdvancedOptions;

        private SqlServerConnectionInfoModel _sqlServerConnectionInfo;

        #endregion

        #region Readonly properties

        public Dictionary<SqlServerAuthenticationType, string> SqlServerAuthenticationTypes => new Dictionary<SqlServerAuthenticationType, string>
        {
            [SqlServerAuthenticationType.SqlServerAuthentication] = "SQL Server Authentication",
            [SqlServerAuthenticationType.WindowsAuthentication] = "Windows Authentication"
        };

        #endregion

        #region Properties

        public RelayCommand ConnectToDatabaseCommand { get; set; }

        public bool CreateDatabaseIfNotExists
        {
            get => _createDatabaseIfNotExists;
            set
            {
                _createDatabaseIfNotExists = value;
                RaisePropertyChanged();
            }
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                RaisePropertyChanged();
            }
        }

        public bool SaveConnectionInfo
        {
            get => _saveConnectionInfo;
            set
            {
                _saveConnectionInfo = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowAdvancedOptions
        {
            get => _showAdvancedOptions;
            set
            {
                _showAdvancedOptions = value;
                RaisePropertyChanged();
            }
        }

        public SqlServerConnectionInfoModel SqlServerConnectionInfo
        {
            get => _sqlServerConnectionInfo;
            set
            {
                _sqlServerConnectionInfo = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Constructors And Destructors

        public DatabaseConnectionViewModel()
        {
            if (File.Exists(ConfigFileName))
            {
                var jsonString = File.ReadAllText(ConfigFileName);
                var config = JsonSerializer.Deserialize<DatabaseConnectionSaveModel>(jsonString);
                SqlServerConnectionInfo = config.SqlServerConnectionInfo;
                SaveConnectionInfo = config.SaveConnectionInfo;
                CreateDatabaseIfNotExists = config.CreateDatabaseIfNotExists;
                ShowAdvancedOptions = config.ShowAdvancedOptions;
            }
            else
            {
                SqlServerConnectionInfo = new SqlServerConnectionInfoModel();
            }

            ConnectToDatabaseCommand = new RelayCommand(ConnectToDatabaseCommandHandler);
        }

        #endregion

        #region IDialogValueProvider implementations

        public event EventHandler CloseDialog;

        public object GetValue()
        {
            return SqlServerConnectionInfo.GetConnectionString();
        }

        #endregion

        #region Private members

        private async void ConnectToDatabaseCommandHandler()
        {
            try
            {
                Enabled = false;

                await ValidateDatabase();
                await ValidateDataContext();

                CloseDialog?.Invoke(this, null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Enabled = true;
            }
        }

        private async Task ValidateDatabase()
        {
            var connectionString = SqlServerConnectionInfo.GetConnectionString("master");
            await using var connection = new SqlConnection(connectionString);
            await using var checkDatabaseExistsCommand = new SqlCommand($"SELECT db_id('{SqlServerConnectionInfo.DatabaseName}')", connection);
            await connection.OpenAsync();
            var exists = await checkDatabaseExistsCommand.ExecuteScalarAsync() != DBNull.Value;

            if (exists == false && CreateDatabaseIfNotExists == false)
            {
                MessageBox.Show("Database does not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (exists == false && CreateDatabaseIfNotExists)
            {
                await using var createDatabaseCommand = new SqlCommand($"CREATE DATABASE {SqlServerConnectionInfo.DatabaseName}", connection);
                await createDatabaseCommand.ExecuteNonQueryAsync();
            }

            if (SaveConnectionInfo)
            {
                var saveModel = new DatabaseConnectionSaveModel
                {
                    SqlServerConnectionInfo = SqlServerConnectionInfo,
                    SaveConnectionInfo = SaveConnectionInfo,
                    CreateDatabaseIfNotExists = CreateDatabaseIfNotExists,
                    ShowAdvancedOptions = ShowAdvancedOptions
                };

                var jsonString = JsonSerializer.Serialize(saveModel);
                await File.WriteAllTextAsync(ConfigFileName, jsonString);
            }
        }

        private async Task ValidateDataContext()
        {
            var connectionString = SqlServerConnectionInfo.GetConnectionString();
            await using var dataContext = new InventoryDataContext(connectionString);
            await dataContext.Database.MigrateAsync();
        }

        #endregion
    }
}
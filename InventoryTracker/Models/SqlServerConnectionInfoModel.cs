using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace InventoryTracker.Models
{
    public enum SqlServerAuthenticationType
    {
        WindowsAuthentication = 1,
        SqlServerAuthentication = 2,
    }

    public class SqlServerConnectionInfoModel : ObservableObject
    {
        private string _serverName;

        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                RaisePropertyChanged();
            }
        }

        private SqlServerAuthenticationType _sqlServerAuthenticationType = SqlServerAuthenticationType.WindowsAuthentication;

        public SqlServerAuthenticationType SqlServerAuthenticationType
        {
            get => _sqlServerAuthenticationType;
            set
            {
                _sqlServerAuthenticationType = value;
                RaisePropertyChanged();
            }
        }

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }

        private string _databaseName;

        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                RaisePropertyChanged();
            }
        }

        private bool _encrypt;

        public bool Encrypt
        {
            get => _encrypt;
            set
            {
                _encrypt = value;
                RaisePropertyChanged();
            }
        }

        private bool _trustServerCertificate;

        public bool TrustServerCertificate
        {
            get => _trustServerCertificate;
            set
            {
                _trustServerCertificate = value;
                RaisePropertyChanged();
            }
        }

        private int _connectionTimeout = 120;

        public int ConnectionTimeout
        {
            get => _connectionTimeout;
            set
            {
                _connectionTimeout = value;
                RaisePropertyChanged();
            }
        }

        private bool _multipleActiveResultSets;

        public bool MultipleActiveResultSets
        {
            get => _multipleActiveResultSets;
            set
            {
                _multipleActiveResultSets = value;
                RaisePropertyChanged();
            }
        }

        public string GetConnectionString(string databaseName = null)
        {
            string authPart;
            switch (SqlServerAuthenticationType)
            {
                case SqlServerAuthenticationType.WindowsAuthentication:
                    authPart = $"Trusted_Connection=True;";
                    break;
                case SqlServerAuthenticationType.SqlServerAuthentication:
                    authPart = $"User Id={Username};Password={Password};";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var timeoutPart = ConnectionTimeout > 0 ? $"Connection Timeout={ConnectionTimeout};" : "";
            return $"Server={ServerName};Database={databaseName ?? DatabaseName};{authPart}MultipleActiveResultSets={MultipleActiveResultSets};{timeoutPart}Encrypt={Encrypt};TrustServerCertificate={TrustServerCertificate}";
        }
    }
}

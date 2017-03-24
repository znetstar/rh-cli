using System;
namespace BasicallyMe.RobinhoodNet
{
    public enum RobinhoodAuthRequestType
    {
        mfa,
        password,
        backupcode
    }

    public class RobinhoodAuthRequest
    {
        protected RobinhoodAuthRequestType _requestType;
        public RobinhoodAuthRequestType RequestType {
            get { return this._requestType; }
            set { this._requestType = value; }
        }

        protected string _username;
        protected string _password;
        protected string _mfa_code;
        protected string _backup_code;

        public string Username { get { return _username; } }
        public string Password { get { return _password; } }
        public string MFA_Code { get { return _mfa_code; } set { _mfa_code = value; } }
        public string Backup_Code { get { return _backup_code; } set { _backup_code = value; } }

        public RobinhoodAuthRequest (RobinhoodAuthRequestType requestType, string username, string password)
        {
            this._requestType = requestType;
            this._username = username;
            this._password = password;
        }

        public RobinhoodAuthRequest (string username, string password)
        {
            this._username = username;
            this._password = password;
            this._requestType = RobinhoodAuthRequestType.password;
        }
    }
}

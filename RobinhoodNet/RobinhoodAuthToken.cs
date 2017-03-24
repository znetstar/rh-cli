using System;
namespace BasicallyMe.RobinhoodNet
{
    public enum RobinhoodAuthTokenType
    {
        Bearer,
        Token
    }

    public class RobinhoodAuthToken
    {
        protected RobinhoodAuthTokenType _tokenType = RobinhoodAuthTokenType.Token;
        public RobinhoodAuthTokenType TokenType {
            get {
                return _tokenType;
            }
            set {
                _tokenType = value;
            }
        }

        protected string _value;
        public string Value {
            get { return _value; }
            set { _value = value; }
        }

        public RobinhoodAuthToken (string value)
        {
            _value = value;
        }

        public RobinhoodAuthToken (RobinhoodAuthTokenType tokenType, string value)
        {
            _tokenType = tokenType;
            _value = value;
        }

        [Newtonsoft.Json.JsonConstructor]
        public RobinhoodAuthToken ()
        {
        }
    }
}

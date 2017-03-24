using System;
namespace BasicallyMe.RobinhoodNet
{
    public enum RobinhoodAuthResponseType
    {
        mfa,
        token,
        none
    }

    public class RobinhoodAuthResponse
    {
        protected RobinhoodAuthResponseType _responseType;
        public RobinhoodAuthResponseType ResponseType {
            get { return this._responseType; }
        }

        protected RobinhoodAuthToken _value = null;
        public RobinhoodAuthToken Value {
            get { return this._value; }
        }

        public RobinhoodAuthResponse (RobinhoodAuthResponseType responseType, RobinhoodAuthToken value)
        {
            this._responseType = responseType;
            this._value = value;
        }

        public RobinhoodAuthResponse (RobinhoodAuthResponseType responseType)
        {
            this._responseType = responseType;
        }
    }
}

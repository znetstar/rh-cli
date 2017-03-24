using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BasicallyMe.RobinhoodNet.Raw
{
    public partial class RawRobinhoodClient
    {
        RobinhoodAuthToken _authToken;

        // oAuth constants
        static readonly string OAUTH_GRANT_TYPE = "password";
        static readonly string OAUTH_SCOPE = "internal";
        static readonly string OAUTH_CLIENT_ID = "c82SH0WZOsabOXGP2sxqcj34FxkvfnWRZBKlBjFS";


        public RobinhoodAuthToken AuthToken
        {
            get { return _authToken; }
            private set
            {
                _authToken = value;
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    _authToken.TokenType.ToString(),
                    _authToken.Value);
            }
        }

        public async Task<RobinhoodAuthResponse>
        MFAAuthenticate (RobinhoodAuthRequest authRequest)
        {
            try {
                var form = new Dictionary<String, String> () {
                    {"grant_type", OAUTH_GRANT_TYPE},
                    {"scope", OAUTH_SCOPE},
                    {"client_id", OAUTH_CLIENT_ID},
                    {"mfa_code",((authRequest.RequestType == RobinhoodAuthRequestType.mfa) ? authRequest.MFA_Code : "")},
                    {"backup_code", ((authRequest.RequestType == RobinhoodAuthRequestType.backupcode) ? authRequest.Backup_Code : "")},
                    {"username", authRequest.Username},
                    {"password", authRequest.Password}
                };

                var rawResponse = await doPost (OAUTH_TOKEN_URL, form);

                RobinhoodAuthResponse response = null;

                if (rawResponse ["access_token"] != null) {
                    var token = new RobinhoodAuthToken (RobinhoodAuthTokenType.Bearer, rawResponse ["access_token"].ToString ());
                    this.AuthToken = token;
                    response = new RobinhoodAuthResponse (RobinhoodAuthResponseType.token, token);
                } else {
                    response = new RobinhoodAuthResponse (RobinhoodAuthResponseType.none);
                }

                return response;                
            } catch {
                return new RobinhoodAuthResponse (RobinhoodAuthResponseType.none);
            }
        }

        public async Task<RobinhoodAuthResponse>
        Authenticate (RobinhoodAuthRequest authRequest)
        {
            try
            {
                var rawResponse = await doPost(LOGIN_URL, new Dictionary<string, string>
                {
                    { "username", authRequest.Username },
                    { "password", authRequest.Password }
                });

                RobinhoodAuthResponse response = null;

                if (rawResponse ["mfa_required"] != null) {
                    response = new RobinhoodAuthResponse (RobinhoodAuthResponseType.mfa);
                } else if (rawResponse ["token"] != null) {
                    var token = new RobinhoodAuthToken(rawResponse ["token"].ToString ());
                    response = new RobinhoodAuthResponse (RobinhoodAuthResponseType.token, token);
                    this.AuthToken = token;
                } else {
                    response = new RobinhoodAuthResponse (RobinhoodAuthResponseType.none);
                }

                return response;
            }
            catch
            {
                return new RobinhoodAuthResponse (RobinhoodAuthResponseType.none);
            }
        }

        public async Task<bool> Authenticate (RobinhoodAuthToken token)
        {
            this.AuthToken = token;

            // Test to see if the token is valid
            try
            {
                await doGet(API_URL);
            }
            catch
            {
                token = null;
                return false;
            }
            return true;
        }
    }
}

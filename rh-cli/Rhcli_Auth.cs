// The MIT License (MIT)
// 
// Copyright (c) 2016 Brendan Chan
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicallyMe.RobinhoodNet;
using Newtonsoft.Json;

namespace rh_cli
{
	partial class Program
	{
		protected static string symbol;

		static readonly string __tokenFile = System.IO.Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
		"RobinhoodNet",
		"token");

		static bool MFA_Auth(RobinhoodClient client, RobinhoodAuthRequest authRequest)
		{
			Console.Write("use mfa code or backup code (enter mfa or backup): ");
			var mfa_or_backup = Console.ReadLine();
			if (mfa_or_backup.Contains("mfa"))
			{
				authRequest.RequestType = RobinhoodAuthRequestType.mfa;
				Console.Write("mfa code: ");
				authRequest.MFA_Code = Console.ReadLine();
			}
			else if (mfa_or_backup.Contains("backup"))
			{
				authRequest.RequestType = RobinhoodAuthRequestType.backupcode;
				Console.Write("backup code: ");
				authRequest.Backup_Code = Console.ReadLine();
			}
			else
			{
				return false;
			}

			RobinhoodAuthResponse response = client.MFAAuthenticate(authRequest);
			return (response.ResponseType == RobinhoodAuthResponseType.token);
		}


		static bool Auth(RobinhoodClient client)
		{

			if (System.IO.File.Exists(__tokenFile))
			{
				try
				{
					var token = (RobinhoodAuthToken)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(__tokenFile), typeof(RobinhoodAuthToken));
					if (!client.Authenticate(token))
					{
						if (System.IO.File.Exists(__tokenFile))
						{
							System.IO.File.Delete(__tokenFile);
						}
						return false;
					}
					return true;
				}
				catch
				{
					if (System.IO.File.Exists(__tokenFile))
					{
						System.IO.File.Delete(__tokenFile);
					}
					return false;
				}
			}
			else
			{
				
				Console.Write("username: ");
				string userName = Console.ReadLine();

				Console.Write("password: ");
				string password = Console.ReadLine();

				RobinhoodAuthRequest authRequest = new RobinhoodAuthRequest(RobinhoodAuthRequestType.password, userName, password);
				RobinhoodAuthResponse authResponse = client.Authenticate(authRequest);
				if (authResponse.ResponseType == RobinhoodAuthResponseType.none)
				{
					return false;
				}
				else if (authResponse.ResponseType == RobinhoodAuthResponseType.mfa)
				{
					if (!MFA_Auth(client, authRequest))
					{
						return false;
					}
				}

				System.IO.Directory.CreateDirectory(
					System.IO.Path.GetDirectoryName(__tokenFile));

				System.IO.File.WriteAllText(__tokenFile, JsonConvert.SerializeObject(client.AuthToken));
				return true;
			}
		}
	}
}


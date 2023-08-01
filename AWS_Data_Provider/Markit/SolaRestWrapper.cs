using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Markit
{
	public class SolaRestWrapper
	{

		public static async Task<string> ProcessResponse(HttpResponseMessage Response) 
		{
			return (await Response.Content.ReadAsStringAsync());
		}

		public async Task<string> GetDividendes()
		{
			//https://sola-api.deltaonesolutions.com/api/dividend/Markit%20Dividends?StartDate=
			string Url = "https://api.ebs.ihsmarkit.com/api/dividend/Markit%20Dividends?StartDate=" + DateTime.Today.ToString("yyyy-MM-dd");

			var Handler = new HttpClientHandler();
			Handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
			Handler.Credentials = new NetworkCredential("user_name", "password");
			HttpClient Client = new HttpClient(Handler);
			Client.Timeout = TimeSpan.FromMinutes(30);

			Client.BaseAddress = new Uri(Url);
			try
			{
				HttpResponseMessage Response = Client.GetAsync(Client.BaseAddress).Result;
				Response.EnsureSuccessStatusCode();
				if (Response.IsSuccessStatusCode)
				{
					return await ProcessResponse(await Client.GetAsync(Client.BaseAddress));
				}
				else
				{
					return Response.ReasonPhrase;
				}
			}
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				return "";
			}
			
		}


		public async Task<string> GetAllAvailableCompositon()
		{
			// "https://sola-api.deltaonesolutions.com/api/basket/subscription"
			string Url = "https://api.ebs.ihsmarkit.com/api/basket/subscription";

			var Handler = new HttpClientHandler();
			Handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
			Handler.Credentials = new NetworkCredential("user_name", "password");
			HttpClient Client = new HttpClient(Handler);

			Client.BaseAddress = new Uri(Url);

			HttpResponseMessage Response = Client.GetAsync(Client.BaseAddress).Result;
			Response.EnsureSuccessStatusCode();
			if (Response.IsSuccessStatusCode)
			{
				return await ProcessResponse(await Client.GetAsync(Client.BaseAddress));
			}
			else 
			{
				return Response.ReasonPhrase;
			}
		}

		public async Task<string> GetCompositon(string SecId)
		{
			//https://sola-api.deltaonesolutions.com/api/index/
			string Url = "https://api.ebs.ihsmarkit.com/api/index/" + SecId + "?asAt=" + DateTime.Today.ToString("yyyy-MM-dd") + "&isOpen=1";

			var Handler = new HttpClientHandler();
			Handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
			Handler.Credentials = new NetworkCredential("user_name", "password");
			HttpClient Client = new HttpClient(Handler);

			Client.BaseAddress = new Uri(Url);

			HttpResponseMessage Response = Client.GetAsync(Client.BaseAddress).Result;
			Response.EnsureSuccessStatusCode();
			if (Response.IsSuccessStatusCode)
			{
				return await ProcessResponse(await Client.GetAsync(Client.BaseAddress));
			}
			else
			{
				return Response.ReasonPhrase;
			}
		}

		public async Task<string> GetCompositon(string SecId, DateTime CompositionDate)
		{
			//https://sola-api.deltaonesolutions.com/api/index/
			string Url = "https://api.ebs.ihsmarkit.com/api/index/" + SecId + "?asAt=" + CompositionDate.ToString("yyyy-MM-dd") + "&isOpen=1";

			var Handler = new HttpClientHandler();
			Handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
			Handler.Credentials = new NetworkCredential("user_name", "password");
			HttpClient Client = new HttpClient(Handler);

			Client.BaseAddress = new Uri(Url);

			HttpResponseMessage Response = Client.GetAsync(Client.BaseAddress).Result;
			Response.EnsureSuccessStatusCode();
			if (Response.IsSuccessStatusCode)
			{
				return await ProcessResponse(await Client.GetAsync(Client.BaseAddress));
			}
			else
			{
				return Response.ReasonPhrase;
			}
		}

	}
}

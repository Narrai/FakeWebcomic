using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FakeWebcomic.Client.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace FakeWebcomic.Client.Controllers
{
	[Route("[controller]")]
	public class ArchiveController : Controller
	{
        private HttpClientHandler _clientHandler;

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			_clientHandler = new HttpClientHandler();
			_clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

			using (var _http = new HttpClient(_clientHandler))
			{
				_http.BaseAddress = new System.Uri("https://localhost:5001/api/");

				var result = await _http.GetAsync("user");
				if (result.IsSuccessStatusCode)
				{
					var content = JsonConvert.DeserializeObject<IEnumerable<ArchiveViewModel>>(await result.Content.ReadAsStringAsync());
					return View("index", content);
				}
				else
				{
					return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
				}
			}
		}
	}
}

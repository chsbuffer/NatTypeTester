using STUN.Client;
using STUN.Enums;
using STUN.StunResult;
using STUN.Utils;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NatTypeTester
{
	internal static class Program
	{
		/// <summary>
		/// stun.qq.com 3478 0.0.0.0:0
		/// </summary>
		private static async Task Main(string[] args)
		{
			var server = @"stun.syncthing.net";
			ushort port = 3478;
			IPEndPoint? local = null;
			if (args.Length > 0 && (Uri.CheckHostName(args[0]) == UriHostNameType.Dns || IPAddress.TryParse(args[0], out _)))
			{
				server = args[0];
			}
			if (args.Length > 1)
			{
				ushort.TryParse(args[1], out port);
			}
			if (args.Length > 2)
			{
				local = NetUtils.ParseEndpoint(args[2]);
			}

			using var client = new StunClient5389UDP(server, port, local);
			await client.QueryAsync();
			var res = client.Status;

			Console.WriteLine($@"Other address is {res.OtherEndPoint}");
			Console.WriteLine($@"Binding test: {res.BindingTestResult}");
			Console.WriteLine($@"Local address: {res.LocalEndPoint}");
			Console.WriteLine($@"Mapped address: {res.PublicEndPoint}");
			Console.WriteLine($@"Nat mapping behavior: {res.MappingBehavior}");
			Console.WriteLine($@"Nat filtering behavior: {res.FilteringBehavior}");
			Console.WriteLine($@"result: {GetResult(res)}");
		}

		private static string GetResult(StunResult5389 res)
		{
			var result = "error";
			switch (res.FilteringBehavior)
			{
				case FilteringBehavior.Unknown:
					result = FilteringBehavior.Unknown.ToString();
					break;
				case FilteringBehavior.UnsupportedServer:
					result = FilteringBehavior.UnsupportedServer.ToString();
					break;
				case FilteringBehavior.Fail:
					result = FilteringBehavior.Fail.ToString();
					break;
				case FilteringBehavior.EndpointIndependent:
					switch (res.MappingBehavior)
					{
						case MappingBehavior.EndpointIndependent:
							result = "1";
							break;
						case MappingBehavior.AddressDependent:
						case MappingBehavior.AddressAndPortDependent:
							result = "2";
							break;
						default:
							break;
					}
					break;
				case FilteringBehavior.AddressDependent:
					result = "3";
					break;
				case FilteringBehavior.AddressAndPortDependent:
					result = "4";
					break;
				default:
					break;
			}

			return result;
		}
	}
}

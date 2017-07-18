﻿using System;
using System.Diagnostics;
using System.Linq;
using Nest;
using NuSearch.Domain.Model;

namespace NuSearch.Domain
{
	public static class NuSearchConfiguration
	{
		private static readonly ConnectionSettings _connectionSettings;

		public static string LiveIndexAlias => "nusearch";

		public static string OldIndexAlias => "nusearch-old";

		public static Uri CreateUri(int port)
		{
			var host = Process.GetProcessesByName("fiddler").Any() 
				? "ipv4.fiddler"
				: "localhost";

			return new Uri($"http://{host}:{port}");
		}

		static NuSearchConfiguration()
		{
			_connectionSettings = new ConnectionSettings(CreateUri(9200))
				.DefaultIndex("nusearch")
				.InferMappingFor<Package>(i => i
					.TypeName("package")
					.IndexName("nusearch")
				)
				.InferMappingFor<FeedPackage>(i => i
					.TypeName("package")
					.IndexName("nusearch")
				);
		}

		public static ElasticClient GetClient() => new ElasticClient(_connectionSettings);

		public static string CreateIndexName() => $"{LiveIndexAlias}-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss}";

		public static string PackagePath => @"C:\nuget-data";
	}
}

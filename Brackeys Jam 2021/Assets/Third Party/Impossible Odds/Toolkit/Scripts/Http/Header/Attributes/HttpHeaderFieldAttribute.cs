﻿namespace ImpossibleOdds.Http
{
	using System;
	using ImpossibleOdds.Serialization;

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class HttpHeaderFieldAttribute : Attribute, ILookupParameter<string>
	{
		private string key = null;

		object ILookupParameter.Key
		{
			get { return Key; }
		}

		public string Key
		{
			get { return key; }
			set { key = value; }
		}

		public HttpHeaderFieldAttribute()
		{ }

		public HttpHeaderFieldAttribute(string key)
		{
			this.key = key;
		}
	}
}

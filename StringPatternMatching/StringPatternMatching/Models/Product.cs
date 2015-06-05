﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StringPatternMatching {
	public class Product {

		public string product_name { get; set; }
		public string manufacturer { get; set; }
		public string model { get; set; }
		public string family { get; set; }
		public DateTime _announceddate { get; private set; }

		public string announceddate {
			get { return _announceddate.ToString(); }
			set { _announceddate = DateTime.Parse(value); }
		}

		public ConcurrentBag<long> MatchedListings { get; set; }
	}
}

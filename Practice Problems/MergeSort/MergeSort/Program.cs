using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort {

    class MergeFile : IEnumerator<string>
    {
		private readonly StreamReader _reader;

		public MergeFile(string file) {
			_reader = File.OpenText(file);
			Current = _reader.ReadLine();
		}

		public string Current { get; set; }

		public void Dispose() {
			_reader.Close();
		}

		public bool MoveNext() {
			Current = _reader.ReadLine();
			return Current != null;
		}

		public void Reset() {
			throw new NotImplementedException();
		}

		object IEnumerator.Current {
			get { return Current; }
		}
	}

	class Program {
		static void Main(string[] args) {
			// Get the file names and instantiate our helper class
			List<StreamReader> files = 
				Directory.GetFiles(@"FilesToMerge/", "*.txt")
				.Select(file => new MergeFile(file))
				.Cast<StreamReader>().ToList();

			IEnumerator<string> next = null;
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"WriteLines.txt")) {
				while (true) {
					bool done = true;
					// loop over the helpers
					foreach (var mergeFile in files) {
						done = false;
						if (next == null || string.Compare(mergeFile.ReadLine, next.Current) < 1) {
							next = mergeFile;
						}
					}
					if (done) break;
					file.WriteLine(next.Current);
					if (!next.MoveNext()) {
						// file is exhausted, dispose and remove from list
						next.Dispose();
						files.Remove(next);
						next = null;
					}
				}
			}
		}
	}
}

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeGen
{
	public class Parser
	{
		public Parser ()
		{

		}

		public void parseSettings (string path)
		{
			bool freerdpSettingsClone = true;
			FileInfo fi = new FileInfo (path);
			StreamReader reader = fi.OpenText ();
			string pattern = @"^\tALIGN64 (\S+) (\w+); /\* (\d+) \*/.*$";
			Regex regex = new Regex (pattern);
			Dictionary<string, int> boolDict = new Dictionary<string, int> ();
			Dictionary<string, int> uint32Dict = new Dictionary<string, int> ();
			Dictionary<string, int> stringDict = new Dictionary<string, int> ();

			string s = "";
			
			while ((s = reader.ReadLine()) != null) {
				Match m = regex.Match (s);

				if (m.Success) {
					string type = m.Groups [1].ToString ();
					string name = m.Groups [2].ToString ();
					string number = m.Groups [3].ToString ();

					//Console.WriteLine ("# {0} / {1} / {2}", type, name, number);

					if (type.Equals ("char*")) {
						stringDict.Add (name, Int32.Parse (number));
					} else if (type.Equals ("UINT32") || type.Equals ("DWORD")) {
						uint32Dict.Add (name, Int32.Parse (number));
					} else if (type.Equals ("BOOL")) {
						boolDict.Add (name, Int32.Parse (number));
					}
				}
			}

			if (freerdpSettingsClone)
			{
				Console.WriteLine("char* values\n");

				foreach (KeyValuePair<string, int> pair in stringDict)
				{
					Console.WriteLine("_settings->{0} = _strdup(settings->{0}); /* {1} */", pair.Key, pair.Value);
				}

				Console.WriteLine("UINT32 values\n");

				foreach (KeyValuePair<string, int> pair in uint32Dict)
				{
					Console.WriteLine("_settings->{0} = settings->{0}; /* {1} */", pair.Key, pair.Value);
				}

				Console.WriteLine("BOOL values\n");

				foreach (KeyValuePair<string, int> pair in boolDict)
				{
					Console.WriteLine("_settings->{0} = settings->{0}; /* {1} */", pair.Key, pair.Value);
				}
			}
		}
	}
}


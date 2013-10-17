using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeGen
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Parser parser = new Parser();

			Console.WriteLine ("Hello World!");

			parser.parseSettings(@"/home/awake/settings.txt");
		}
	}
}

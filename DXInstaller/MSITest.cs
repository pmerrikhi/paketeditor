/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 25/08/2013
 * Time: 13:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Linq;

namespace DXInstaller
{
	/// <summary>
	/// Description of MSITest.
	/// </summary>
	public class MSITest
	{
		public static void Main(String[] args)
		{
			MSIInstaller msi = new MSIInstaller(@"C:\Matt\Projects\vs.msi");
			var properties = from p in msi.Properties
				select p;
			
			using(FileStream fs = new FileStream(msi.File.Name + ".log", FileMode.Create, FileAccess.Write))
			{
				using(StreamWriter sw = new StreamWriter(fs))
				{
					foreach (var property in properties)
					{
						sw.WriteLine("{0} = {1}", property.Property, property.Value);
					}
				}
			}
		}
	}
}

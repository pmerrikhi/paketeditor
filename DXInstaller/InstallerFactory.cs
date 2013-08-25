/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/25/2013
 * Time: 14:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DXInstaller
{
	/// <summary>
	/// Description of InstallerFactory.
	/// </summary>
	public class InstallerFactory
	{
		public static IInstaller GetInstaller(string file)
		{
			IInstaller r = null;
			if(file.EndsWith("msi"))
			{
				r = new MSIInstaller(file);
			}
			return r;
		}
	}
}

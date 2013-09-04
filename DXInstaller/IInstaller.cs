/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 25/08/2013
 * Time: 12:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;

namespace DXInstaller
{
	public interface IInstaller
	{
		FileInfo File { get; }
		bool IsInstaller { get; }
		string Subject { get; }
		string Comments { get; }
		string Creator { get; }
		string Company { get; }
		string Version { get; }
		string Guid { get; }
		DateTime Created { get; }
	}
}

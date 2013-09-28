/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 28/09/2013
 * Time: 16:05
 * 
 */
using System;
using MVVm.Core;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	///  InstallBasViewModel: a ViewModel for the View InstallBasTextEditor
	/// a ViewModel should not contain (should avoid) references to GUI libraries, even though the use of ICommand does this
	/// </summary>
	public class InstallBasViewModel:MediatorEnabledViewModel<string>
	{
		
		public InstallBasViewModel()
		{
		}
	}
}

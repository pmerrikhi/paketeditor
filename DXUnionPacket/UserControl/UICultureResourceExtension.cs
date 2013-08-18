/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/18/2013
 * Time: 13:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;
using System.Windows.Data;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// An xaml extension for displaying UICulture language resources
	/// NB  for now it is still a dummy class
	/// </summary>
	public class UICultureResourceExtension:DynamicResourceExtension
		//StaticResourceExtension
	{
		public UICultureResourceExtension()
		{
			
		}
		
		public UICultureResourceExtension(object key):base(key)
		{
		}
	}
}

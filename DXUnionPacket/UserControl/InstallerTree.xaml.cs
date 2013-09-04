/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/25/2013
 * Time: 14:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for InstallerTree.xaml
	/// </summary>
	public partial class InstallerTree : System.Windows.Controls.UserControl
	{
		public ViewModel.InstallersDirectory VM
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<ViewModel.InstallersDirectory>();
				
			}
		}
		public InstallerTree()
		{
			try{
				InitializeComponent();
				this.DataContext = VM;
			}catch(Exception ex)
			{
				ex.Message.ToLower();
			}
		}
	}
}
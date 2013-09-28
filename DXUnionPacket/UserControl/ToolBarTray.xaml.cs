/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 28/09/2013
 * Time: 16:47
 * 
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
using StructureMap;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for ToolBarTray.xaml
	/// </summary>
	public partial class ToolBarTray : System.Windows.Controls.ToolBarTray
	{
		public DXUnionPacket.ViewModel.MainWindowViewModel VM
		{
			get{
				return ObjectFactory.GetInstance<DXUnionPacket.ViewModel.MainWindowViewModel>();
			}
		}
		public ToolBarTray()
		{
			InitializeComponent();
			this.DataContext = VM;
			
		}
	}
}
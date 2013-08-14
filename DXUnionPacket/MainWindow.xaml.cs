/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 10/08/2013
 * Time: 11:18
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
using DXUnionPacket.DataModel;
using ICSharpCode.AvalonEdit.Highlighting;

namespace DXUnionPacket
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			try{
				InitializeComponent();
				
				//DXUnionPacket.DataModel.Database db  = StructureMap.ObjectFactory.GetInstance<DXUnionPacket.DataModel.Database>();

			}
			catch (Exception ex)
			{
				ex.StackTrace.ToLower();
			}
		}
	}
}
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
using System.IO;
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
		 const String DOCK_SETTINGS = "dock_config.xml";
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
		
		void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Source.ToString();
		}
		
		void DockMan_Loaded(object sender, RoutedEventArgs e)
		{
//			try
//			{
//				string tempPath = System.IO.Path.GetTempPath();
//				tempPath = Path.Combine(tempPath, DOCK_SETTINGS);
//				if (System.IO.File.Exists(tempPath))
//				{
//					DockMan.DeserializationCallback += (s, ee) =>
//					{
//						string item = ee.Name;
////						DockableContent dc = getDockableContent(item);
////						ee.Content = dc;
//					};
//					DockMan.RestoreLayout(tempPath);
//				}
//			}
//			catch (Exception ex)
//			{
//				
//			}
		}
		
		void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
//			try
//			{
//				string tempPath = System.IO.Path.GetTempPath();
//				tempPath = Path.Combine(tempPath, DOCK_SETTINGS);
//				DockMan.SaveLayout(tempPath);
//			}
//			catch (Exception ex)
//			{
//				
//			}
		}
		
	}
}
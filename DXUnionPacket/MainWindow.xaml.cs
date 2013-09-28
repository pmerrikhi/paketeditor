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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using AvalonDock;
using DXUnionPacket.DataModel;
using DXUnionPacket.UserControl;
using DXUnionPacket.ViewModel;
using ICSharpCode.AvalonEdit.Highlighting;
using MVVm.Core;
using StructureMap;

namespace DXUnionPacket
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const String DOCK_SETTINGS = "dock_config.xml";
		bool _reset_views = false;
		
		public DXUnionPacket.ViewModel.MainWindowViewModel VM
		{
			get{
				return ObjectFactory.GetInstance<DXUnionPacket.ViewModel.MainWindowViewModel>();
			}
		}
		public MainWindow()
		{
			
			try{
				InitializeComponent();
				this.DataContext = VM;
				VM.Mediator.Register(this);
				//DXUnionPacket.DataModel.Database db  = StructureMap.ObjectFactory.GetInstance<DXUnionPacket.DataModel.Database>();
			}
			catch (Exception ex)
			{
				
			}
		}
		
		
		void DockMan_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				string tempPath = System.IO.Path.GetTempPath();
				tempPath = Path.Combine(tempPath, DOCK_SETTINGS);
				if (System.IO.File.Exists(tempPath))
				{
					DockMan.DeserializationCallback += (s, ee) =>
					{
						string item = ee.Name;
//						DockableContent dc = getDockableContent(item);
//						ee.Content = dc;
					};
					DockMan.RestoreLayout(tempPath);
				}
			}
			catch (Exception ex)
			{
				ex.Message.ToString();
			}
		}
		
		void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if(_reset_views)
				{
					string tempPath = System.IO.Path.GetTempPath();
					tempPath = Path.Combine(tempPath, DOCK_SETTINGS);
					if (System.IO.File.Exists(tempPath))
					{
						System.IO.File.Delete(tempPath);
					}
				}else{
					string tempPath = System.IO.Path.GetTempPath();
					tempPath = Path.Combine(tempPath, DOCK_SETTINGS);
					DockMan.SaveLayout(tempPath);
				}
			}
			catch (Exception ex)
			{
				
			}
		}
		
		void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			_reset_views = !_reset_views;
			MenuItem m = (MenuItem) sender;
			m.IsChecked = _reset_views;
			
		}
		UICultureResourceExtension ui_strings = new UICultureResourceExtension();
		
		[MediatorMessageSink(MainWindowViewModel.TOOLBAR_ADD_BAS)]
		void AddEditor(object dummy)
		{
			DockableContent dc = new DockableContent();
			string dc_Name = "editor_" + DPeditors.Items.Count.ToString();
			DockableContent dc_prev = null;
			foreach(var item in DPeditors.Items)
			{
				dc_prev = item as DockableContent;
				if(dc_prev.Name.Equals(dc_Name))
				{
					break;
				} else {
					dc_prev = null;
				}
			}
			if(dc_prev != null )
			{
				dc_Name += "_" + DPeditors.Items.Count.ToString();
			}
			dc.Name = dc_Name;
			
			InstallBasTextEditor bas = new InstallBasTextEditor();
			
			Binding bi = new Binding("Current");
			bi.Source = ui_strings;
			dc.SetBinding(DockableContent.TitleProperty, bi);
			
			dc.Content = bas;
			DPeditors.Items.Add(dc);
		}
		
	}
}
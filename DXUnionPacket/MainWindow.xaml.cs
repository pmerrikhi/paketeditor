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
using System.ComponentModel;
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
using Microsoft.Win32;
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
				
				foreach(var hd in ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.HighlightingDefinitions)
				{
					this.VM.Mediator.NotifyColleagues(MainWindowViewModel.TOOLBAR_HIGHLIGHT_DEF, hd.Name);
				}
				
				foreach (FontFamily font in System.Windows.Media.Fonts.SystemFontFamilies)
				{
					this.VM.Mediator.NotifyColleagues(MainWindowViewModel.TOOLBAR_FONT_FAMILY, font.Source);
				}
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
		[MediatorMessageSink(MainWindowViewModel.TOOLBAR_OPEN_FILE)]
		void openFile(object dummy)
		{
			if(this.VM.ActiveEditor == null)
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
				dlg.FilterIndex = 0;
				dlg.CheckFileExists = true;
				if (dlg.ShowDialog() ?? false) {
					
					InstallBasTextEditor bas = this.AddEditor("");
					bas.VM.CurrentFileName = dlg.FileName;
					bas.textEditor.Load(bas.VM.CurrentFileName);
					bas.textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(bas.VM.CurrentFileName)).Name;
				}
			}
		}
		[MediatorMessageSink(MainWindowViewModel.TOOLBAR_ADD_BAS)]
		void AddEditorMediator(Object dummy)
		{
			if(dummy == null)
			{
				dummy = "";
			}
			this.AddEditor(dummy.ToString());
			
		}
		
		void DockMan_ActiveContentChanged(object sender, EventArgs e)
		{
			foreach(var editor in this.VM.Editors)
			{
				editor.IsActive = false;
			}
			
			DockableContent mc = DockMan.ActiveContent as DockableContent;
			if(mc != null && mc.Content != null && mc.Content is InstallBasTextEditor)
			{
				(mc.Content as InstallBasTextEditor).VM.IsActive = true;
				this.VM.OnPropertyChanged("ActiveEditor");
			} else {
				if(VM.ActiveEditor != null && VM.ActiveEditor.IsActive)
				{
					VM.ActiveEditor.IsActive = false;
					this.VM.OnPropertyChanged("ActiveEditor");
				}
			}
		}
		InstallBasTextEditor AddEditor(string InstallBasAction)
		{
			ManagedContent mc = null;
			if(DockMan.ActiveContent == null)
			{
				mc = DockMan.DockableContents.First();
			}else{
				mc = DockMan.ActiveContent;
			}
			
			DockablePane cPane = mc.ContainerPane as DockablePane;
			
			string dc_Name = "editor_" + (VM.Editors.Count + 1).ToString();
			
			DockableContent dc_prev = null;
			foreach(var item in cPane.Items)
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
				dc_Name += "_" + cPane.Items.Count.ToString();
			}
			
			
			DockableContent dc = new DockableContent();
			dc.Name = dc_Name;
			dc.Title = dc_Name;
			cPane.Items.Add(dc);
			
			
			InstallBasTextEditor bas = new InstallBasTextEditor();
			bas.VM.SyntaxHighlighting = "VBNET";
			bas.VM.Font = "Consolas";
			bas.VM.FontSize =  12;
			bas.VM.CurrentFileName = dc_Name;
			if(!String.IsNullOrEmpty(InstallBasAction))
			{
				if(InstallBasAction.Equals(MainWindowViewModel.TOOLBAR_ADD_BAS_ADD_INSTALL))
				{
					bas.VM.InstallBas();
				}else if(InstallBasAction.Equals(MainWindowViewModel.TOOLBAR_ADD_BAS_ADD_REMOVE))
				{
					bas.VM.RemoveBas();
				}
			}
			
			dc.Content = bas;
			
			dc.Closing += delegate(object sender, CancelEventArgs e)
			{
				this.VM.ActiveEditor.IsActive = false;
				this.VM.Editors.Remove(bas.VM);
			};
			dc.Activate();
			
			this.VM.Editors.Add(bas.VM);
			bas.VM.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
			{
				if(e.PropertyName.Equals("CurrentFileName"))
				{
					dc.Title = Path.GetFileName(bas.VM.CurrentFileName);
				}
			};
			return bas;
		}
		

		
	}
}
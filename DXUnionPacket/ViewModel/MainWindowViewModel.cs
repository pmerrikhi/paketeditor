﻿/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 09/28/2013
 * Time: 16:58
 * 
 */
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using MVVm.Core;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	///MainWindowViewModel.
	/// </summary>
	public class MainWindowViewModel:MediatorEnabledViewModel<string>
	{
		public const String TOOLBAR_ADD_BAS = "DXUnionPacket.AddEditor";
		public const String TOOLBAR_ADD_BAS_ADD_INSTALL = "ADD_INSTALL";
		public const String TOOLBAR_ADD_BAS_ADD_REMOVE = "ADD_REMOVE";
		
		
		public const String TOOLBAR_OPEN_FILE = "DXUnionPacket.OpenFile";
		public const String TOOLBAR_SAVE_FILE = "DXUnionPacket.SaveFile";
		public const String TOOLBAR_SAVE_AS_FILE = "DXUnionPacket.SaveAsFile";
		
		
		public const String TOOLBAR_HIGHLIGHT_DEF = "DXUnionPacket.HighlightingDefinitions";
		public const String TOOLBAR_FONT_FAMILY = "DXUnionPacket.FontFamily";
		
		
		RelayCommand _addInstallBas;
		public ICommand AddInstallBasCommand
		{
			get{
				if(_addInstallBas == null){
					_addInstallBas = new RelayCommand(	param => {
					                                  	this.Mediator.NotifyColleagues(TOOLBAR_ADD_BAS, TOOLBAR_ADD_BAS_ADD_INSTALL);
					                                  }, param => {
					                                  	return true;
					                                  });
				}
				return _addInstallBas as ICommand;
			}
		}
		RelayCommand _addRemoveBas;
		public ICommand AddRemoveBasCommand
		{
			get{
				if(_addRemoveBas == null){
					_addRemoveBas = new RelayCommand(	param => {
					                                 	this.Mediator.NotifyColleagues(TOOLBAR_ADD_BAS, TOOLBAR_ADD_BAS_ADD_REMOVE);
					                                 }, param => {
					                                 	return true;
					                                 });
				}
				return _addRemoveBas as ICommand;
			}
		}
		RelayCommand _openFile;
		public ICommand OpenFileCommand
		{
			get{
				if(_openFile == null){
					_openFile = new RelayCommand(	param => {
					                             	this.Mediator.NotifyColleagues(TOOLBAR_OPEN_FILE, "");
					                             }, param => {
					                             	return true;
					                             });
				}
				return _openFile as ICommand;
			}
		}
		RelayCommand _saveFile;
		public ICommand SaveFileCommand
		{
			get{
				if(_saveFile == null){
					_saveFile = new RelayCommand(	param => {
					                             	this.Mediator.NotifyColleagues(TOOLBAR_SAVE_FILE, "");
					                             }, param => {
					                             	return true;
					                             });
				}
				return _saveFile as ICommand;
			}
		}
		RelayCommand _saveAsFile;
		public ICommand SaveAsFileCommand
		{
			get{
				if(_saveAsFile == null){
					_saveAsFile = new RelayCommand(	param => {
					                               	this.Mediator.NotifyColleagues(TOOLBAR_SAVE_AS_FILE, "");
					                               }, param => {
					                               	return true;
					                               });
				}
				return _saveAsFile as ICommand;
			}
		}
		public MainWindowViewModel()
		{
		}
		
		private InstallBasViewModel _activeEditor;
		public InstallBasViewModel ActiveEditor
		{
			get{
				if(_activeEditor !=  null)
				{
					if(!_activeEditor.IsActive)
					{
						if(Editors.Count > 0)
						{
							foreach(InstallBasViewModel editor in this.Editors)
							{
								if(editor.IsActive)
								{
									_activeEditor = editor;
									ActiveEditorPropertiesChanged();
									break;
								}
							}
						}
					}
				} else {
					if(Editors.Count > 0)
					{
						foreach(InstallBasViewModel editor in this.Editors)
						{
							if(editor.IsActive)
							{
								_activeEditor = editor;
								ActiveEditorPropertiesChanged();
								break;
							}
						}
					}
				}
				return _activeEditor;
			}
		}
		private void ActiveEditorPropertiesChanged()
		{
			if(_activeEditor != null)
			{
				Type activeEditorType = typeof(InstallBasViewModel);
				PropertyInfo[] props = activeEditorType.GetProperties();
				foreach(var pi in props)
				{
					_activeEditor.OnPropertyChanged(pi.Name);
				}
			}
		}
		private ObservableCollection<InstallBasViewModel> _editors;
		public ObservableCollection<InstallBasViewModel> Editors
		{
			get{
				if(_editors == null)
				{
					_editors = new ObservableCollection<InstallBasViewModel>();
					this._editors.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
					{
						this.OnPropertyChanged("ActiveEditor");
					};
				}
				return _editors;
			}
		}
		private ObservableCollection<string> _fonts;
		public ObservableCollection<string> Fonts
		{
			get{
				if(_fonts == null)
				{
					_fonts = new ObservableCollection<string>();
				}
				return _fonts;
			}
		}
		private ObservableCollection<double> _fontSizes;
		public ObservableCollection<double> FontSizes
		{
			get{
				if(_fontSizes == null)
				{
					_fontSizes = new ObservableCollection<double>();
					for(int i = 7; i < 45; i++)
					{
						_fontSizes.Add(i);
					}
				}
				return _fontSizes;
			}
		}
		private ObservableCollection<string> _highlightingDefinitions;
		public ObservableCollection<string> HighlightingDefinitions
		{
			get{
				if(_highlightingDefinitions == null)
				{
					_highlightingDefinitions = new ObservableCollection<string>();
				}
				return _highlightingDefinitions;
			}
		}
		

		[MediatorMessageSink(TOOLBAR_HIGHLIGHT_DEF)]
		void AddHighlightingDefinition(string definition)
		{
			if(!String.IsNullOrWhiteSpace(definition))
			{
				this.HighlightingDefinitions.Add(definition);
			}
		}
		
		[MediatorMessageSink(TOOLBAR_FONT_FAMILY)]
		void AddFontFamily(string font)
		{
			if(!String.IsNullOrWhiteSpace(font))
			{
				this.Fonts.Add(font);
			}
		}
		
	}
}

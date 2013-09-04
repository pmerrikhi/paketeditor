﻿/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/25/2013
 * Time: 14:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DXInstaller;
using GongSolutions.Wpf.DragDrop;
using StructureMap.Construction;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	/// Description of InstallersDirectory.
	/// </summary>
	public class InstallersDirectory : MVVm.Core.MediatorEnabledViewModel<object>,  IDragSource, IDropTarget
	{
		
		DXUnionPacket.DXSettings  setts
		{
			get{
				return  StructureMap.ObjectFactory.GetInstance<DXUnionPacket.DXSettings>();
			}
		}
		public String Directory
		{
			get{
				string t =  setts[DXSettings.SETTING_MSI_ROOT_DIR];
				return t;
			}
			set{
				if(value != null )
				{
					setts.Settings.Add(DXSettings.SETTING_MSI_ROOT_DIR, value);
					this.OnPropertyChanged("Directory");
				}
			}
		}
		
		private ObservableCollection<IInstaller> _installers;
		public ObservableCollection<IInstaller> Installers
		{
			get{
				if(_installers == null)
				{
					_installers = new ObservableCollection<IInstaller>();
				}
				return _installers;
			}
		}
		FileSystemWatcher _fsw;
		FileSystemWatcher Fsw
		{
			get{
				if(_fsw == null)
				{
					_fsw = new FileSystemWatcher();
					_fsw.EnableRaisingEvents = false;
					_fsw.IncludeSubdirectories = true;
					_fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
						| NotifyFilters.FileName | NotifyFilters.DirectoryName;
					
					//Necessary for updating the UI thread from these FileSystemEvents
					SynchronizationContext context = SynchronizationContext.Current;
					_fsw.Created += delegate(object sender, FileSystemEventArgs e)
					{
						context.Post( val => {
						             	System.Threading.Thread.Sleep(100);
						             	IInstaller inst = InstallerFactory.GetInstaller(e.FullPath);
						             	if(inst != null && inst.IsInstaller)
						             	{
						             		var i = this.Installers.Where(xx => xx.IsInstaller && xx.File.Name == e.Name);
						             		if(i == null || i.Count() < 1)
						             		{
						             			this.Installers.Add(inst);
						             			this.OnPropertyChanged("Installers");
						             		}
						             	}
						             }, sender);
					};
					_fsw.Changed += delegate(object sender, FileSystemEventArgs e)
					{
						context.Post( val => {
						             	var i = this.Installers.Where(xx => xx.IsInstaller && xx.File.Name == e.Name);
						             	if(i != null && i.Count() > 0)
						             	{
						             		this.Installers.Remove(i.First());
						             		
						             		IInstaller inst = InstallerFactory.GetInstaller(e.FullPath);
						             		this.Installers.Add(inst);
						             		this.OnPropertyChanged("Installers");
						             	}
						             }, sender);
						
					};
					_fsw.Deleted += delegate(object sender, FileSystemEventArgs e)
					{
						context.Post( val => {
						             	var i = this.Installers.Where(xx => xx.IsInstaller && xx.File.Name == e.Name);
						             	if(i != null && i.Count() > 0)
						             	{
						             		this.Installers.Remove(i.First());
						             		this.OnPropertyChanged("Installers");
						             	}
						             }, sender);
					};
					_fsw.Renamed += delegate(object sender, RenamedEventArgs e)
					{
						context.Post( val => {
						             	var i = this.Installers.Where(xx => xx.IsInstaller && xx.File.Name == e.OldName);
						             	if(i != null && i.Count() > 0)
						             	{
						             		this.Installers.Remove(i.First());
						             		
						             		IInstaller inst = InstallerFactory.GetInstaller(e.FullPath);
						             		this.Installers.Add(inst);
						             		this.OnPropertyChanged("Installers");
						             	}
						             }, sender);
					};
				}
				return _fsw;
			}
		}
		public InstallersDirectory()
		{
			ProcessDirectory();
			this.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
			{
				if(e.PropertyName.Equals("Directory"))
				{
					this.Fsw.EnableRaisingEvents = false;
					this.Fsw.Path = this.Directory;
					ProcessDirectory();
					this.Fsw.EnableRaisingEvents = true;
				}
			};
		}
		
//		public InstallersDirectory(string dir):this()
//		{
//
//			this.Directory = dir;
//			ProcessDirectory();
//
//		}
		public void ProcessDirectory()
		{
			bool changed = false;
			DirectoryInfo di = new DirectoryInfo(this.Directory);
			foreach(FileInfo fi in di.GetFiles())
			{
				try{
					IInstaller inst = InstallerFactory.GetInstaller(fi.FullName);
					if(inst != null)
					{
						this.Installers.Add(inst);
						changed = true;
					}
				}catch(Exception)
				{
					
				}
			}
			foreach(DirectoryInfo di_next in di.GetDirectories())
			{
				try{
					changed = ProcessDirectory(di_next, changed);
				}catch(Exception)
				{
					
				}
			}
			if(changed)
			{
				this.OnPropertyChanged("Installers");
			}
		}
		private bool ProcessDirectory(DirectoryInfo di, bool changed)
		{
			foreach(FileInfo fi in di.GetFiles())
			{
				try{
					IInstaller inst = InstallerFactory.GetInstaller(fi.FullName);
					if(inst != null)
					{
						this.Installers.Add(inst);
						changed = true;
					}
				}catch(Exception)
				{
					
				}
			}
			foreach(DirectoryInfo di_next in di.GetDirectories())
			{
				try{
					changed = ProcessDirectory(di_next, changed);
				}catch(Exception)
				{
					
				}
			}
			return changed;
		}
		
		public void StartDrag(DragInfo dragInfo)
		{
			dragInfo.Effects = DragDropEffects.Copy | DragDropEffects.Move;
			
			dragInfo.Data = dragInfo.SourceItem;
		}
		
		public void DragOver(DropInfo dropInfo)
		{
			if(dropInfo.Data != null )
			{
				if(dropInfo.Data is IInstaller && dropInfo.TargetItem != dropInfo.Data){
					dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
					dropInfo.Effects = DragDropEffects.Copy;
				}else if(dropInfo.Data != null){
					dropInfo.Data.ToString();
				}
			}
		}
		
		public void Drop(DropInfo dropInfo)
		{
			if(dropInfo.Data != null )
			{
				if(dropInfo.Data is IInstaller && dropInfo.TargetItem != dropInfo.Data){
					
				}
			}
		}
	}
}

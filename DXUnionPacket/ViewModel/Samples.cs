/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 11/08/2013
 * Time: 14:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using GongSolutions.Wpf.DragDrop;
using MVVm.Core;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	/// Description of Samples.
	/// </summary>
	public class Samples :MediatorEnabledViewModel<object>, IDragSource, IDropTarget
	{
		public static int instance_count;
		private DataModel.Database _db
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<DataModel.Database>();
			}
		}
		
		private ObservableCollectionWithCurrent<Sample> _sampleTree;
		public ObservableCollectionWithCurrent<Sample> SampleTree
		{
			get{
				if(_sampleTree == null)
				{
					_sampleTree = new ObservableCollectionWithCurrent<Sample>(SampleList.Where(xx => String.IsNullOrWhiteSpace(xx.Parent)));
					
				}
				return _sampleTree;
			}
		}
		private readonly ObservableCollectionWithCurrent<Sample> _sampleList  = new ObservableCollectionWithCurrent<Sample>();
		public ObservableCollectionWithCurrent<Sample> SampleList
		{
			get{
				if(_sampleList.Count == 0)
				{
					IEnumerable<DataModel.Sample> tmp_samples = _db.QuerySamples("","","","");
					foreach(DataModel.Sample s in tmp_samples)
					{
						_sampleList.Add(new Sample(s));
					}

				}
				return _sampleList;
			}
		}
		
		private RelayCommand _refreshListCommand;
		public RelayCommand RefreshListCommand
		{
			get{
				if(_refreshListCommand == null)
				{
					_refreshListCommand = new RelayCommand( xx =>
					                                       {
					                                       	IEnumerable<DataModel.Sample> tmp_samples = _db.QuerySamples("","","","");
					                                       	SampleList.Clear();
					                                       	foreach(DataModel.Sample s in tmp_samples)
					                                       	{
					                                       		SampleList.Add(new Sample(s));
					                                       	}
					                                       	this.OnPropertyChanged("SampleList");
					                                       }, xx => true);
					
				}
				return _refreshListCommand;
			}
		}
		private RelayCommand _addNewSampleCommand;
		public RelayCommand AddNewSampleCommand
		{
			get{
				if(_addNewSampleCommand == null)
				{
					_addNewSampleCommand = new RelayCommand( xx =>
					                                        {
					                                        	SampleList.AddNew();
					                                        	this.OnPropertyChanged("SampleList");
					                                        }, xx => true);
					
				}
				return _addNewSampleCommand;
			}
		}
		private RelayCommand _deleteSampleCommand;
		public RelayCommand DeleteSampleCommand
		{
			get{
				if(_addNewSampleCommand == null)
				{
					_addNewSampleCommand = new RelayCommand( xx =>
					                                        {
					                                        	Sample current = SampleList.CurrentItem;
					                                        	SampleList.RemoveCurrent();
					                                        	this.OnPropertyChanged("SampleList");
					                                        	current.Delete();
					                                        }, xx => true);
					
				}
				return _addNewSampleCommand;
			}
		}
		public Samples()
		{
			instance_count++;
		}
		
		void IDragSource.StartDrag(DragInfo dragInfo)
		{
			dragInfo.Effects = DragDropEffects.Copy | DragDropEffects.Move;
			
			dragInfo.Data = dragInfo.SourceItem;
		}
		
		void IDropTarget.DragOver(DropInfo dropInfo)
		{
			if(dropInfo.Data != null )
			{
				if(dropInfo.Data is Sample && dropInfo.TargetItem != dropInfo.Data){
					dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
					dropInfo.Effects = DragDropEffects.Copy;
				}
			}
		}
		
		void IDropTarget.Drop(DropInfo dropInfo)
		{
			if(dropInfo.Data != null )
			{
				if(dropInfo.Data is Sample && dropInfo.TargetItem != dropInfo.Data){
					try
					{
						Sample sample = dropInfo.Data as Sample;
						Sample target = dropInfo.TargetItem as Sample;
						string parent_sample = sample.Parent;
						string[] parents_sample = new string[]{ "", parent_sample};
						if(parent_sample.Contains("."))
						{
							parents_sample[1] = parent_sample.Substring(parent_sample.LastIndexOf('.') + 1);
							parents_sample[0] = parent_sample.Substring(0, parent_sample.Length - parents_sample[1].Length  -1);
						}
						Sample old_parent = this.SampleList.Where( xx => xx.Name.Equals(parents_sample[1]) && xx.Parent.Equals(parents_sample[0])).First();
						
						if(!String.IsNullOrWhiteSpace(target.Parent)){
							sample.FixDescendantsParent( target.Parent + "." + target.Name);
							sample.Parent = target.Parent + "." + target.Name;
						}else{
							sample.FixDescendantsParent( target.Name);
							sample.Parent = target.Name;
						}
						old_parent.Children.Remove(sample);
						old_parent.OnPropertyChanged("Children");
						target.Children.Add(sample);
						
						target.OnPropertyChanged("Children");
						this.OnPropertyChanged("SampleTree");
					}catch(Exception ex)
					{
						ex.StackTrace.ToLower();
					}
				}
			}
		}
	}
	public static class SamplesExtensions
	{
		
		public static IEnumerable<Sample> GetChildren(this Samples samples, Sample s)
		{
			if(String.IsNullOrEmpty(s.Parent))
			{
				var t = samples.SampleList.Where(xx => xx.Parent == s.Name);
				return t;
			}else{
				var t = samples.SampleList.Where(xx => xx.Parent == s.Parent + "." + s.Name);
				return t;
			}
		}
		public static void FixDescendantsParent(this Sample sample, String parent)
		{
			foreach(Sample s in sample.Children)
			{
				s.FixDescendantsParent(parent + "." + sample.Name);
				s.Parent = parent + "." + sample.Name;
			}
		}
	}
}

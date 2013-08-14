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
using System.Windows.Input;
using MVVm.Core;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	/// Description of Samples.
	/// </summary>
	public class Samples :MediatorEnabledViewModel<object>
	{
		private DataModel.Database _db
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<DataModel.Database>();
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
	}
}

/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/10/2013
 * Time: 11:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using MVVm.Core;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	/// Description of Samples.
	/// </summary>
	public class Sample : MVVm.Core.MediatorEnabledViewModel<object>
	{
		private DataModel.Database _db
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<DataModel.Database>();
			}
		}
		private ViewModel.Samples _samples
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<ViewModel.Samples>();
			}
		}
		
		private DataModel.Sample sample;
		public Guid Id
		{
			get{
				return sample.Id;
			}
		}
		
		public String Text
		{
			get{
				return sample.Text;
			}
			set{
				if(value != null)
				{
					sample.Text = value;
					this.OnPropertyChanged("Text");
				}
			}
		}
		public String Name
		{
			get{
				return sample.Name;
			}
			set{
				if(value != null)
				{
					sample.Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}
		public String Description
		{
			get{
				return sample.Description;
			}
			set{
				if(value != null)
				{
					sample.Description = value;
					this.OnPropertyChanged("Description");
				}
			}
		}
		public String Parent
		{
			get{
				return sample.Parent;
			}
			set{
				if(value != null)
				{
					sample.Parent = value;
					this.OnPropertyChanged("Parent");
				}
			}
		}
		
		private ObservableCollectionWithCurrent<Sample> _children;
		public ObservableCollectionWithCurrent<Sample> Children
		{
			get{
				if(_children == null)
				{
					_children =  new ObservableCollectionWithCurrent<Sample>();
				}
				var c = _samples.GetChildren(this);
				_children.Clear();
				foreach(Sample s in c)
				{
					if(!_children.Contains(s))
					{
						_children.Add(s);
					}
				}
				return _children;
			}
		}
		
		private RelayCommand _deleteCommand;
		public RelayCommand DeleteCommand
		{
			get{
				if(_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand( xx => {
					                                  	this.Delete();
					                                  });
				}
				return _deleteCommand;
			}
		}
		
		private RelayCommand _insertCommand;
		public RelayCommand InsertCommand
		{
			get{
				if(_insertCommand == null)
				{
					_insertCommand = new RelayCommand( xx => {
					                                  	this.Insert();
					                                  });
				}
				return _insertCommand;
			}
		}
		
		private RelayCommand _updateCommand;
		public RelayCommand UpdateCommand
		{
			get{
				if(_updateCommand == null)
				{
					_updateCommand = new RelayCommand( xx => {
					                                  	this.Update();
					                                  });
				}
				return _updateCommand;
			}
		}
		
		public Sample(DataModel.Sample sample)
		{
			this.sample = sample;
			this.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
			{
				 this.sample.UpdateAsync(true);
				//this.sample = _db.QuerySample(this.sample.Id);
//				this._samples.SampleList.Insert(this._samples.SampleList.CurrentPosition, this);
//				if(!_samples.SampleList.Contains(this))
//				{
//					_samples.SampleList.Insert(_samples.SampleList.CurrentPosition, this);
//				//	_samples.SampleList.MoveCurrentTo(this);
//				}
			};
		}
		public Sample()  :this(new DataModel.Sample())
		{
		}
		
		public bool Delete()
		{
			return this.sample.Delete();
		}
		public bool Update(bool tryInsert = true)
		{
			return this.sample.Update(tryInsert);
		}
		public bool Insert()
		{
			return this.sample.Insert();
		}
	}
}

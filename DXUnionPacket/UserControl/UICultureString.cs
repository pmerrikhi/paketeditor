/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/18/2013
 * Time: 13:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using MVVm.Core;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Description of UICultureString.
	/// </summary>
	public class UICultureString:MVVm.Core.ViewModelBase
	{
		public string UICulture
		{
			get;
			set;
		}
		public string Value
		{
			get;
			set;
		}
		public UICultureString()
		{
		}
		public UICultureString(string val)
		{
			this.Value = val;
		}
		public override string ToString()
		{
			if(this.Value != null)
			{
				return this.Value;
			}else{
				return "Null";
			}
		}

	}
	public class UIString:List<UICultureString>, INotifyPropertyChanged
	{
		private static List<UIString> _list;
		private static List<UIString> _List
		{
			get{
				if(_list == null)
				{
					_list= new List<UIString>();
				}
				return _list;
			}
		}
//		private List<String> _cultures;
		public IEnumerable<String> Cultures
		{
			get{
				
				var r = this.Select(xx => xx.UICulture).Distinct();

				return r;
			}
		}
		private Mediator Mediator
		{
			get{
				return MVVm.Core.Mediator.Instance;
			}
		}
		public UIString()
		{
			this.Mediator.Register(this);
			_List.Add(this);
		}
		private CultureInfo _currentCulture;
		public CultureInfo CurrentCulture
		{
			get{
				if(_currentCulture == null)
				{
					_currentCulture = System.Globalization.CultureInfo.CurrentCulture;
				}
				
				return _currentCulture;
			}
			set{
				if(value != null && value is CultureInfo && value != CultureInfo.CurrentCulture)
				{
					System.Threading.Thread.CurrentThread.CurrentCulture = value;
					System.Threading.Thread.CurrentThread.CurrentUICulture = value;
					_currentCulture = value;
					this.Mediator.NotifyColleagues("DXUnionPacket.CultureChanged", System.Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag );

					this.OnPropertyChanged("Current");
					this.OnPropertyChanged("Cultures");
				}else if(value != null && value is CultureInfo)
				{
					_currentCulture = value;
					this.Mediator.NotifyColleagues("DXUnionPacket.CultureChanged", System.Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag );

					this.OnPropertyChanged("Current");
					this.OnPropertyChanged("Cultures");
				}
				
			}
		}
		private UICultureString _current;
		public UICultureString Current
		{
			get{
				UICultureString _current = null;
				if(this.Count > 0){
					_current = this[0];
				}
				if(this.Count > 1){
					foreach(UICultureString lang_string in this)
					{
						if(lang_string.UICulture.StartsWith(this.CurrentCulture.Parent.Name))
						{
							_current = lang_string;
							if(this.CurrentCulture.Name.Equals(lang_string.UICulture))
							{
								_current = lang_string;
								break;
							}
						}
					}
				}
				if(!_current.UICulture.Equals(this.CurrentCulture.Name))
				{
					CultureInfo ci = CultureInfo.CreateSpecificCulture(_current.UICulture);
					this.CurrentCulture = ci;
				}
				
				return _current;
			}
			set{
				if(value != null && value is UICultureString && ( _current == null || value.UICulture != _current.UICulture))
				{
					if(!value.UICulture.Equals(this.CurrentCulture.Name))
					{
						this._current = value;
						CultureInfo ci = CultureInfo.CreateSpecificCulture(value.UICulture);
						this.CurrentCulture = ci;
						UpdateUIStrings();
					}
				}
			}
		}

		
		public override String ToString()
		{
			string r = "";
			if(this.Count > 0){
				r = this[0].Value;
			}
			if(this.Count > 1){
				r = this.Current.Value;
			}
			return r;
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
				propertyChanged(this, e);
			}
		}
		private void UpdateUIStrings()
		{
			foreach(UIString s in _List)
			{
				if(s.CurrentCulture != this.CurrentCulture)
				{
					s.CurrentCulture = this.CurrentCulture;
				}
			}
		}
	}
}

/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 28/09/2013
 * Time: 16:05
 * 
 */
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;

using MVVm.Core;

namespace DXUnionPacket.ViewModel
{
	/// <summary>
	///  InstallBasViewModel: a ViewModel for the View InstallBasTextEditor
	/// a ViewModel should not contain (should avoid) references to GUI libraries, even though the use of ICommand does this
	/// </summary>
	public class InstallBasViewModel:MediatorEnabledViewModel<string>
	{
		bool _isFile;
		public bool IsFile
		{
			get{
				return _isFile;
			}
			set{
				if(value != _isFile)
				{
					_isFile = value;
					this.OnPropertyChanged("IsFile");
				}
			}
		}
		string _currentFileName;
		public string CurrentFileName
		{
			get{
				return _currentFileName;
			}
			set{
				if(value != null && !value.Equals(_currentFileName))
				{
					_currentFileName = value;
					this.OnPropertyChanged("CurrentFileName");
				}
			}
		}
		bool _isActive;
		public bool IsActive
		{
			get{
				return _isActive;
			}
			set{
				if(value != _isActive)
				{
					_isActive = value;
					this.OnPropertyChanged("IsActive");
				}
			}
		}
		bool _isWordWrap;
		public bool IsWordWrap
		{
			get{
				return _isWordWrap;
			}
			set{
				if(value != _isWordWrap)
				{
					_isWordWrap = value;
					this.OnPropertyChanged("IsWordWrap");
				}
			}
		}
		bool _isShowLineNumbers;
		public bool IsShowLineNumbers
		{
			get{
				return _isShowLineNumbers;
			}
			set{
				if(value != _isShowLineNumbers)
				{
					_isShowLineNumbers = value;
					this.OnPropertyChanged("IsShowLineNumbers");
				}
			}
		}
		bool _isShowEndOfLine;
		public bool IsShowEndOfLine
		{
			get{
				return _isShowEndOfLine;
			}
			set{
				if(value != _isShowEndOfLine)
				{
					_isShowEndOfLine = value;
					this.OnPropertyChanged("IsShowEndOfLine");
				}
			}
		}
		string _text;
		public string Text
		{
			get{
				return _text;
			}
			set{
				if(value != null && !value.Equals(_text))
				{
					_text = value;
					this.OnPropertyChanged("Text");
				}
			}
		}
		
		public InstallBasViewModel()
		{
		}
		
		public void InstallBas()
		{
			String res = "Template/Install.bas";
			this.Text = this.GetType().Assembly.GetStringFromResource(res);
		}
		public void RemoveBas()
		{
			String res = "Template/Remove.bas";
			this.Text = this.GetType().Assembly.GetStringFromResource(res);
		}
	}
	public static class MvvmTextEditorExtensions
	{
		public static  String GetStringFromResource(this  Assembly ass, string psResourceName)
		{
			String r = "";
			Uri uri = new Uri("pack://application:,,,/" +ass.FullName +";component/" +psResourceName, UriKind.RelativeOrAbsolute);
			StreamResourceInfo sri = Application.GetResourceStream(uri);
			using(StreamReader sr = new StreamReader(sri.Stream))
			{
				r =sr.ReadToEnd();
			}
			return r;
		}
	}
}

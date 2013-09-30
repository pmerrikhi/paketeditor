/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 28/09/2013
 * Time: 16:18
 * 
 */
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	///  MvvmTextEditor: an ModelView-ViewModel class extending ICSharpCode.AvalonEdit.TextEditor
	/// <summary>http://stackoverflow.com/questions/12344367/making-avalonedit-mvvm-compatible</summary>
	public class MvvmTextEditor:TextEditor, INotifyPropertyChanged
	{
		public static DependencyProperty ShowEndOfLineProperty =
			DependencyProperty.Register("ShowEndOfLine", typeof(bool), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.ShowEndOfLine = (bool)args.NewValue;
			                                                 })
			                           );
		public static DependencyProperty ShowLineNumbersBindableProperty =
			DependencyProperty.Register("ShowLineNumbersBindable", typeof(bool), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.ShowLineNumbersBindable = (bool)args.NewValue;
			                                                 })
			                           );
		public static DependencyProperty WordWrapBindableProperty =
			DependencyProperty.Register("WordWrapBindable", typeof(bool), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.WordWrapBindable = (bool)args.NewValue;
			                                                 })
			                           );
		public static DependencyProperty CaretOffsetProperty =
			DependencyProperty.Register("CaretOffset", typeof(int), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.CaretOffset = (int)args.NewValue;
			                                                 })
			                           );
		
		public static DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.Text = (string)args.NewValue;
			                                                 })
			                           );
		
		public static DependencyProperty SyntaxHighlightingProperty =
			DependencyProperty.Register("SyntaxHighlighting", typeof(string), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.SyntaxHighlighting = (string)args.NewValue;
			                                                 })
			                           );
		public static DependencyProperty FontFamilyProperty =
			DependencyProperty.Register("FontFamily", typeof(string), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.FontFamily = (string)args.NewValue;
			                                                 })
			                           );

		public static DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(double), typeof(MvvmTextEditor),
			                            // binding changed callback: set value of underlying property
			                            new PropertyMetadata((obj, args) =>
			                                                 {
			                                                 	MvvmTextEditor target = (MvvmTextEditor)obj;
			                                                 	target.FontSize = (double)args.NewValue;
			                                                 })
			                           );
		public new string Text
		{
			get {return base.Text; }
			set { base.Text = value; }
		}
		public new string FontFamily
		{
			get{return base.FontFamily.Source;}
			set{
				if(value != null)
				{
					foreach(var font in System.Windows.Media.Fonts.SystemFontFamilies)
					{
						if(value.ToLower().Equals(font.Source.ToLower()))
						{
							base.FontFamily = font;
							break;
						}
					}
				}
			}
		}
		public new double FontSize
		{
			get{return base.FontSize;}
			set{
				if(value != null)
				{
					base.FontSize = value;
				}
			}
		}
		public new string SyntaxHighlighting
		{
			get{return base.SyntaxHighlighting.Name;}
			set{
				if(value != null)
				{
					foreach(var hd in ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.HighlightingDefinitions)
					{
						if(value.ToLower().Equals(hd.Name.ToLower()))
						{
							base.SyntaxHighlighting = hd;
							break;
						}
					}
				}
			}
		}
		public new bool ShowEndOfLine
		{
			get{return base.Options.ShowEndOfLine;}
			set{base.Options.ShowEndOfLine = value;}
		}
		public new bool ShowLineNumbersBindable
		{
			get { return base.ShowLineNumbers; }
			set { base.ShowLineNumbers = value; }
		}
		public new bool WordWrapBindable
		{
			get {
				return base.WordWrap; }
			set { base.WordWrap = value; }
		}
		public new int CaretOffset
		{
			get { return base.CaretOffset; }
			set { base.CaretOffset = value; }
		}
		
		public MvvmTextEditor():base()
		{
			init();
		}
		public MvvmTextEditor(TextArea ta):base(ta)
		{
			init();
		}
		void init()
		{
			this.KeyDown += delegate(object sender, KeyEventArgs e)
			{
				if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemPlus)
				{
					this.FontSize += 1;
				}else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus)
				{
					this.FontSize -= 1;
				}
			};
		}
		
		public int Length { get { return base.Text.Length; } }

		protected override void OnTextChanged(EventArgs e)
		{
			RaisePropertyChanged("Length");
			base.OnTextChanged(e);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void RaisePropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
	}
}

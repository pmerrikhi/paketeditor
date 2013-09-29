/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 28/09/2013
 * Time: 16:18
 * 
 */
using System;
using System.ComponentModel;
using System.Windows;

using ICSharpCode.AvalonEdit;

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

		public new string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
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

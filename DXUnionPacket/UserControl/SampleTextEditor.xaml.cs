/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/11/2013
 * Time: 16:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using AvalonDock;
using DXUnionPacket.ViewModel;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for SampleTextEditor.xaml
	/// </summary>
	public partial class SampleTextEditor : DockableContent
	{
		
		private Samples _vm;
		public Samples VM
		{
			get{
				if(_vm ==null)
				{
					_vm = StructureMap.ObjectFactory.GetInstance<DXUnionPacket.ViewModel.Samples>();
				}
				return _vm;
			}
		}
		public SampleTextEditor()
		{
			try{
				InitializeComponent();
				VM.SampleList.DefaultView.CurrentChanged += delegate(object sender, EventArgs e)
				{
					this.DataContext = VM.SampleList.CurrentItem;
				};
				VM.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
				{
					
				};
				this.DataContext = VM.SampleList.CurrentItem;
				textEditor.Text = VM.SampleList.CurrentItem.Text;
			}catch(Exception ex)
			{
				ex.StackTrace.ToLower();
			}
		}
		
		void TextEditor_TextChanged(object sender, EventArgs e)
		{
			this.VM.SampleList.CurrentItem.Text = textEditor.Text;
		}
	}
}
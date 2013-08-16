/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 15/08/2013
 * Time: 08:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MVVm.Core;

using DXUnionPacket.ViewModel;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for SampleDescriptionControl.xaml
	/// </summary>
	public partial class SampleDescriptionControl : System.Windows.Controls.UserControl
	{
		public Samples VM
		{
			get{
				return  StructureMap.ObjectFactory.GetInstance<DXUnionPacket.ViewModel.Samples>();
				
			}
		}
		public SampleDescriptionControl()
		{
			InitializeComponent();
			VM.Mediator.Register(this);
			this.DataContext = VM.SampleList.CurrentItem;
		}
		
		[MediatorMessageSink("SampleTextEditor.CurrentItem")]
		private void SamplesCurrent(object o)
		{
			this.DataContext  = VM.SampleList.CurrentItem;
			this.textEditor.Text = VM.SampleList.CurrentItem.Description;
		}
		
	}
}
/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 14/08/2013
 * Time: 13:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
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
	/// Interaction logic for SamplesTree.xaml
	/// </summary>
	public partial class SamplesTree : System.Windows.Controls.UserControl
	{
		public ViewModel.Samples VM
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<ViewModel.Samples>();
				
			}
		}
		public SamplesTree()
		{
			try{
				InitializeComponent();
				this.DataContext = this.VM;
				samplesTreeView.SelectedItemChanged += delegate(object sender, RoutedPropertyChangedEventArgs<object> e)
				{
					VM.SampleList.CurrentItem = e.NewValue as Sample;
					VM.Mediator.NotifyColleagues("SampleTextEditor.CurrentItem", e.NewValue);
				};
			}catch(Exception ex)
			{
				ex.StackTrace.ToLower();
			}
		}
	}
}
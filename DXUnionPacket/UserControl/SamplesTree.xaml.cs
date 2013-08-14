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
	public partial class SamplesTree : DockableContent
	{
		private ViewModel.Samples _vm;
		public ViewModel.Samples VM
		{
			get{
				if(_vm == null)
				{
					_vm = StructureMap.ObjectFactory.GetInstance<ViewModel.Samples>();
				}
				return _vm;
			}
		}
		public SamplesTree()
		{
			InitializeComponent();
			this.DataContext = this.VM;
			samplesTreeView.SelectedItemChanged += delegate(object sender, RoutedPropertyChangedEventArgs<object> e) 
			{
				e.ToString();
				VM.SampleList.CurrentItem = e.NewValue as Sample;
			};
		}
	}
}
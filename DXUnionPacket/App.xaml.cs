using System;
using System.ComponentModel;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using DXUnionPacket.ViewModel;

namespace DXUnionPacket
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		
		void Application_Startup(object sender, StartupEventArgs e)
		{
			
			StructureMap.ObjectFactory.Initialize(  x  => {
			                                      	x.For<DXUnionPacket.DXSettings>().Singleton();
			                                      	x.For<DXUnionPacket.DataModel.Database>().Singleton();
			                                      	x.For<DXUnionPacket.ViewModel.Samples>().Singleton();
			                                      	x.For<DXUnionPacket.ViewModel.InstallersDirectory>().Singleton();
			                                      });
			
			MainWindow window = new MainWindow();
			window.ShowDialog();
		}
	}

}
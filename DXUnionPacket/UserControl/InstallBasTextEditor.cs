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
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using AvalonDock;
using AvalonEdit.Sample;
using DXUnionPacket.ViewModel;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using MVVm.Core;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for SampleTextEditor.xaml
	/// </summary>
	public partial class InstallBasTextEditor : DockableContent
	{
		
		public Samples VM
		{
			get{
				return  StructureMap.ObjectFactory.GetInstance<DXUnionPacket.ViewModel.Samples>();
				
			}
		}
		public InstallBasTextEditor()
		{
			try{
				InitializeComponent();
				VM.Mediator.Register(this);
				this.isAnotherItem = true;
				this.textEditor.Text = VM.SampleList.CurrentItem.Text;
				this.isAnotherItem = false;
			}catch(Exception ex)
			{
				
				this.isAnotherItem = false;
				ex.StackTrace.ToLower();
			}
		}
		
		bool isAnotherItem;
		void TextEditor_TextChanged(object sender, EventArgs e)
		{
			if(!isAnotherItem)
			{
				this.VM.SampleList.CurrentItem.Text = textEditor.Text;
			}
		}
		[MediatorMessageSink("SampleTextEditor.CurrentItem")]
		private void SamplesCurrent(object o)
		{
			this.DataContext  = VM.SampleList.CurrentItem;
			isAnotherItem = true;
			this.textEditor.Text = VM.SampleList.CurrentItem.Text;
			isAnotherItem = false;
		}
		
		string currentFileName;
		
		void openFileClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			if (dlg.ShowDialog() ?? false) {
				currentFileName = dlg.FileName;
				textEditor.Load(currentFileName);
				textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
			}
		}
		
		void saveFileClick(object sender, EventArgs e)
		{
			if (currentFileName == null) {
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.DefaultExt = ".txt";
				if (dlg.ShowDialog() ?? false) {
					currentFileName = dlg.FileName;
				} else {
					return;
				}
			}
			textEditor.Save(currentFileName);
		}
		FoldingManager foldingManager;
		AbstractFoldingStrategy foldingStrategy;
		
		void HighlightingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (textEditor.SyntaxHighlighting == null) {
				foldingStrategy = null;
			} else {
				switch (textEditor.SyntaxHighlighting.Name) {
					case "XML":
						foldingStrategy = new XmlFoldingStrategy();
						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						break;
					case "C#":
					case "C++":
					case "PHP":
					case "Java":
						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
						foldingStrategy = new BraceFoldingStrategy();
						break;
					default:
						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
						foldingStrategy = null;
						break;
				}
			}
			if (foldingStrategy != null) {
				if (foldingManager == null)
					foldingManager = FoldingManager.Install(textEditor.TextArea);
				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
			} else {
				if (foldingManager != null) {
					FoldingManager.Uninstall(foldingManager);
					foldingManager = null;
				}
			}
		}
		
		void foldingUpdateTimer_Tick(object sender, EventArgs e)
		{
			if (foldingStrategy != null) {
				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
			}
		}
		
		void TextEditor_KeyDown(object sender, KeyEventArgs e)
		{
			
			if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemPlus)
			{
				textEditor.FontSize += 1;
			}else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus)
			{
				textEditor.FontSize -= 1;
			}
		}
	}
}
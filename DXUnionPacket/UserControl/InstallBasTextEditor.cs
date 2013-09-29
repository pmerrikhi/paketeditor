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
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using AvalonEdit.Sample;
using DXInstaller;
using DXUnionPacket.ViewModel;
using GongSolutions.Wpf.DragDrop;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using MVVm.Core;
using StructureMap;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for SampleTextEditor.xaml
	/// </summary>
	public partial class InstallBasTextEditor : System.Windows.Controls.UserControl
	{
		private InstallBasViewModel _vm;
		public InstallBasViewModel VM
		{
			get{
				if(_vm == null)
				{
					_vm = ObjectFactory.GetInstance<InstallBasViewModel>();
				}
				return _vm;
			}
		}
		public InstallBasTextEditor()
		{
			try{
				InitializeComponent();
				VM.Mediator.Register(this);
				this.DataContext = VM;
//				this.textEditor.Text = Guid.NewGuid().ToString();
			}catch(Exception ex)
			{
				ex.ToString();
			}
		}
		
		[MediatorMessageSink(MainWindowViewModel.TOOLBAR_OPEN_FILE)]
		void openFile(object dummy)
		{
			if(this.VM.IsActive)
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
				dlg.FilterIndex = 0;
				dlg.CheckFileExists = true;
				if (dlg.ShowDialog() ?? false) {
					this.VM.CurrentFileName = dlg.FileName;
					this.VM.IsFile = true;
					textEditor.Load(this.VM.CurrentFileName);
					textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(this.VM.CurrentFileName));
				}
			}
		}
		
		[MediatorMessageSink(MainWindowViewModel.TOOLBAR_SAVE_FILE)]
		void saveFile(object sender)
		{
			if(this.VM.IsActive)
			{
				if (this.VM.CurrentFileName == null || !this.VM.IsFile) {
					SaveFileDialog dlg = new SaveFileDialog();
					dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
					dlg.FilterIndex = 0;
					if (dlg.ShowDialog() ?? false) {
						this.VM.CurrentFileName = dlg.FileName;
						this.VM.IsFile = true;
					} else {
						return;
					}
				}
				textEditor.Save(this.VM.CurrentFileName);
			}
		}
		
		[MediatorMessageSink(MainWindowViewModel.TOOLBAR_SAVE_AS_FILE)]
		void saveAsFile(object sender)
		{
			if(this.VM.IsActive)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
				dlg.FilterIndex = 0;
				if (dlg.ShowDialog() ?? false) {
					this.VM.CurrentFileName = dlg.FileName;
					this.VM.IsFile = true;
				} else {
					return;
				}
				textEditor.Save(this.VM.CurrentFileName);
			}
		}
		FoldingManager foldingManager;
		AbstractFoldingStrategy foldingStrategy;
		
		void HighlightingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
//			if (textEditor.SyntaxHighlighting == null) {
//				foldingStrategy = null;
//			} else {
//				switch (textEditor.SyntaxHighlighting.Name) {
//					case "XML":
//						foldingStrategy = new XmlFoldingStrategy();
//						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
//						break;
//					case "C#":
//					case "C++":
//					case "PHP":
//					case "Java":
//						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
//						foldingStrategy = new BraceFoldingStrategy();
//						break;
//					default:
//						textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
//						foldingStrategy = null;
//						break;
//				}
//			}
//			if (foldingStrategy != null) {
//				if (foldingManager == null)
//					foldingManager = FoldingManager.Install(textEditor.TextArea);
//				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
//			} else {
//				if (foldingManager != null) {
//					FoldingManager.Uninstall(foldingManager);
//					foldingManager = null;
//				}
//			}
		}
		
		void foldingUpdateTimer_Tick(object sender, EventArgs e)
		{
//			if (foldingStrategy != null) {
//				foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
//			}
		}
		
		void TextEditor_KeyDown(object sender, KeyEventArgs e)
		{
			
//			if(e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemPlus)
//			{
//				textEditor.FontSize += 1;
//			}else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus)
//			{
//				textEditor.FontSize -= 1;
//			}
		}

//		void IDropTarget.Drop(DropInfo dropInfo)
//		{
//			if(dropInfo.Data != null )
//			{
//				if(dropInfo.Data is Sample ){
//					try
//					{
//						InsertText((dropInfo.Data as Sample).Text, dropInfo.Position);
//					}
//					catch(Exception ex)
//					{
//						ex.StackTrace.ToLower();
//					}
//				}
//			}
//		}
//		void IDropTarget.DragOver(DropInfo dropInfo)
//		{
//			if(dropInfo.Data != null )
//			{
//				if(dropInfo.Data is Sample ){
//					dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
//					dropInfo.Effects = DragDropEffects.Copy;
//				} else{
//					dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
//					dropInfo.Effects = DragDropEffects.Copy;
//				}
//			}
//
//		}

		object sync = new Object();
		void TextEditor_Drop(object sender, DragEventArgs e)
		{
			Point pos = e.GetPosition(sender as IInputElement);
			System.Windows.DataObject o = e.Data as System.Windows.DataObject;
			string[] formats = o.GetFormats();
			string SAMPLE_FORMAT = "DXUnionPacket.ViewModel.Sample";
			string MSI_INSTALL_FORMAT ="DXInstaller.MSIInstaller";
			string GONG_DD_FORMAT ="GongSolutions.Wpf.DragDrop.DropInfo";
			string text = "";
			
			foreach(string format in formats)
			{
				if(format.Equals(SAMPLE_FORMAT))
				{
					if(o.GetDataPresent(SAMPLE_FORMAT)){
						lock(sync){
							Object oo =  o.GetData(SAMPLE_FORMAT) ;
							Sample s = o.GetData(SAMPLE_FORMAT, true) as Sample;
							text = s.Text;
							break;
						}
					}
				}
				if(format.Equals(MSI_INSTALL_FORMAT))
				{
					MSIInstaller i = o.GetData(MSI_INSTALL_FORMAT) as MSIInstaller;
					text = i.Guid;
					break;
				}
			}
			if(!String.IsNullOrEmpty(text))
			{
				InsertText(text, pos);
			}
		}

		private void InsertText(String text, Point posPoint)
		{
			var pos = textEditor.GetPositionFromPoint(posPoint);
			var line = pos.Value.Line;
			var column = pos.Value.Column;

			var offset = textEditor.Document.GetOffset(line, column);
			textEditor.TextArea.Caret.Offset = offset;

			textEditor.Document.Insert(textEditor.TextArea.Caret.Offset, text);
		}

		
	}
	public static class TextEditorExtensions
	{

		public static string GetWordUnderMouse(this TextDocument document, TextViewPosition position)
		{
			string wordHovered = string.Empty;

			var line = position.Line;
			var column = position.Column;

			var offset = document.GetOffset(line, column);
			if (offset >= document.TextLength)
				offset--;

			var textAtOffset = document.GetText(offset, 1);

			// Get text backward of the mouse position, until the first space
			while (!string.IsNullOrWhiteSpace(textAtOffset))
			{
				wordHovered = textAtOffset + wordHovered;

				offset--;

				if (offset < 0)
					break;

				textAtOffset = document.GetText(offset, 1);
			}

			// Get text forward the mouse position, until the first space
			offset = document.GetOffset(line, column);
			if (offset < document.TextLength - 1)
			{
				offset++;

				textAtOffset = document.GetText(offset, 1);

				while (!string.IsNullOrWhiteSpace(textAtOffset))
				{
					wordHovered = wordHovered + textAtOffset;

					offset++;

					if (offset >= document.TextLength)
						break;

					textAtOffset = document.GetText(offset, 1);
				}
			}

			return wordHovered;
		}
	}
}
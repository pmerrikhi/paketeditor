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
using AvalonEdit.Sample;
using DXUnionPacket.ViewModel;
using GongSolutions.Wpf.DragDrop;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using MVVm.Core;

namespace DXUnionPacket.UserControl
{
	/// <summary>
	/// Interaction logic for SampleTextEditor.xaml
	/// </summary>
	public partial class InstallBasTextEditor : System.Windows.Controls.UserControl, IDropTarget
	{
		
//		public Samples VM
//		{
//			get{
//				return  StructureMap.ObjectFactory.GetInstance<DXUnionPacket.ViewModel.Samples>();
//
//			}
//		}
		public InstallBasTextEditor()
		{
			try{
				InitializeComponent();
				this.DataContext = this;
//				VM.Mediator.Register(this);
//				this.isAnotherItem = true;
//				this.textEditor.Text = VM.SampleList.CurrentItem.Text;
//				this.isAnotherItem = false;
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
//				this.VM.SampleList.CurrentItem.Text = textEditor.Text;
			}
		}

		
		string currentFileName;
		
		void openFileClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
			dlg.FilterIndex = 0;
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
				dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
				dlg.FilterIndex = 0;
				if (dlg.ShowDialog() ?? false) {
					currentFileName = dlg.FileName;
				} else {
					return;
				}
			}
			textEditor.Save(currentFileName);
		}
		void saveAsFileClick(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "(*.bas)|*.bas|(*.vbs)|*.vbs|(*.txt)|*.txt|All files (*.*)|*.*";
			dlg.FilterIndex = 0;
			if (dlg.ShowDialog() ?? false) {
				currentFileName = dlg.FileName;
			} else {
				return;
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
		void installBas(object sender, EventArgs e)
		{
			
		}
		void removeBas(object sender, EventArgs e)
		{
			
		}
		void IDropTarget.Drop(DropInfo dropInfo)
		{
			if(dropInfo.Data != null )
			{
				if(dropInfo.Data is Sample ){
					try
					{
						Sample sample = dropInfo.Data as Sample;
						TextLocation loc = textEditor.TextArea.Caret.Location;
						textEditor.Text = textEditor.Text.Insert(textEditor.TextArea.Caret.Offset,sample.Text);
					}
					catch(Exception ex)
					{
						ex.StackTrace.ToLower();
					}
				}
			}
		}
		void IDropTarget.DragOver(DropInfo dropInfo)
		{
			if(dropInfo.Data != null )
			{
				if(dropInfo.Data is Sample ){
					dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
					dropInfo.Effects = DragDropEffects.Copy;
				}
			}
			
		}
		
		void TextEditor_DragOver(object sender, DragEventArgs e)
		{
		}
		
		void TextEditor_MouseHover(object sender, MouseEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed)
			{
				var pos = textEditor.GetPositionFromPoint(e.GetPosition(textEditor));
				
				var line = pos.Value.Line;
				var column = pos.Value.Column;

				var offset = textEditor.Document.GetOffset(line, column);
				textEditor.TextArea.Caret.Offset = offset;
			}
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
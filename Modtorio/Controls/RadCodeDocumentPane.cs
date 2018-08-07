using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Telerik.Windows.Controls;

namespace Modtorio.Controls
{
	public class RadCodeDocumentPane : RadPane
	{
		public FileInfo codeFile = null;

		static RadCodeDocumentPane()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RadCodeDocumentPane),
		new FrameworkPropertyMetadata(typeof(RadCodeDocumentPane)));
		}

		private TextEditor _textEditor = new TextEditor();

		public RadCodeDocumentPane()
		{
			_textEditor.Margin = new Thickness(0);
			_textEditor.VerticalAlignment = VerticalAlignment.Stretch;
			_textEditor.HorizontalAlignment = HorizontalAlignment.Stretch;
			//_textEditor.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
			//_textEditor.Foreground = Brushes.White;
			_textEditor.ShowLineNumbers = true;

			string dir = @"C:\Program Files\MyFolder\";
#if DEBUG
			dir = @"C:\Dev\";
#endif

			Stream xshd_stream = System.IO.File.OpenRead(dir + "csharp.xshd");
			XmlTextReader xshd_reader = new XmlTextReader(xshd_stream);
			_textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
			xshd_reader.Close();
			xshd_stream.Close();
			_textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
			_textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
			Content = _textEditor;

		}

		public void OpenFile(FileInfo fileInfo)
		{
			codeFile = fileInfo;
			Header = codeFile.Name;
			StreamReader sr = codeFile.OpenText();
			Editor.Text = sr.ReadToEnd();
			sr.Close();
		}

		public void Save()
		{
			if(codeFile != null && codeFile.Exists)
			{
				StreamWriter sw = new StreamWriter(codeFile.FullName);
				sw.Write(Editor.Text);
				sw.Close();
				Header = codeFile.Name;
			}
		}

		public TextEditor Editor
		{
			get
			{
				return _textEditor;
			}
		}

		~RadCodeDocumentPane()
		{
			_textEditor.TextArea.TextEntering -= textEditor_TextArea_TextEntering;
			_textEditor.TextArea.TextEntered -= textEditor_TextArea_TextEntered;
		}



		CompletionWindow completionWindow;

		void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
		{
			Header = codeFile.Name + "*";
			if (e.Text == ".")
			{
				// Open code completion after the user has pressed dot:
				completionWindow = new CompletionWindow(e.Source as TextArea);
				IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
				data.Add(new MyCompletionData("Item1"));
				data.Add(new MyCompletionData("Item2"));
				data.Add(new MyCompletionData("Item3"));
				completionWindow.Show();
				completionWindow.Closed += delegate
				{
					completionWindow = null;
				};
			}
		}

		void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
		{
			if (e.Text.Length > 0 && completionWindow != null)
			{
				if (!char.IsLetterOrDigit(e.Text[0]))
				{
					// Whenever a non-letter is typed while the completion window is open,
					// insert the currently selected element.
					completionWindow.CompletionList.RequestInsertion(e);
				}
			}
			// Do not set e.Handled=true.
			// We still want to insert the character that was typed.
		}
	}
}

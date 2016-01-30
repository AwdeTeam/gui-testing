using System;
using System.Collections.Generic;
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

namespace AlgGui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// member variables
		private bool isTyping = false;
		private bool isTypingNotAvail = false; // set to true if need to type elsewhere so doesn't auto put cursor in console when start to type

		// log levels (don't necessarily need this)
		const int DEBUG = 1;
		const int NORMAL = 0;

		public MainWindow()
		{
			InitializeComponent();
			clearConsole();
			Logger.assignWindow(this);
			log("Program initialized!");

			addRect(10, 10, 40, 40);
		}

		// ------------------------------------
		//  EVENTS
		// ------------------------------------

		// If user starts typing (and wasn't typing in some other field), put cursor in command line bar
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (isTypingNotAvail) { return; }
			if (isTyping) { return; }

			isTyping = true;
			txtConsoleCommand.Focus();
			txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; // move cursor to end of line
		}

		private void txtConsoleCommand_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) { enterConsoleCommand(); }
		}

		private void txtConsoleCommand_LostFocus(object sender, RoutedEventArgs e)
		{
			isTyping = false;
		}

		// ------------------------------------
		//  FUNCTIONS
		// ------------------------------------

		private void addRect(int x, int y, int w, int h)
		{
			Rectangle square = new Rectangle();
			square.Fill = new SolidColorBrush(Colors.Green);
			square.Height = h;
			square.Width = w;
			world.Children.Add(square);
			Canvas.SetLeft(square, x);
			Canvas.SetTop(square, y);

			log("Added rectangle at x = " + x + ", y = " + y + ", of width " + w + ", and height " + h);
		}

		private void loadData(string fileName)
		{

		}

		public void log(string message) { log(message, Colors.DarkCyan); }
		public void log(string message, Color color) 
		{
			Run r = new Run(message);
			r.Foreground = new SolidColorBrush(color);
			lblConsole.Document.Blocks.Add(new Paragraph(r));
			lblConsole.ScrollToEnd();
		}
		private void clearConsole() 
		{ 
			lblConsole.Document.Blocks.Clear();
		}

		// when user hits enter in the console bar
		private void enterConsoleCommand()
		{
			// first get command
			string command = txtConsoleCommand.Text;
			txtConsoleCommand.Text = "";

			// add to console
			log("> " + command, Colors.White);
			txtConsoleCommand.Focus(); // make sure command line retains focus

			// check for and run command
			parseCommand(command);
		}

		private void parseCommand(string command)
		{
			// log("Attempting to run '" + command + "'...", Colors.Green); // DEBUG
			List<string> words = new List<string>();

			List<string> keys = new List<string>();
			List<string> vals = new List<string>();

			// separate into keys and vals
			words = command.Split(' ').ToList();
			foreach (string word in words)
			{
				if (word[0] == '-')	{ vals.Add(word.Substring(1)); }
				else { keys.Add(word); }
			}
			try { handleCommand(keys, vals); }
			catch (Exception e) 
			{ 
				log(">>> COMMAND FAILED", Colors.Red);
				log(e.Message, Colors.Red);
			}
		}

		private void handleCommand(List<string> keys, List<string> vals)
		{
			if (keys[0] == "exit" || keys[0] == "quit") { cmd_exit(); }
			else if (keys[0] == "help") { cmd_printHelp(); }
			else if (keys[0] == "draw")
			{
				if (keys[1] == "rect" || keys[1] == "rectangle")
				{
					int x = 0;
					int y = 0;
					int w = 0;
					int h = 0;

					// one of two methods to get xywh arguments
					if (vals.Count > 1)
					{
						x = Int32.Parse(vals[0]);
						y = Int32.Parse(vals[1]);
						w = Int32.Parse(vals[2]);
						h = Int32.Parse(vals[3]);
					}
					else
					{
						List<string> args = vals[0].Split(',').ToList();
						x = Int32.Parse(args[0]);
						y = Int32.Parse(args[1]);
						w = Int32.Parse(args[2]);
						h = Int32.Parse(args[3]);	
					}

					addRect(x, y, w, h);
				}
			}
		}

		// ------------------------------------
		//  COMMAND FUNCTIONS
		// ------------------------------------

		private void cmd_exit() { Application.Current.Shutdown(); }
		private void cmd_printHelp()
		{
			log("exit | quit", Colors.Yellow);
			log("help", Colors.Yellow);
			log("draw rect[angle] -[x] -[y] -[width] -[height]\ndraw rect[angle] -[x],[y],[width],[height]", Colors.Yellow);
		}
	}
}

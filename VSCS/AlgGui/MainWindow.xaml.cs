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

// TODO: can have input file representations?

namespace AlgGui
{
	public partial class MainWindow : Window
	{
		// member variables
		private bool isTyping = false; // meant to counteract window auto focusing textbox even if already typing there
		private bool isTypingNotAvail = false; // set to true if need to type elsewhere so doesn't auto put cursor in console when start to type

		private bool isDragging = false; // updated by individual components so don't stop dragging when mouse leaves
		private Representation draggingRep = null;

		private List<string> commandHistory = new List<string>();
		private int commandIndex = 0; // keeps track of where in command history you are


		//private List<Representation> representations = new List<Representation>();
		private Dictionary<int, Representation> representations = new Dictionary<int, Representation>();

		// log channel
		const int NORMAL = 0;
		const int DEBUG = 1;
		const int VERBOSE = 2;

		public MainWindow()
		{
			InitializeComponent();
			cmd_clearConsole();
			Master.assignWindow(this);
			log("Program initialized!");

			addRect(10, 10, 40, 40);
			addRep(2, 1);
			
			this.MouseMove += world_MouseMove;
		}

		// properties
		public Canvas getMainCanvas() { return world; }
		public void setDragging(bool dragging, Representation dragRep) 
		{ 
			isDragging = dragging; draggingRep = dragRep;
			//log("dragging is " + dragging, Colors.Fuchsia); // DEBUG
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

		// NOTE: technically a PreviewKeyDown event, so that it also registers arrow keys
		private void txtConsoleCommand_KeyDown(object sender, KeyEventArgs e)
		{
			//log("Key was pressed"); // DEBUG
			if (e.Key == Key.Enter) { enterConsoleCommand(); }
			else if (e.Key == Key.Up || e.Key == Key.Down)
			{
				if (e.Key == Key.Up) { commandIndex--; }
				else { commandIndex++; }
				if (commandIndex < 0 || commandIndex >= commandHistory.Count) { txtConsoleCommand.Text = ""; }
				else { txtConsoleCommand.Text = commandHistory[commandIndex]; }
			}
			else if (e.Key == Key.Escape) { txtConsoleCommand.Text = ""; }
		}

		private void txtConsoleCommand_LostFocus(object sender, RoutedEventArgs e)
		{
			isTyping = false;
		}

		private void world_MouseMove(object sender, MouseEventArgs e)
		{
			// if mouse has left the box it's dragging, manually call its event
			if (isDragging) { draggingRep.body_MouseMove(sender, e); }
		}

		// ------------------------------------
		//  FUNCTIONS
		// ------------------------------------

		public void setCommandPrompt(string prompt) { txtConsoleCommand.Text = prompt; txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; }

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

		private void addRep(int inputs, int outputs)
		{
			Representation r = new Representation(inputs, outputs);
			representations.Add(r.getID(), r);
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
		

		// when user hits enter in the console bar
		private void enterConsoleCommand()
		{
			// first get command
			string command = txtConsoleCommand.Text;
			txtConsoleCommand.Text = "";

			// add to console
			log("> " + command, Colors.White);
			txtConsoleCommand.Focus(); // make sure command line retains focus

			// add to command history
			commandHistory.Add(command);
			commandIndex = commandHistory.Count;

			// check for and run command
			try { parseCommand(command); }
			catch (Exception e)
			{
				log(">>> COMMAND PARSING FAILED", Colors.Red);
				log(e.Message, Colors.Red);
			}
		}

		private void parseCommand(string command)
		{
			List<string> prePassWords = new List<string>(); // used for handling quoted text
			List<string> words = new List<string>();

			List<string> keys = new List<string>();
			List<string> vals = new List<string>();


			// find and replace any spaces NOT inside of quotes
			if (command.IndexOf("\"") != -1)
			{
				string before = command.Substring(0, command.IndexOf("\""));
			}

			// get all words within quotes and condense
			prePassWords = command.Split(' ').ToList();
			for (int i = 0; i < prePassWords.Count; i++)
			{
				string word = prePassWords[i];
				if (word.Contains("\""))
				{
					// check for just one word in quotes
					if (word.Substring(word.IndexOf("\"") + 1).Contains("\"")) { word = word.Replace("\"", ""); words.Add(word); continue; }

					// go through until hit another quote
					string nextWord = prePassWords[i + 1];
					do
					{
						i++;
						nextWord = prePassWords[i];
						word += " " + nextWord;
					}
					while (!nextWord.Contains("\""));

					// remove both quotes
					word = word.Replace("\"","");
				}
				words.Add(word);
				//log("adding " + word, Colors.Gray); // DEBUG
			}

			// separate into keys and vals
			foreach (string word in words)
			{
				if (word.Length == 0) { return; }
				if (word[0] == '-') { vals.Add(word.Substring(1)); }
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
			else if (keys[0] == "clear" || keys[0] == "cls") { cmd_clearConsole(); }
			else if (keys[0] == "add")
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
					else // if all in one argument
					{
						List<string> args = vals[0].Split(',').ToList();
						x = Int32.Parse(args[0]);
						y = Int32.Parse(args[1]);
						w = Int32.Parse(args[2]);
						h = Int32.Parse(args[3]);	
					}

					addRect(x, y, w, h);
				}
				else if (keys[1] == "rep" || keys[1] == "representation")
				{
					List<string> args = vals[0].Split(',').ToList();
					int ins = Int32.Parse(args[0]);
					int outs = Int32.Parse(args[1]);
					addRep(ins, outs);
				}
			}
			else if (keys[0] == "rep" || keys[0] == "representation")
			{
				if (keys[1] == "edit")
				{
					int id = Int32.Parse(vals[0]);
					string attr = vals[1];
					string val = vals[2];

					Representation r = representations[id];

					if (attr == "lbl") 
					{ 
						r.setLabelText(val);
						log("Updated representation label to '" + val + "'");
					}
					else if (attr == "color") 
					{ 
						r.getBody().Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + val));
						log("Updated representation color to #" + val);
					}
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
			log("clear | cls     // clears console", Colors.Yellow);
			log("help", Colors.Yellow);
			log("add rect[angle] -[x] -[y] -[width] -[height]\add rect[angle] -[x],[y],[width],[height]", Colors.Yellow);
			log("rep[resentation] edit -[id] -[attr] -[value]\n\tattr: color, lbl", Colors.Yellow);
		}
		private void cmd_clearConsole() { lblConsole.Document.Blocks.Clear(); }
	}
}

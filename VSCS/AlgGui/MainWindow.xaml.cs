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


// NOTE: IF AN ELEMENT HAS A TRANSPARENT FILL (hasn't been assigned) IT CAN BE CLICKED THROUGH AND WILL NOT REGISTER EVENTS

namespace AlgGui
{
	public partial class MainWindow : Window
	{
		// member variables
		private bool m_isTyping = false; // meant to counteract window auto focusing textbox even if already typing there
		private bool m_isTypingNotAvail = false; // set to true if need to type elsewhere so doesn't auto put cursor in console when start to type

		private bool m_isDragging = false; // updated by individual components so don't stop dragging when mouse leaves
		private Representation m_draggingRep = null;

		private bool m_isDraggingScreen = false; // panning entire screen

		private bool m_isDraggingConnection = false;
		private Connection m_draggingCon = null;

		private List<string> m_commandHistory = new List<string>();
		private int m_commandIndex = 0; // keeps track of where in command history you are

		// hashmap is probably unnecessary, but I actually understand how they work now, so yeah ^_^
		private Dictionary<int, Representation> m_representations = new Dictionary<int, Representation>();

		private GraphicContainer m_gc = new GraphicContainer();

		// log channel (not implemented yet)
		const int NORMAL = 0;
		const int DEBUG = 1;
		const int VERBOSE = 2;

		// construction
		public MainWindow()
		{

			InitializeComponent();
			cmd_clearConsole();
			Master.assignWindow(this);
			log("Program initialized!");
            Datatype.testingTypes();
            Datatype t0 = Datatype.getType(0);
            Datatype t1 = Datatype.getType(1);
            Datatype t2 = Datatype.aggregateTypes("Dataheavy Image", t0, t1);
            Datatype t3 = Datatype.aggregateTypes("Datasheavy Image", t0, t1);
			
			// test representations
			addRep(new Datatype[] {t0, t1}, new Datatype[] {t2});
            addRep(new Datatype[] {t0}, new Datatype[] {t0});
            addRep(new Datatype[] { t3 }, new Datatype[] { t0 });
            /*Representation r = new AlgorithmRepresentation(2, 3);
            m_representations.Add(r.getID(), r);
			parseCommand("edit rep -1 -color -ff0000");*/
			
			// canvas initially wasn't handling events properly, so adding them to window instead
			this.MouseMove += world_MouseMove;
		}

		// properties
		public Canvas getMainCanvas() { return world; }
		public GraphicContainer getGraphicContainer() { return m_gc; }
		public void setDragging(bool dragging, Representation dragRep) 
		{ 
			m_isDragging = dragging; m_draggingRep = dragRep;
			//log("dragging is " + dragging, Colors.Fuchsia); // DEBUG
		}
		public void setDraggingConnection(bool dragging, Connection con)
		{
			m_isDraggingConnection = true;
			m_draggingCon = con;
			log("connection dragging is " + dragging, Colors.Fuchsia); // DEBUG
		}
		public Connection getDraggingConnection() { return m_draggingCon; }

		// ------------------------------------
		//  EVENT HANDLERS
		// ------------------------------------

		// If user starts typing (and wasn't typing in some other field), put cursor in command line bar
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_isTypingNotAvail) { return; }
			if (m_isTyping) { return; }

			m_isTyping = true;
			txtConsoleCommand.Focus();
			txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; // move cursor to end of line
		}

		// NOTE: technically a PreviewKeyDown event, so that it also registers arrow keys
		private void txtConsoleCommand_KeyDown(object sender, KeyEventArgs e)
		{
			//log("Key was pressed"); // DEBUG
			if (e.Key == Key.Enter) { enterConsoleCommand(); }
			else if (e.Key == Key.Up || e.Key == Key.Down) // scroll command history
			{
				if (e.Key == Key.Up) { m_commandIndex--; }
				else { m_commandIndex++; }
				if (m_commandIndex < 0 || m_commandIndex >= m_commandHistory.Count) { txtConsoleCommand.Text = ""; }
				else { txtConsoleCommand.Text = m_commandHistory[m_commandIndex]; }
			}
			else if (e.Key == Key.Escape) { txtConsoleCommand.Text = ""; }
		}

		private void txtConsoleCommand_LostFocus(object sender, RoutedEventArgs e)
		{
			m_isTyping = false;
		}

		// technically window_mousemove
		private void world_MouseMove(object sender, MouseEventArgs e)
		{
			// if mouse has left the box it's dragging, manually call its event
			/*if (m_isDragging) { m_draggingRep.body_MouseMove(sender, e); }
			if (m_isDraggingScreen)
			{
				// if panning screen, move ALL things
				foreach (Representation r in m_representations.Values)
				{
					Point p = e.GetPosition(world);
					double x = p.X - r.getRelativeX();
					double y = p.Y - r.getRelativeY();
					r.move(x, y);
				}
			}
			if (m_isDraggingConnection)
			{
				Point p = e.GetPosition(world);
				int x = (int)p.X;
				int y = (int)p.Y;
				//log("mouse point: " + x + " " + y, Colors.PaleTurquoise); // DEBUG
				m_draggingCon.adjustSecondPoint(x, y);
			}*/
			m_gc.evt_MouseMove(sender, e);
		}

		// technically window instead of world as well (canvases don't handle events properly....)
		private void world_MouseDown(object sender, MouseButtonEventArgs e)
		{
			/*if (e.MiddleButton == MouseButtonState.Pressed)
			{
				m_isDraggingScreen = true;
				foreach (Representation r in m_representations.Values)
				{
					Point p = e.GetPosition(world);
					//r.setRelativeX(p.X - r.getCurrentX());
					//r.setRelativeY(p.Y - r.getCurrentY());
				}
			}*/
			m_gc.evt_MouseDown(sender, e);
		}

		private void world_MouseUp(object sender, MouseButtonEventArgs e)
		{
			/*if (m_isDraggingScreen)
			{
				m_isDraggingScreen = false;
				foreach (Representation r in m_representations.Values)
				{
					//r.setRelativeX(0);
					//r.setRelativeY(0);
				}
			}
			if (m_isDraggingConnection)
			{
				if (m_draggingCon != null && !m_draggingCon.isComplete()) { world.Children.Remove(m_draggingCon.getBody()); }
				m_draggingCon = null;
				m_isDraggingConnection = false;
			}*/
			m_gc.evt_MouseUp(sender, e);
		}


		// ------------------------------------
		//  FUNCTIONS
		// ------------------------------------

		public void setCommandPrompt(string prompt) { txtConsoleCommand.Text = prompt; txtConsoleCommand.CaretIndex = txtConsoleCommand.Text.Length; }

		// add random rectangle, cause why not.
		// NOTE: unneeded function at this point
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

		// create representation (eventually this should be based SOLELY on an imported algorithm, not created manually)
		private void addRep(int inputs, int outputs)
		{
			Representation r = new Representation(inputs, outputs);
			m_representations.Add(r.getID(), r);
		}

        private void addRep(Datatype[] inputs, Datatype[] outputs)
        {
            Representation r = new Representation(inputs, outputs);
            m_representations.Add(r.getID(), r);
        }

		private void loadData(string fileName)
		{
			// load me
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
			m_commandHistory.Add(command);
			m_commandIndex = m_commandHistory.Count;

			// check for and run command
			try { parseCommand(command); }
			catch (Exception e)
			{
				log(">>> COMMAND PARSING FAILED", Colors.Red);
				log(e.Message, Colors.Red);
			}
		}

		// take a command string and figure out what's what (USE THIS FOR HANDLING COMMAND SCRIPTS)
		// NOTE: this function is NOT setup to handle more than one set of quotes in an entire command
		// (essentially adds every space-delimited word to a prelist, then selectively adds to actual word
		//   list, combining multiple into one 'word' if quotes were detected)
		private void parseCommand(string command)
		{
			List<string> prePassWords = new List<string>(); // used for handling quoted text
			List<string> words = new List<string>();

			List<string> keys = new List<string>();
			List<string> vals = new List<string>();

			// get all words within quotes and condense
			prePassWords = command.Split(' ').ToList();
			for (int i = 0; i < prePassWords.Count; i++)
			{
				string word = prePassWords[i];
				if (word.Contains("\""))
				{
					// check for just one word in quotes
					if (word.Substring(word.IndexOf("\"") + 1).Contains("\"")) { word = word.Replace("\"", ""); words.Add(word); continue; }

					// go through and append into single 'word' until hit another quote
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

		// NOTE: rough command syntax convention: [VERB] [NOUN] [-VALSLIST]
		// NOTE: No need to make basic validation checks for if keys[1] exists etc, try catch in parseCommand will handle
		// based on parsed command, figure out what function to execute
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
			else if (keys[0] == "edit")
			{
				if (keys[1] == "rep" || keys[1] == "representation")
				{
					int id = Int32.Parse(vals[0]);
					string attr = vals[1];
					string val = vals[2];

					Representation r = m_representations[id];

					if (attr == "lbl") 
					{ 
						//r.setLabelText(val);
						log("Updated representation label to '" + val + "'");
					}
					else if (attr == "color") 
					{
                        r.m_baseColor = ((SolidColorBrush)(new BrushConverter().ConvertFrom("#" + val))).Color; 
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
			log("add rep[resentation] -[numInputs],[numOutputs]", Colors.Yellow);
			log("edit rep[resentation] -[id] -[attr] -[value]\n\tattr: color, lbl", Colors.Yellow);
		}
		private void cmd_clearConsole() { lblConsole.Document.Blocks.Clear(); }
	}
}

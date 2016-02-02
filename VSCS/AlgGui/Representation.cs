using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows;

namespace AlgGui
{
	public class Representation
	{
		private const int NODE_SIZE = 10;

		// member variables
		private int id = 0;
		private Rectangle body = new Rectangle();
		private List<Node> nodes = new List<Node>();
		//private List<Ellipse> outNodes = new List<Ellipse>();

		private Label label = new Label();
		private Label labelID = new Label();

		private bool isBodyClicked = false;
		private double bodyRelativeX = 0; // relative coordinates from body to where mouse clicked
		private double bodyRelativeY = 0;

		// construction
		public Representation(int numIn, int numOut)
		{
			Master.log("----Creating representation----");
			id = Master.getNextRepID();
			Master.log("ID: " + id, Colors.GreenYellow);
			int width = calcOptimalWidth(numIn, numOut);
			createDrawing(100, 100, width, 40, numIn, numOut);
		}

		// PROPERTIES
		public void setLabelText(string text) { label.Content = text; }
		public int getID() { return id; }
		public Rectangle getBody() { return body; }

		// FUNCTIONS


		// find least amount of space to fit all nodes
		private int calcOptimalWidth(int numIn, int numOut)
		{
			int totalXIn = numIn * NODE_SIZE;
			int totalXOut = numOut * NODE_SIZE;

			int widest = totalXIn;
			if (totalXOut > widest) { widest = totalXOut; }

			if (widest < 25) { widest = 25; }
			return widest;
		}

		private void createDrawing(int x, int y, int w, int h, int numIn, int numOut)
		{
			// create body
			body.Fill = new SolidColorBrush(Colors.RoyalBlue);
			body.Height = h;
			body.Width = w;
			body.RadiusX = 5;
			body.RadiusY = 5;

			Canvas.SetLeft(body, x);
			Canvas.SetTop(body, y);

			Master.getCanvas().Children.Add(body);
			Master.log("Representation drawing completed (" + x + "," + y + "," + w + "," + h + "," + numIn + "," + numOut + ")");

			// body event handlers
			body.MouseDown += new MouseButtonEventHandler(body_MouseDown);
			body.MouseUp += new MouseButtonEventHandler(body_MouseUp);
			body.MouseMove += new MouseEventHandler(body_MouseMove);

			// create nodes

			// find left of center point (for centering the two groups of nodes)
			int inStartPoint = (int)((body.Width - (numIn * NODE_SIZE)) / 2);
			int outStartPoint = (int)((body.Width - (numOut * NODE_SIZE)) / 2);
			
			// input nodes
			for (int i = 0; i < numIn; i++)
			{
				Node n = new Node(x, y, inStartPoint + i * NODE_SIZE, -NODE_SIZE, NODE_SIZE);
				nodes.Add(n);
			}
			// output nodes
			for (int i = 0; i < numOut; i++)
			{
				Node n = new Node(x, y, outStartPoint + i * NODE_SIZE, (int)body.Height, NODE_SIZE);
				nodes.Add(n);
			}

			// create labels
			labelID.Margin = new Thickness(0);
			labelID.Content = id;
			labelID.Foreground = new SolidColorBrush(Colors.White);
			labelID.IsHitTestVisible = false;
			Canvas.SetLeft(labelID, x);
			Canvas.SetTop(labelID, y);

			Master.getCanvas().Children.Add(labelID);

			label.Foreground = new SolidColorBrush(Colors.Black);
			label.Content = "I'm a good label!";
			label.Padding = new Thickness(0);
			label.Margin = new Thickness(0);
			label.Height = 20;

			Canvas.SetLeft(label, x + body.Width + 2);
			Canvas.SetTop(label, y + (body.Height / 2) - (label.Height / 2)); // center it!

			Master.getCanvas().Children.Add(label);
			Master.log("Representation label created");

			// label event handlers
			label.MouseDown += new MouseButtonEventHandler(label_MouseDown);
		}


		// EVENT HANDLERS

		private void label_MouseDown(object sender, MouseEventArgs e)
		{
			Master.log("Preparing to update label");
			Master.setCommandPrompt("rep edit -" + id + " -lbl -\"");
		}

		// TODO: differentiate between right click and left click (right click to change color?)
		// don't forget to add a function in Master to change the command prompt line
		private void body_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Master.log("I was clicked upon!", Colors.Salmon); // DEBUG
				isBodyClicked = true;

				// get relative coordinates
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X;
				double y = p.Y;

				bodyRelativeX = x - Canvas.GetLeft(body);
				bodyRelativeY = y - Canvas.GetTop(body);

				// tell main window that we're being dragged so it continues action even if mouse moves out of box
				Master.setDragging(true, this);
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
				Master.log("Right click", Colors.DarkSeaGreen);
				Master.log("Editing representation id " + id);
				Master.setCommandPrompt("rep edit -" + id + " -");
			}
		}
		private void body_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released)
			{
				isBodyClicked = false;
				Master.setDragging(false, null);
			}
		}
		public void body_MouseMove(object sender, MouseEventArgs e)
		{
			if (isBodyClicked) 
			{
				// move body
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X - bodyRelativeX;
				double y = p.Y - bodyRelativeY;

				Canvas.SetLeft(body, x);
				Canvas.SetTop(body, y);

				// move nodes
				foreach (Node n in nodes)
				{
					Canvas.SetLeft(n.getBody(), x + n.getOffsetX());
					Canvas.SetTop(n.getBody(), y + n.getOffsetY());
				}

				// move labels
				Canvas.SetLeft(label, x + body.Width + 2);
				Canvas.SetTop(label, y + (body.Height / 2) - (label.Height / 2));

				Canvas.SetLeft(labelID, x);
				Canvas.SetTop(labelID, y);
			}
		}
	}
}

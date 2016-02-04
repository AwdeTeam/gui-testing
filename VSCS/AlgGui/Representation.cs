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
		private int m_id = 0;
		private Rectangle m_body = new Rectangle();
		private List<Node> m_nodes = new List<Node>();
		//private List<Ellipse> outNodes = new List<Ellipse>();

		private Label m_lblName = new Label();
		private Label m_lblID = new Label();

		private bool m_isBodyClicked = false;
		private double m_bodyRelativeX = 0; // relative coordinates from body to where mouse clicked
		private double m_bodyRelativeY = 0;

		// construction
		public Representation(int numIn, int numOut)
		{
			Master.log("----Creating representation----");
			m_id = Master.getNextRepID();
			Master.log("ID: " + m_id, Colors.GreenYellow);
			int width = calcOptimalWidth(numIn, numOut);
			createDrawing(100, 100, width, 40, numIn, numOut);
		}

		// PROPERTIES
		public void setLabelText(string text) { m_lblName.Content = text; }
		public int getID() { return m_id; }
		public Rectangle getBody() { return m_body; }
		public double getCurrentX() { return Canvas.GetLeft(m_body); }
		public double getCurrentY() { return Canvas.GetTop(m_body); }
		public double getRelativeX() { return m_bodyRelativeX; } // should only be necessary for middle clicking
		public double getRelativeY() { return m_bodyRelativeY; }
		public void setRelativeX(double x) { m_bodyRelativeX = x; }
		public void setRelativeY(double y) { m_bodyRelativeY = y; }


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
			m_body.Fill = new SolidColorBrush(Colors.RoyalBlue);
			m_body.Height = h;
			m_body.Width = w;
			m_body.RadiusX = 5;
			m_body.RadiusY = 5;
			Canvas.SetZIndex(m_body, 10);

			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);

			Master.getCanvas().Children.Add(m_body);
			Master.log("Representation drawing completed (" + x + "," + y + "," + w + "," + h + "," + numIn + "," + numOut + ")");

			// body event handlers
			m_body.MouseDown += new MouseButtonEventHandler(body_MouseDown);
			m_body.MouseUp += new MouseButtonEventHandler(body_MouseUp);
			m_body.MouseMove += new MouseEventHandler(body_MouseMove);

			// create nodes

			// find left of center point (for centering the two groups of nodes)
			int inStartPoint = (int)((m_body.Width - (numIn * NODE_SIZE)) / 2);
			int outStartPoint = (int)((m_body.Width - (numOut * NODE_SIZE)) / 2);
			
			// input nodes
			for (int i = 0; i < numIn; i++)
			{
				Node n = new Node(this, x, y, inStartPoint + i * NODE_SIZE, -NODE_SIZE, NODE_SIZE, true);
				m_nodes.Add(n);
			}
			// output nodes
			for (int i = 0; i < numOut; i++)
			{
				Node n = new Node(this, x, y, outStartPoint + i * NODE_SIZE, (int)m_body.Height, NODE_SIZE, false);
				m_nodes.Add(n);
			}

			// create labels
			m_lblID.Margin = new Thickness(0);
			m_lblID.Content = m_id;
			m_lblID.Foreground = new SolidColorBrush(Colors.White);
			m_lblID.IsHitTestVisible = false;
			Canvas.SetZIndex(m_lblID, 10);
			Canvas.SetLeft(m_lblID, x);
			Canvas.SetTop(m_lblID, y);

			Master.getCanvas().Children.Add(m_lblID);

			m_lblName.Foreground = new SolidColorBrush(Colors.Black);
			m_lblName.Content = "I'm a good label!";
			m_lblName.Padding = new Thickness(0);
			m_lblName.Margin = new Thickness(0);
			m_lblName.Height = 20;

			Canvas.SetLeft(m_lblName, x + m_body.Width + 2);
			Canvas.SetTop(m_lblName, y + (m_body.Height / 2) - (m_lblName.Height / 2)); // center it!

			Master.getCanvas().Children.Add(m_lblName);
			Master.log("Representation label created");

			// label event handlers
			m_lblName.MouseDown += new MouseButtonEventHandler(label_MouseDown);
		}

		public void move(double x, double y)
		{
			// move body
			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);

			// move nodes
			foreach (Node n in m_nodes)
			{
				//Canvas.SetLeft(n.getBody(), x + n.getOffsetX());
				//Canvas.SetTop(n.getBody(), y + n.getOffsetY());
				n.move(x, y);
			}

			// move labels
			Canvas.SetLeft(m_lblName, x + m_body.Width + 2);
			Canvas.SetTop(m_lblName, y + (m_body.Height / 2) - (m_lblName.Height / 2));

			Canvas.SetLeft(m_lblID, x);
			Canvas.SetTop(m_lblID, y);
		}

		// EVENT HANDLERS

		private void label_MouseDown(object sender, MouseEventArgs e)
		{
			Master.log("Preparing to update label");
			Master.setCommandPrompt("edit rep -" + m_id + " -lbl -\"");
		}

		// TODO: differentiate between right click and left click (right click to change color?)
		// don't forget to add a function in Master to change the command prompt line
		private void body_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Master.log("Representation ID " + m_id + " was clicked on", Colors.Salmon); // DEBUG
				m_isBodyClicked = true;

				// get relative coordinates
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X;
				double y = p.Y;

				m_bodyRelativeX = x - Canvas.GetLeft(m_body);
				m_bodyRelativeY = y - Canvas.GetTop(m_body);

				// tell main window that we're being dragged so it continues action even if mouse moves out of box
				Master.setDragging(true, this);
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
				Master.log("Right click", Colors.DarkSeaGreen); // DEBUG
				Master.log("Editing representation id " + m_id);
				Master.setCommandPrompt("edit rep -" + m_id + " -");
			}
		}
		private void body_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released)
			{
				m_isBodyClicked = false;
				Master.setDragging(false, null);
			}
		}
		public void body_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_isBodyClicked) 
			{
				// move body
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X - m_bodyRelativeX;
				double y = p.Y - m_bodyRelativeY;

				this.move(x, y);
			}
		}
	}
}

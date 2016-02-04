using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace AlgGui
{
	public class Node
	{
		// TODO: add labels on hover (right click allows you to change?)

		// member variables
		private Ellipse body = new Ellipse();
		private Representation parent;

		// NOTE: offsets are from TOP LEFT CORNER OF REPRESENTATION
		private int offsetX = 0; 
		private int offsetY = 0;

		private List<Connection> connections = new List<Connection>();

		bool m_isInput = true; // false means output

		// construction
		public Node(Representation parent, int x, int y, int offX, int offY, int size, bool isInput) // PASS IN X AND Y OF REPRESENTATION
		{
			this.parent = parent;
			offsetX = offX;
			offsetY = offY;
			m_isInput = isInput;

			createDrawing(x, y, size);
		}

		// properties
		public int getOffsetX() { return offsetX; }
		public int getOffsetY() { return offsetY; }

		public Ellipse getBody() { return body; }

		public double getCurrentX() { return Canvas.GetLeft(body); }
		public double getCurrentY() { return Canvas.GetTop(body); }

		public bool isInput() { return m_isInput; }

		public void addConnection(Connection c) { connections.Add(c); }
		

		private void createDrawing(int x, int y, int size)
		{
			// create body
			//body = new Ellipse();
			body.Fill = Brushes.White;
			body.Stroke = new SolidColorBrush(Colors.Black);
			body.StrokeThickness = 2;
			body.Height = size;
			body.Width = size;

			Canvas.SetLeft(body, x + offsetX);
			Canvas.SetTop(body, y + offsetY);

			Master.getCanvas().Children.Add(body);
			Master.log("Representation node created with offsets (" + offsetX + "," + offsetY + ")");

			// create event handlers
			body.MouseDown += new MouseButtonEventHandler(body_mouseDown);
			body.MouseUp += new MouseButtonEventHandler(body_mouseUp);
		}

		public void move(double x, double y)
		{
			Canvas.SetLeft(body, x + offsetX);
			Canvas.SetTop(body, y + offsetY);

			// update connections
			foreach (Connection c in connections)
			{
				c.adjustRelatedPoint(this);
			}
		}

		// EVENT HANDLERS

		private void body_mouseDown(object sender, MouseEventArgs e)
		{
			Master.log("Node has been clicked!", Colors.Tomato); // DEBUG
		
			Connection n = new Connection(this);
		}
		private void body_mouseUp(object sender, MouseEventArgs e)
		{
			Master.log("Registered up");
			if (Master.getDraggingConnection() != null)
			{
				Master.log("Released on node", Colors.Orchid); // DEBUG
				Connection c = Master.getDraggingConnection();
				c.completeConnection(this);

				// add connection to both node's collection
				connections.Add(c);
				c.getOrigin().addConnection(c);

				Master.setDraggingConnection(false, null);
			}
		}
	}
}

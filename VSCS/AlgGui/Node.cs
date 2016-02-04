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

// TODO: way to represent which group node it is (input1, input2, output1, output2) so can be referenced from command line

// (connect format should be: "connect -[outputNodeRepresetnationID (from)],[nodeNum] -[inputNodeRepresentationID (to)],[nodeNum]

namespace AlgGui
{
	public class Node
	{
		// TODO: add labels on hover (right click allows you to change?)

		// member variables
		private Ellipse m_body = new Ellipse();
		private Representation m_parent;

		// NOTE: offsets are from TOP LEFT CORNER OF REPRESENTATION
		private int m_offsetX = 0; 
		private int m_offsetY = 0;

		private List<Connection> m_connections = new List<Connection>();

		bool m_isInput = true; // false means output

		// construction
		public Node(Representation parent, int x, int y, int offX, int offY, int size, bool isInput) // PASS IN X AND Y OF REPRESENTATION
		{
			this.m_parent = parent;
			m_offsetX = offX;
			m_offsetY = offY;
			m_isInput = isInput;

			createDrawing(x, y, size);
		}

		// properties
		public int getOffsetX() { return m_offsetX; }
		public int getOffsetY() { return m_offsetY; }

		public Ellipse getBody() { return m_body; }

		public Representation getParent() { return m_parent; }

		public double getCurrentX() { return Canvas.GetLeft(m_body); }
		public double getCurrentY() { return Canvas.GetTop(m_body); }

		public bool isInput() { return m_isInput; }

		public void addConnection(Connection c) { m_connections.Add(c); }
		public void removeConnection(Connection c) { m_connections.Remove(c); }
		

		private void createDrawing(int x, int y, int size)
		{
			// create body
			//body = new Ellipse();
			m_body.Fill = Brushes.White;
			m_body.Stroke = new SolidColorBrush(Colors.Black);
			m_body.StrokeThickness = 2;
			m_body.Height = size;
			m_body.Width = size;
			Canvas.SetZIndex(m_body, 10);

			Canvas.SetLeft(m_body, x + m_offsetX);
			Canvas.SetTop(m_body, y + m_offsetY);

			Master.getCanvas().Children.Add(m_body);
			Master.log("Representation node created with offsets (" + m_offsetX + "," + m_offsetY + ")");

			// create event handlers
			m_body.MouseDown += new MouseButtonEventHandler(body_mouseDown);
			m_body.MouseUp += new MouseButtonEventHandler(body_mouseUp);
		}

		public void move(double x, double y)
		{
			Canvas.SetLeft(m_body, x + m_offsetX);
			Canvas.SetTop(m_body, y + m_offsetY);

			// update connections
			foreach (Connection c in m_connections)
			{
				c.adjustRelatedPoint(this);
			}
		}

		// EVENT HANDLERS

		private void body_mouseDown(object sender, MouseEventArgs e)
		{
			Master.log("Node has been clicked!", Colors.Tomato); // DEBUG
		
			Connection con = new Connection(this);
		}
		private void body_mouseUp(object sender, MouseEventArgs e)
		{
			Master.log("Registered up");
			if (Master.getDraggingConnection() != null)
			{
				Master.log("Released on node", Colors.Orchid); // DEBUG
				Connection con = Master.getDraggingConnection();
				if (!con.completeConnection(this)) { return; }

				// add connection to both node's collection
				m_connections.Add(con);
				con.getOrigin().addConnection(con);

				Master.setDraggingConnection(false, null);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlgGui
{
	public class Connection
	{
		private Line body = new Line();
		private Node inNode, outNode; // same as below, but referenced for different reasons

		private Node origin;
		private Node end;

		private bool m_completed = false;

		//private bool initial = true;

		/*public Connection(Node n1, Node n2)
		{
			node1 = n1;
			node2 = n2;
		}*/

		public Connection(Node start)
		{
			Master.log("Connection initialized");
			origin = start;
			if (start.isInput()) { inNode = start; }
			else { outNode = start; }
			createDrawing();
		}

		public Node getOrigin() { return origin; }
		public Node getEnd() { return end; }
		public bool isComplete() { return m_completed; }
		public Line getBody() { return body; }

		public Node getInputNode() { return inNode; }
		public Node getOutputNode() { return outNode; }

		// returns true on success, false on failure
		public bool completeConnection(Node other)
		{
			end = other;
			if (outNode == null) { outNode = other; }
			else { inNode = other; }

			// make sure the nodes aren't the same
			if (origin.Equals(end))
			{
				Master.getCanvas().Children.Remove(body);
				return false;
			}

			adjustSecondPoint((int)(end.getCurrentX() + end.getBody().Width / 2), (int)(end.getCurrentY() + end.getBody().Height / 2));
			m_completed = true;

			int inputRepID = inNode.getParent().getID();
			int outputRepID = outNode.getParent().getID();
			Master.log("Connection created - OutputID: " + outputRepID + " InputID: " + inputRepID);

			body.IsHitTestVisible = true;
			return true;
		}

		public void adjustRelatedPoint(Node node)
		{
			if (node.Equals(origin)) { adjustFirstPoint((int)(origin.getCurrentX() + origin.getBody().Width / 2), (int)(origin.getCurrentY() + origin.getBody().Height / 2)); }
			else if (node.Equals(end)) { adjustSecondPoint((int)(end.getCurrentX() + end.getBody().Width / 2), (int)(end.getCurrentY() + end.getBody().Height / 2)); }
		}

		public void adjustFirstPoint(int x, int y)
		{
			body.X1 = x;
			body.Y1 = y;
		}

		public void adjustSecondPoint(int x, int y)
		{
			body.X2 = x;
			body.Y2 = y;
		}

		private void createDrawing()
		{
			body.Stroke = Brushes.Black;
			body.StrokeThickness = 2;
			body.X1 = origin.getCurrentX() + origin.getBody().Width / 2;
			body.Y1 = origin.getCurrentY() + origin.getBody().Height / 2; 
			body.X2 = origin.getCurrentX();
			body.Y2 = origin.getCurrentY();
			body.IsHitTestVisible = false; // make click-throughable
			Canvas.SetZIndex(body, 0);

			Master.getCanvas().Children.Add(body);
			Master.setDraggingConnection(true, this);

			body.MouseDown += new MouseButtonEventHandler(body_mouseDown);
			body.MouseMove += new MouseEventHandler(body_mouseDown);
		}


		// EVENT HANDLERS

		// this event handler is also added as a mousemove, so that you can click and drag to delete multiple connections
		private void body_mouseDown(object sender, MouseEventArgs e)
		{
			if (e.RightButton == MouseButtonState.Pressed)
			{
				origin.removeConnection(this);
				end.removeConnection(this);

				Master.getCanvas().Children.Remove(body);
				Master.log("Connection destroyed");
			}
		}
	}
}

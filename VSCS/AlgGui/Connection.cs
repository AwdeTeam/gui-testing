using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace AlgGui
{
	public class Connection
	{
		private Line body = new Line();
		private Node node1, node2; //1 is input, 2 is output

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
			if (start.isInput()) { node1 = start; }
			else { node2 = start; }
			createDrawing();
		}

		public Node getOrigin() { return origin; }
		public Node getEnd() { return end; }
		public bool isComplete() { return m_completed; }
		public Line getBody() { return body; }


		public void completeConnection(Node other)
		{
			end = other;
			if (node2 == null) { node2 = other; }
			else { node1 = other; }

			adjustSecondPoint((int)(end.getCurrentX() + end.getBody().Width / 2), (int)(end.getCurrentY() + end.getBody().Height / 2));
			m_completed = true;
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
			body.IsHitTestVisible = false;
			Canvas.SetZIndex(body, 0);

			Master.getCanvas().Children.Add(body);
			Master.setDraggingConnection(true, this);
		}
	}
}

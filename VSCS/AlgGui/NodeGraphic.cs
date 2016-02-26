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
	public class NodeGraphic
	{
		private Ellipse m_body = new Ellipse();
		
		private Node m_parent;

		private SolidColorBrush m_brushFill = new SolidColorBrush(Colors.White);
		private SolidColorBrush m_brushBorder = new SolidColorBrush(Colors.Black);

		private int m_offsetX = 0;
		private int m_offsetY = 0;

		// construction
		public NodeGraphic(Node parent)
		{
			m_parent = parent;
			m_offsetX = parent.getParent().getGraphic().getNodeOffsetX(parent.isInput(), parent.getGroupNum());
			m_offsetY = parent.getParent().getGraphic().getNodeOffsetY(parent.isInput());

			createDrawing();
		}

		// -- FUNCTIONS --
		private void createDrawing()
		{
			// create body
			m_body.Fill = m_brushFill;
			m_body.Stroke = m_brushBorder;
			m_body.StrokeThickness = 2;
			m_body.Height = GraphicContainer.NODE_SIZE;
			m_body.Width = GraphicContainer.NODE_SIZE;
			Canvas.SetZIndex(m_body, GraphicContainer.NODE_Z_LEVEL);

			// inital position
			move(m_parent.getParent().getGraphic().getCurrentX(), m_parent.getParent().getGraphic().getCurrentY());

			// add to canvas
			Master.getCanvas().Children.Add(m_body);

			// event handlers?
		}

		public void move(double x, double y)
		{
			Canvas.SetLeft(m_body, x + m_offsetX);
			Canvas.SetTop(m_body, y + m_offsetY);
			// CONNECTION STUFF?
		}

		// -- EVENT HANDLERS --

		private void evt_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void evt_MouseUp(object sender, MouseEventArgs e)
		{

		}
	}
}

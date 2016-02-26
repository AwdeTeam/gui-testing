using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;

namespace AlgGui
{
	public class ConnectionGraphic
	{
		private const int Z_LEVEL = 9;

		// member variables
		private Line m_body = new Line();
		private Label m_lblTypeName = new Label();
		private SolidColorBrush m_brushStroke = new SolidColorBrush(Colors.Black);

		private Connection m_parent;

		private bool m_isDragging = false;

		// construction
		public ConnectionGraphic(Connection parent)
		{
			m_parent = parent;
			createDrawing();
		}

		// -- FUNCTIONS -- 
		private void createDrawing()
		{
			m_body.Stroke = m_brushStroke;
			m_body.StrokeThickness = 2;
			Node origin = m_parent.getOrigin();
			m_body.X1 = origin.getGraphic().getCurrentX() + GraphicContainer.NODE_SIZE / 2;
			m_body.Y1 = origin.getGraphic().getCurrentY() + GraphicContainer.NODE_SIZE / 2;
			m_body.X2 = origin.getGraphic().getCurrentX();
			m_body.Y2 = origin.getGraphic().getCurrentY();
			m_body.IsHitTestVisible = false; // make click-throughable
			Canvas.SetZIndex(m_body, Z_LEVEL);

			// TODO: null check necessary?
			m_lblTypeName.Content = m_parent.getOrigin().getDatatype();
			Canvas.SetLeft(m_lblTypeName, m_body.X2);
			Canvas.SetTop(m_lblTypeName, m_body.Y2);

			Master.getCanvas().Children.Add(m_body);
			Master.getCanvas().Children.Add(m_lblTypeName);

			Master.setDraggingConnection(true, this);
			m_isDragging = true;
		}

		// moves the end of the line attached to passed node
		public void adjustRelatedPoint(Node node)
		{
			//if (node.Equals(m_origin)) { adjustFirstPoint((int)(m_origin.getCurrentX() + m_origin.getBody().Width / 2), (int)(m_origin.getCurrentY() + m_origin.getBody().Height / 2)); }
			//else if (node.Equals(m_end)) { adjustSecondPoint((int)(m_end.getCurrentX() + m_end.getBody().Width / 2), (int)(m_end.getCurrentY() + m_end.getBody().Height / 2)); }
		}

		// adjusts "origin" connected point
		public void adjustFirstPoint(int x, int y)
		{
			m_body.X1 = x;
			m_body.Y1 = y;
		}



		// adjusts "end" connected point
		public void adjustSecondPoint(int x, int y)
		{
			m_body.X2 = x;
			m_body.Y2 = y;
			if (m_isDragging == true)
			{
				Canvas.SetLeft(m_lblTypeName, m_body.X2);
				Canvas.SetTop(m_lblTypeName, m_body.Y2);
			}
		}


		// -- EVENT HANDLERS --

		public void evt_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_isDragging)
			{
				Point p = e.GetPosition(Master.getCanvas());
				int x = (int)p.X;
				int y = (int)p.Y;
				adjustSecondPoint(x, y);
			}
		}
	}
}

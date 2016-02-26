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
	public class ConnectionGraphic
	{
		private const int Z_LEVEL = 9;

		// member variables
		private Line m_body = new Line();
		private Label m_lblTypeName = new Label();

		private SolidColorBrush m_brushStroke = new SolidColorBrush(Colors.Black);

		private Connection m_parent;

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
			/*m_body.X1 = origin.getCurrentX() + origin.getBody().Width / 2;
			m_body.Y1 = origin.getCurrentY() + origin.getBody().Height / 2;
			m_body.X2 = origin.getCurrentX();
			m_body.Y2 = origin.getCurrentY();*/
			m_body.IsHitTestVisible = false; // make click-throughable
			Canvas.SetZIndex(m_body, Z_LEVEL);
		}
	}
}

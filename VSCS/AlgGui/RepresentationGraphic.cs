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
	class RepresentationGraphic
	{
		private const int NODE_SIZE = 10;
		private const int MINIMUM_WIDTH = 25;
		private const int PADDING_TOP = 24;
		private const int PADDING_LEFT = 6;

		private int m_id = 0; // this should be passed in from representation, don't need access to representation from here?
		
		

		// gui elements
		private Rectangle m_body = new Rectangle();
		private Rectangle m_board = new Rectangle();
		private Label m_lblName = new Label();
		private Label m_lblID = new Label();
		private TextBox m_txtContent = new TextBox();
		private Color m_baseColor = Colors.SeaGreen; // TODO: PUT IN CONSTRUCTOR
		private SolidColorBrush m_brushBase;
		private SolidColorBrush m_brushLightenedBase;

		// construction
		public RepresentationGraphic(int x, int y, int w, int h, int numIn, int numOut)
		{
			// create body
			m_body.Fill = new SolidColorBrush(m_baseColor);
		}

		// -- FUNCTIONS --

		// find least amount of space to fit all nodes
		private int calcOptimalWidth(int numIn, int numOut)
		{
			int totalXIn = numIn * NODE_SIZE;
			int totalXOut = numOut * NODE_SIZE;

			int widest = totalXIn;
			if (totalXOut > widest) { widest = totalXOut; }

			if (widest < MINIMUM_WIDTH) { widest = MINIMUM_WIDTH; } // make it at least 25 pixels wide
			return widest;
		}

		// assumes m_baseColor has been assigned appropriately
		private void setBrushes()
		{
			m_brushBase = new SolidColorBrush(m_baseColor);
			//m_brushLightenedBase = new Solid
		}

		private color lighten()
	}
}

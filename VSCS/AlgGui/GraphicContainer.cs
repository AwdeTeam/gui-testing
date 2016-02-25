using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AlgGui
{
	class GraphicContainer
	{
		/*private bool m_isDraggingRepresentation;
		private RepresentationGraphic m_draggingRepresentation;*/

		// this class is "central hub" for all event handlers. Takes any events from mainwindow and distributes as necessary.
		// (handles dragging controls, etc)
		// ONLY GOAL OF EVENT HANDLERS HERE ARE TO CALL/ROUTE THE APPROPRIATE EVENT HANDLERS ON COMPONENTS (unless global thing like panning)

		// graphical data constants
		public static const int NODE_SIZE = 10;

		public static const int REP_MINIMUM_WIDTH = 85;
		public static const int REP_MINIMUM_HEIGHT = 80;
		public static const int REP_BOARD_PADDING_TOP = 24;
		public static const int REP_BOARD_PADDING_LEFT = 6;

		public static const int REP_Z_LEVEL = 10;
		public static const int NODE_Z_LEVEL = 10;
		public static const int CONNECTION_Z_LEVEL = 9;



		public GraphicContainer()
		{

		}




		// EVENT HANDLERS
		public void evt_MouseMove(object sender, MouseEventArgs e)
		{

		}

		public void evt_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		public void evt_MouseUp(object sender, MouseButtonEventArgs e)
		{

		}
	}
}

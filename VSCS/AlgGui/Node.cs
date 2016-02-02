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
	class Node
	{
		// member variables
		private Ellipse body = new Ellipse();

		// NOTE: offsets are from TOP LEFT CORNER OF REPRESENTATION
		private int offsetX = 0; 
		private int offsetY = 0;

		// construction
		public Node(int x, int y, int offX, int offY, int size) // PASS IN X AND Y OF REPRESENTATION
		{
			offsetX = offX;
			offsetY = offY;

			createDrawing(x, y, size);
		}

		// properties
		public int getOffsetX() { return offsetX; }
		public int getOffsetY() { return offsetY; }

		public Ellipse getBody() { return body; }

		private void createDrawing(int x, int y, int size)
		{
			// create body
			//body = new Ellipse();
			body.Stroke = new SolidColorBrush(Colors.Black);
			body.StrokeThickness = 2;
			body.Height = size;
			body.Width = size;

			Canvas.SetLeft(body, x + offsetX);
			Canvas.SetTop(body, y + offsetY);

			Master.getCanvas().Children.Add(body);
			Master.log("Representation node created with offsets (" + offsetX + "," + offsetY + ")");

			// create event handlers
		}
	}
}

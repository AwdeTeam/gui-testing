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
	class Representation
	{
		// member variables
		private Rectangle body = new Rectangle();
		private List<Ellipse> nodes = new List<Ellipse>();

		private bool isBodyClicked = false;
		private double bodyRelativeX = 0; // relative coordinates from body to where mouse clicked
		private double bodyRelativeY = 0;

		// construction
		public Representation()
		{
			Master.log("Creating representation");
			createDrawing(100, 100, 100, 100, 0, 0);
		}

		private void createDrawing(int x, int y, int w, int h, int numIn, int numOut)
		{
			// create body
			body = new Rectangle();
			body.Fill = new SolidColorBrush(Colors.RoyalBlue);
			body.Height = h;
			body.Width = w;
			Canvas.SetLeft(body, x);
			Canvas.SetTop(body, y);
			Master.getCanvas().Children.Add(body);
			Master.log("Representation drawing completed (" + x + "," + y + "," + w + "," + h + "," + numIn + "," + numOut + ")");

			// body event handlers
			body.MouseDown += new MouseButtonEventHandler(body_MouseDown);
			body.MouseUp += new MouseButtonEventHandler(body_MouseUp);
			body.MouseMove += new MouseEventHandler(body_MouseMove);
		}


		// EVENT HANDLERS


		private void body_MouseDown(object sender, MouseEventArgs e)
		{
			Master.log("I was clicked upon!", Colors.Salmon);
			isBodyClicked = true;

			// get relative coordinates
			Point p = e.GetPosition(Master.getCanvas());
			double x = p.X;
			double y = p.Y;

			bodyRelativeX = x - Canvas.GetLeft(body);
			bodyRelativeY = y - Canvas.GetTop(body);
		}
		private void body_MouseUp(object sender, EventArgs e)
		{
			isBodyClicked = false;
		}
		private void body_MouseMove(object sender, MouseEventArgs e)
		{
			if (isBodyClicked) 
			{ 
				//Master.log("Mouse moved over me!", Colors.Salmon);

				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X - bodyRelativeX;
				double y = p.Y - bodyRelativeY;

				Canvas.SetLeft(body, x);
				Canvas.SetTop(body, y);
			}
		}
	}
}

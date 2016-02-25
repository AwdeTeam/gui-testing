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
		// constants
		private const int NODE_SIZE = 10;

		private const int MINIMUM_WIDTH = 85;
		private const int MINIMUM_HEIGHT = 80;

		private const int PADDING_TOP = 24;
		private const int PADDING_LEFT = 6;

		private const int Z_LEVEL = 10;



		// info variables
		private int m_id = 0; // stored only once, never changed, passed in from representation
		private string m_name = "Unnamed algorithm";
		private string m_version = "##.## XXX";
		private string m_algorithm = "No-Op"; //TODO: merge with python algorithm
		
		
		// gui elements
		private Rectangle m_body = new Rectangle();
		private Rectangle m_board = new Rectangle();
		private Label m_lblName = new Label();
		private Label m_lblID = new Label();
		//private TextBox m_txtContent = new TextBox(); // TODO: why textbox? Does user need to edit?
		private Label m_lblContent = new Label();
		private Color m_baseColor = Colors.SeaGreen;
		private SolidColorBrush m_brushBorder = new SolidColorBrush(Colors.Black); // prob better way to do this?
		private SolidColorBrush m_brushForeground;
		private SolidColorBrush m_brushBase;
		private SolidColorBrush m_brushLightenedBase;
		private Thickness m_noThickness = new Thickness(0);

		// construction
		// TODO: overloaded part, make this a function that takes a lot more, than constructors just call different ones with some default parameters instead
		public RepresentationGraphic(int id, string name, string version, string algorithm, int numIn, int numOut)
		{
			m_id = id;
			m_name = name;
			m_version = version;
			m_algorithm = algorithm;

			createDrawing(100, 100, numIn, numOut, m_baseColor);
		}

		// -- FUNCTIONS --

		private void createDrawing(int x, int y, int numIn, int numOut, Color initialColor)
		{
			int width = calcOptimalWidth(numIn, numOut);

			m_baseColor = initialColor;
			setBrushes();

			// create body
			m_body.Fill = m_brushBase;
			m_body.Height = MINIMUM_HEIGHT;
			m_body.Width = width;
			m_body.RadiusX = 5;
			m_body.RadiusY = 5;
			m_body.Stroke = m_brushBorder;
			m_body.StrokeThickness = 2;
			Canvas.SetZIndex(m_body, Z_LEVEL);

			// board (inner part of body)
			m_board.Fill = m_brushLightenedBase;
			m_board.Height = MINIMUM_HEIGHT - 30;
			m_board.Width = width - 12;
			m_board.RadiusX = 3;
			m_board.RadiusY = 3;
			Canvas.SetZIndex(m_board, Z_LEVEL);

			// labels
			m_lblID.Margin = m_noThickness;
			m_lblID.Content = m_id + " " + m_version;
			m_lblID.Foreground = m_brushForeground;
			m_lblID.IsHitTestVisible = false;
			Canvas.SetZIndex(m_lblID, Z_LEVEL);

			m_lblName.Margin = m_noThickness;
			m_lblName.Content = m_name;
			m_lblName.Foreground = m_brushForeground;
			m_lblName.Padding = m_noThickness;
			m_lblName.Margin = m_noThickness;
			m_lblName.Height = 20;
			Canvas.SetZIndex(m_lblName, Z_LEVEL);

			m_lblContent.Foreground = m_brushForeground;
			m_lblContent.Content = m_algorithm + "\n" + "Accuracy stuff blah blah blah blah";
			m_lblContent.IsHitTestVisible = false;
			m_lblContent.Width = m_board.Width;
			Canvas.SetZIndex(m_lblContent, Z_LEVEL);

			moveAbsolute(x, y);

			// TODO: mousedown events will be handled in here?

			// ADD ALL THE THINGS!!
			Canvas cnvs = Master.getCanvas();
			cnvs.Children.Add(m_body);
			cnvs.Children.Add(m_board);
			cnvs.Children.Add(m_lblID);
			cnvs.Children.Add(m_lblContent);
			cnvs.Children.Add(m_lblName);
		}

		// moves entire representation to passed x and y (based on upper left corner)
		private void moveAbsolute(int x, int y) // public?
		{
			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);

			Canvas.SetLeft(m_board, x + PADDING_LEFT);
			Canvas.SetTop(m_board, y + PADDING_TOP);

			Canvas.SetLeft(m_lblID, x);
			Canvas.SetTop(m_lblID, y);

			Canvas.SetLeft(m_lblName, x + m_body.Width + 2);
			Canvas.SetTop(m_lblName, x + (m_body.Height / 2) - (m_lblName.Height / 2));

			Canvas.SetLeft(m_lblContent, x + PADDING_LEFT);
			Canvas.SetTop(m_lblContent, y + PADDING_TOP);
		}

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
		// (Use this to avoid unnecessary recalling of functions over and over)
		private void setBrushes()
		{
			m_brushBase = new SolidColorBrush(m_baseColor);
			m_brushLightenedBase = new SolidColorBrush(lightenColor(m_baseColor, 0.6f));
		}

		private Color lightenColor(Color color, float p)
		{
			float red = (255 - color.R) * p + color.R;
			float green = (255 - color.G) * p + color.G;
			float blue = (255 - color.B) * p + color.B;
			return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
		}

		// -- EVENT HANDLERS --
		// (if starts with evt, second hand, being called indirectly from container)
	}
}

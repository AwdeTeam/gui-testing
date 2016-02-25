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
	public class Representation
	{
        public enum AlgorithmFamily { Classifier, Clustering, DimensionReduction, Operation };

		protected const int NODE_SIZE = 10;

		// member variables
		protected int m_id = 0;
		protected Rectangle m_body = new Rectangle();
        protected Rectangle m_board = new Rectangle();
        protected List<Node> m_nodes = new List<Node>();

        protected int m_leftPadding = 6;
        protected int m_topPadding = 24;

		protected bool m_isBodyClicked = false;
        protected bool m_hasFocus = false;
		protected double m_bodyRelativeX = 0; // relative coordinates from body to where mouse clicked
        protected double m_bodyRelativeY = 0;

        protected Datatype[] inputs;
        protected Datatype[] outputs;

        //Information Variables
        public Label m_lblName = new Label();
        public Label m_lblID = new Label();
        public TextBox m_txtContent = new TextBox();

        public String m_name = "unnamed algorithm";
        public String m_version = "##.## XXX";
        public AlgorithmFamily m_family = AlgorithmFamily.Operation;
        public String m_algorithm = "No-Op"; //TODO Merge with Python Algorithm IDs

        public Color m_baseColor = Colors.SeaGreen;

		// construction
		public Representation(int numIn, int numOut)
		{
			Master.log("----Creating representation----");
			m_id = Master.getNextRepID();
			Master.log("ID: " + m_id, Colors.GreenYellow);
			int width = calcOptimalWidth(numIn, numOut) + 60;
			createDrawing(100, 100, width, 80, numIn, numOut);
		}

        public Representation(Datatype[] inputs, Datatype[] outputs)
        {
            Master.log("----Creating representation----");
            m_id = Master.getNextRepID();
            Master.log("ID: " + m_id, Colors.GreenYellow);
            int width = calcOptimalWidth(inputs.Length, outputs.Length) + 60;
            this.inputs = inputs;
            this.outputs = outputs;
            createDrawing(100, 100, width, 80, inputs.Length, outputs.Length);
        }

		// properties
		public void setLabelText(string text) { m_lblName.Content = text; }
		public int getID() { return m_id; }
		public Rectangle getBody() { return m_body; }
        //public Rectangle getOutline() { return m_outline; }
		public double getCurrentX() { return Canvas.GetLeft(m_body); }
		public double getCurrentY() { return Canvas.GetTop(m_body); }
		public double getRelativeX() { return m_bodyRelativeX; } // should only be necessary for middle clicking
		public double getRelativeY() { return m_bodyRelativeY; }
		public void setRelativeX(double x) { m_bodyRelativeX = x; }
		public void setRelativeY(double y) { m_bodyRelativeY = y; }


		// -- FUNCTIONS --

		

		// initialize graphics
		/*protected void createDrawing(int x, int y, int w, int h, int numIn, int numOut)
		{
            // create body
            m_body.Fill = new SolidColorBrush(m_baseColor);
            m_body.Height = h;
            m_body.Width = w;
            m_body.RadiusX = 5;
            m_body.RadiusY = 5;
            m_body.Stroke = (!m_hasFocus) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Yellow);
            m_body.StrokeThickness = 2;
            Canvas.SetZIndex(m_body, 10);

            m_board.Fill = new SolidColorBrush(lighten(m_baseColor, .6f));
            m_board.Height = h - 30;
            m_board.Width = w - 12;
            m_board.RadiusX = 3;
            m_board.RadiusY = 3;
            Canvas.SetZIndex(m_board, 11);

            Canvas.SetLeft(m_body, x);
            Canvas.SetTop(m_body, y);
            Canvas.SetLeft(m_board, x + m_leftPadding);
            Canvas.SetTop(m_board, y + m_topPadding);

            Master.getCanvas().Children.Add(m_body);
            Master.getCanvas().Children.Add(m_board);
            Master.log("Representation drawing completed (" + x + "," + y + "," + w + "," + h + "," + numIn + "," + numOut + ")");

            // body event handlers
            m_board.MouseDown += new MouseButtonEventHandler(body_MouseDown);
            m_board.MouseUp += new MouseButtonEventHandler(body_MouseUp);
            m_board.MouseMove += new System.Windows.Input.MouseEventHandler(body_MouseMove); //Argh!!!

       
            m_body.MouseDown += new MouseButtonEventHandler(body_MouseDown);
            m_body.MouseUp += new MouseButtonEventHandler(body_MouseUp);
            m_body.MouseMove += new System.Windows.Input.MouseEventHandler(body_MouseMove);

            // create nodes

            // find left of center point (for centering the two groups of nodes on the representation)
            int inStartPoint = (int)((m_body.Width - (numIn * NODE_SIZE)) / 2);
            int outStartPoint = (int)((m_body.Width - (numOut * NODE_SIZE)) / 2);

            // input nodes
            for (int i = 0; i < numIn; i++)
            {
                Node n = new Node(this, x, y, inStartPoint + i * NODE_SIZE, -NODE_SIZE, NODE_SIZE, true, i);
                n.datatype = inputs == null ? null : inputs[i];
                m_nodes.Add(n);
            }
            // output nodes
            for (int i = 0; i < numOut; i++)
            {
                Node n = new Node(this, x, y, outStartPoint + i * NODE_SIZE, (int)m_body.Height, NODE_SIZE, false, i);
                n.datatype = outputs == null ? null : outputs[i];
                m_nodes.Add(n);
            }

            // create labels
            m_lblID.Margin = new Thickness(0);
            m_lblID.Content = m_id + " " + m_version;
            m_lblID.Foreground = new SolidColorBrush(Colors.Black);
            m_lblID.IsHitTestVisible = false;
            Canvas.SetZIndex(m_lblID, 10);
            Canvas.SetLeft(m_lblID, x);
            Canvas.SetTop(m_lblID, y);

            Master.getCanvas().Children.Add(m_lblID);

            m_txtContent.Foreground = new SolidColorBrush(Colors.Black);
            m_txtContent.Text = m_algorithm + "\n" + "Accuracy stuff blah blah blah blah";
            m_txtContent.Background = null;
            m_txtContent.BorderBrush = new SolidColorBrush(Colors.Transparent);
            m_txtContent.Focusable = false;
            m_txtContent.Width = m_body.Width - m_leftPadding- 6;
            m_txtContent.IsHitTestVisible = false;

            Canvas.SetLeft(m_txtContent, x + m_leftPadding);
            Canvas.SetTop(m_txtContent, y + m_topPadding);
            Canvas.SetZIndex(m_txtContent, 12);

            Master.getCanvas().Children.Add(m_txtContent);

            m_lblName.Foreground = new SolidColorBrush(Colors.Black);
            m_lblName.Content = m_name;
            m_lblName.Padding = new Thickness(0);
            m_lblName.Margin = new Thickness(0);
            m_lblName.Height = 20;

            Canvas.SetLeft(m_lblName, x + m_body.Width + 2);
            Canvas.SetTop(m_lblName, y + (m_body.Height / 2) - (m_lblName.Height / 2)); // center it!

            Master.getCanvas().Children.Add(m_lblName);
            Master.log("Representation label created");

            // label event handlers
            m_lblName.MouseDown += new MouseButtonEventHandler(label_MouseDown);
		}

		// this moves the representation...self documentation ftw
		// NOTE: leave this public, can later be accessed from command line? (for command scripts)
		public virtual void move(double x, double y)
		{
            // move body
            Canvas.SetLeft(m_body, x);
            Canvas.SetTop(m_body, y);
            Canvas.SetLeft(m_board, x + m_leftPadding);
            Canvas.SetTop(m_board, y + m_topPadding);

            // move nodes
            foreach (Node n in m_nodes)
            {
                n.move(x, y);
            }

            // move labels
            Canvas.SetLeft(m_lblName, x + m_body.Width + 2);
            Canvas.SetTop(m_lblName, y + (m_body.Height / 2) - (m_lblName.Height / 2));

            Canvas.SetLeft(m_txtContent, x + m_leftPadding);
            Canvas.SetTop(m_txtContent, y + m_topPadding);

            Canvas.SetLeft(m_lblID, x);
            Canvas.SetTop(m_lblID, y);
		}

		// -- EVENT HANDLERS --

		// if click on label, auto put text in command line to edit label content
		protected void label_MouseDown(object sender, MouseEventArgs e)
		{
			Master.log("Preparing to update label");
			Master.setCommandPrompt("edit rep -" + m_id + " -lbl -\"");
		}

		protected void body_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Master.log("Representation ID " + m_id + " was clicked on", Colors.Salmon); // DEBUG
				m_isBodyClicked = true;
                m_hasFocus = true;

				// get relative coordinates
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X;
				double y = p.Y;

				m_bodyRelativeX = x - Canvas.GetLeft(m_body);
				m_bodyRelativeY = y - Canvas.GetTop(m_body);

				// tell main window that we're being dragged so it continues action even if mouse moves out of box
				Master.setDragging(true, this);
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
				// right clicking on representation quick-puts text in command line to allow editing properties of rep
				Master.log("Right click", Colors.DarkSeaGreen); // DEBUG
				Master.log("Editing representation id " + m_id);
				Master.setCommandPrompt("edit rep -" + m_id + " -");
			}
		}
		protected void body_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released)
			{
				m_isBodyClicked = false;
				Master.setDragging(false, null);
			}
		}
		public void body_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_isBodyClicked) 
			{
				// move body
				Point p = e.GetPosition(Master.getCanvas());
				double x = p.X - m_bodyRelativeX;
				double y = p.Y - m_bodyRelativeY;

				this.move(x, y);
			}
		}

        private Color lighten(Color color, float p)
        {
            float red = (255 - color.R) * p + color.R;
            float green = (255 - color.G) * p + color.G;
            float blue = (255 - color.B) * p + color.B;
            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }*/
	}
}

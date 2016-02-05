﻿using System;
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
	public class Connection
	{
		// member variables
		private Line m_body = new Line();
		private Node m_inNode, m_outNode; // same as below, but referenced for different reasons

		private Node m_origin;
		private Node m_end;

		private bool m_completed = false; // if connection has been created/assigned to two nodes

		// construction
		public Connection(Node start)
		{
			Master.log("Connection initialized");
			m_origin = start;
			if (start.isInput()) { m_inNode = start; }
			else { m_outNode = start; }
			createDrawing();
		}

		// properties
		public Node getOrigin() { return m_origin; }
		public Node getEnd() { return m_end; }

		public bool isComplete() { return m_completed; }
		public Line getBody() { return m_body; }

		public Node getInputNode() { return m_inNode; }
		public Node getOutputNode() { return m_outNode; }

		// -- FUNCTIONS --

		// finishes creating connection/adds connection to both involved nodes
		// returns true on success, false on failure
		// TODO: Need to check not connecting representation to self
		public bool completeConnection(Node other)
		{
			m_end = other;
			if (m_outNode == null) { m_outNode = other; }
			else { m_inNode = other; }

			// make sure the nodes aren't the same
			if (m_origin.Equals(m_end))
			{
				Master.getCanvas().Children.Remove(m_body);
				return false;
			}

			// set end point to end node center
			adjustSecondPoint((int)(m_end.getCurrentX() + m_end.getBody().Width / 2), (int)(m_end.getCurrentY() + m_end.getBody().Height / 2));
			m_completed = true;

			int inputRepID = m_inNode.getParent().getID();
			int inputNodeID = m_inNode.getGroupNum();
			int outputRepID = m_outNode.getParent().getID();
			int outputNodeID = m_outNode.getGroupNum();
			Master.log("Connection created - OutputID: " + outputRepID + " (out-node " + outputNodeID + ") InputID: " + inputRepID + " (in-node " + inputNodeID + ")");

			m_body.IsHitTestVisible = true; // make clickable
			return true;
		}

		// moves the end of the line attached to passed node
		public void adjustRelatedPoint(Node node)
		{
			if (node.Equals(m_origin)) { adjustFirstPoint((int)(m_origin.getCurrentX() + m_origin.getBody().Width / 2), (int)(m_origin.getCurrentY() + m_origin.getBody().Height / 2)); }
			else if (node.Equals(m_end)) { adjustSecondPoint((int)(m_end.getCurrentX() + m_end.getBody().Width / 2), (int)(m_end.getCurrentY() + m_end.getBody().Height / 2)); }
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
		}

		// initialize graphics
		private void createDrawing()
		{
			m_body.Stroke = Brushes.Black;
			m_body.StrokeThickness = 2;
			m_body.X1 = m_origin.getCurrentX() + m_origin.getBody().Width / 2;
			m_body.Y1 = m_origin.getCurrentY() + m_origin.getBody().Height / 2; 
			m_body.X2 = m_origin.getCurrentX();
			m_body.Y2 = m_origin.getCurrentY();
			m_body.IsHitTestVisible = false; // make click-throughable
			Canvas.SetZIndex(m_body, 0);

			Master.getCanvas().Children.Add(m_body);
			Master.setDraggingConnection(true, this);

			// attach event handlers
			m_body.MouseDown += new MouseButtonEventHandler(body_mouseDown);
			m_body.MouseMove += new MouseEventHandler(body_mouseDown);
		}


		// -- EVENT HANDLERS --

		// this event handler is also added as a mousemove, so that you can click and drag to delete multiple connections
		private void body_mouseDown(object sender, MouseEventArgs e)
		{
			if (e.RightButton == MouseButtonState.Pressed)
			{
				// remove all the things! (effectively delete connection)
				m_origin.removeConnection(this);
				m_end.removeConnection(this);

				Master.getCanvas().Children.Remove(m_body);
				Master.log("Connection destroyed");
			}
		}
	}
}

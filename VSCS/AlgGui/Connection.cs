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
	class Connection
	{
		private Line body = new Line();
		private Node node1, node2;

		private bool initial = true;

		public Connection(Node n1, Node n2)
		{
			node1 = n1;
			node2 = n2;
		}
	}
}

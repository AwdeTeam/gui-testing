using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgGui
{
	public class GraphicFactory
	{
		public static RepresentationGraphic createRepresentationGraphic(Representation parent, string name, string version, string algorithm, int numIn, int numOut)
		{
			return new RepresentationGraphic(parent, name, version, algorithm, numIn, numOut);
		}

		public static RepresentationGraphic createRepresentationGraphic(Representation parent, int numIn, int numOut)
		{
			return new RepresentationGraphic(parent, "Unnamed algorithm", "##.## XXX", "No-Op", numIn, numOut);
		}

		public static RepresentationGraphic createRepresentationGraphic(Representation parent)
		{
			return new RepresentationGraphic(parent, "Unnamed algorithm", "##.## XXX", "No-Op", 1, 1);
		}
	
		public static NodeGraphic createNodeGraphic(Node parent, int offX, int offY, int size, int z)
		{
			return new NodeGraphic(parent, offX, offY, size, z);
		}
	
	
	}
}

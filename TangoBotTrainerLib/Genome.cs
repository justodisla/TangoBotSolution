using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;

namespace TangoBotTrainerCoreLib
{
    internal class Genome : IGenome
    {
        internal class NodeGene : IGene.INodeGene
        {
            public int Id { get; }
            public int InnovationNumber { get; }
            public int ModuleId { get; }
            public string Name { get; }
            public NodeType Type { get; }
            public int Layer { get; }
            public NodeGene(int id, int innovationNumber, int moduleId, string name, NodeType type, int layer)
            {
                Id = id;
                InnovationNumber = innovationNumber;
                ModuleId = moduleId;
                Name = name;
                Type = type;
                Layer = layer;
            }
        }

        internal class ConnectionGene : IGene.IConnectionGene
        {
            public int Id { get; }
            public int InnovationNumber { get; }
            public int ModuleId { get; }
            public string Name { get; }
            public int FromNode { get; }
            public int ToNode { get; }
            public double Weight { get; }
            public bool Enabled { get; }
            public ConnectionGene(int id, int innovationNumber, int moduleId, string name, int fromNode, int toNode, double weight, bool enabled)
            {
                Id = id;
                InnovationNumber = innovationNumber;
                ModuleId = moduleId;
                Name = name;
                FromNode = fromNode;
                ToNode = toNode;
                Weight = weight;
                Enabled = enabled;
            }
        }

        public Genome()
        {
        }

        public IGene[] GetGenes<T>()
        {
            throw new NotImplementedException();
        }
    }
}

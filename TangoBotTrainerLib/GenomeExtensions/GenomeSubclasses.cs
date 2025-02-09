using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;
using static TangoBotTrainerApi.IGenome.IGene.INodeGene;
using static TangoBotTrainerApi.IGenome;

namespace TangoBotTrainerCoreLib
{
    internal partial class Genome : IGenome
    {

        internal class Gene : IGenome.IGene
        {
            public int Id { get; set; }

            public int InnovationNumber { get; set; }

            public int ModuleId { get; set; }

            public bool Enabled { get; set; }

            public IGenome ParentGenome { get; set; }

            public IGene Mutate(MutationLevels mutationLevel = MutationLevels.DEFAULT, bool canIgnore = true)
            {
                return GeneticOperator.Mutate(this, mutationLevel);
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public Gene(int id, int innovationNumber, int moduleId, bool enabled, IGenome parentGenome)
            {
                Id = id;
                InnovationNumber = innovationNumber;
                ModuleId = moduleId;
                Enabled = enabled;
                ParentGenome = parentGenome;
            }
        }

        internal class NodeGene : Gene, IGene.INodeGene
        {
            public NodeType Type { get; }
            public int Layer { get; }
            public double Bias { get; set; }

            public NodeGene(int id, int innovationNumber, int moduleId, bool enabled, NodeType type, int layer, IGenome genome) : base(id, innovationNumber, moduleId, enabled, genome)
            {
                Type = type;
                Layer = layer;
            }

            public IGene.IConnectionGene[] GetConnections()
            {
                return null;
            }

            public IGene.IConnectionGene[] GetOutGoingConnections()
            {
                return ParentGenome.Genes
                    .OfType<ConnectionGene>()
                    .Where(cg => cg.FromNode == this.Id)
                    .ToArray();
            }

            public IGene.IConnectionGene[] GetIncomingConnections()
            {
                return ParentGenome.Genes
                    .OfType<ConnectionGene>()
                    .Where(cg => cg.ToNode == this.Id)
                    .ToArray();
            }
        }

        internal class ConnectionGene : Gene, IGene.IConnectionGene
        {
            public string Name { get; }
            public int FromNode { get; set; }
            public int ToNode { get; set; }
            public double Weight { get; set; }


            public ConnectionGene(int id, int innovationNumber, int moduleId, int fromNode, int toNode, double weight, bool enabled, IGenome genome) : base(id, innovationNumber, moduleId, enabled, genome)
            {
                FromNode = fromNode;
                ToNode = toNode;
                Weight = weight;
            }

            public void Reconnect()
            {
                throw new NotImplementedException();
            }
        }
    }
}

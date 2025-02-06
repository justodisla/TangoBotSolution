using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotTrainerApi;

namespace TangoBotTrainerCoreLib
{
    internal class GenomePool : IGenomePool
    {

        public List<IGenome> Pool { get; set; }

        public GenomePool(IGenome[] seedGenomes)
        {
            Pool = new List<IGenome>(seedGenomes);
        }

        public GenomePool()
        {
            
        }

        public IGenome.ISpecies[] GetSpecies()
        {
            throw new NotImplementedException();
        }

        public IGenome[] GetSpeciesChampion(IGenome sample, int championCount = -1)
        {
            throw new NotImplementedException();
        }

        public IGenome[] GetSpeciesFails(IGenome sample, int failCount = -1)
        {
            throw new NotImplementedException();
        }

        public void PruneSpecies(IGenome sample, int prunCount = -1)
        {
            throw new NotImplementedException();
        }

        public void RepopulateSpecies(IGenome sample, int repopulateCount = -1)
        {
            throw new NotImplementedException();
        }
    }
}

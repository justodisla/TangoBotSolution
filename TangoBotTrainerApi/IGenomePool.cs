using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotTrainerApi
{
    public interface IGenomePool
    {

        List<IGenome> Pool { get; set; }

        IGenome.ISpecies[] GetSpecies();

        void PruneSpecies(IGenome sample, int prunCount = -1);

        void RepopulateSpecies(IGenome sample, int repopulateCount = -1);

        IGenome[] GetSpeciesChampion(IGenome sample, int championCount = -1);

        IGenome[] GetSpeciesFails(IGenome sample, int failCount = -1);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.API.Persistence;

namespace TangoBot.Core.Api
{
    public interface IPriceActionTable
    {
        long GetDataPointCount();

        void AppendDataPoint(IEntity dataPoint);

        void AppendDataPointRange(IEnumerable<IEntity> dataPoints);

        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex);

        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex);

        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step);

        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step, long maxDataPoints);
        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step, long maxDataPoints, bool overwrite);
        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step, long maxDataPoints, bool overwrite, bool reverse);
        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step, long maxDataPoints, bool overwrite, bool reverse, bool append);   
        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step, long maxDataPoints, bool overwrite, bool reverse, bool append, bool update);
        void AppendDataPointRange(IEnumerable<IEntity> dataPoints, long startIndex, long endIndex, long step, long maxDataPoints, bool overwrite, bool reverse, bool append, bool update, bool validate);



    }
}

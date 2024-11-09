using MechanicBE.Models;

namespace MechanicBE.Services;

public interface IWorkHourEstimator
{
    double WorkHours(Commission commission);
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.Services;

namespace SeparateModels.Commands
{
    public class CancelLastAnnexHandler : IRequestHandler<CancelLastAnnexCommand, CancelLastAnnexResult>
    {
        private readonly IDataStore dataStore;

        public CancelLastAnnexHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public Task<CancelLastAnnexResult> Handle(CancelLastAnnexCommand command, CancellationToken cancellationToken)
        {
            var policy = dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.CancelLastAnnex();
            dataStore.CommitChanges();
            return Task.FromResult(new CancelLastAnnexResult
            {
                PolicyNumber = policy.Number,
                LastActiveVersionNumber = policy.Versions.LatestActive().VersionNumber
            });
        }
    }
}
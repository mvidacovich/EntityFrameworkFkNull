using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Transactions;

namespace EntityFrameworkFkNull
{
    class Program
    {
        static void Main()
        {
            var config = new Migrations.Configuration { CommandTimeout = 3600 };
            var migrator = new DbMigrator(config);
            migrator.Update();

            using (var transaction = GetTransaction()) // I've tried with and without transaction
            {
                var context = new DataContext();
                var userId = context.Set<User>().Where(x=>x.Name == "Foo").Select(x=>x.Id).Single();
                var ticket = context.Set<Ticket>().Single(x=>x.Name == "Bar");
                ticket.LockedByUserId = userId;

                context.SaveChanges(); 
                // Exception thrown here 'System.NullReferenceException' 
                //at System.Data.Entity.Core.Objects.DataClasses.RelatedEnd.GetOtherEndOfRelationship(IEntityWrapper wrappedEntity)
                //at System.Data.Entity.Core.Objects.EntityEntry.AddRelationshipDetectedByForeignKey(Dictionary`2 relationships, Dictionary`2 principalRelationships, EntityKey relatedKey, EntityEntry relatedEntry, RelatedEnd relatedEndFrom)
                //at System.Data.Entity.Core.Objects.EntityEntry.DetectChangesInForeignKeys()
                //at System.Data.Entity.Core.Objects.ObjectStateManager.DetectChangesInForeignKeys(IList`1 entries)
                //at System.Data.Entity.Core.Objects.ObjectStateManager.DetectChanges()
                //at System.Data.Entity.Core.Objects.ObjectContext.DetectChanges()
                //at System.Data.Entity.Internal.InternalContext.DetectChanges(Boolean force)
                //at System.Data.Entity.Internal.InternalContext.GetStateEntries(Func`2 predicate)
                //at System.Data.Entity.Internal.InternalContext.GetStateEntries()
                //at System.Data.Entity.Infrastructure.DbChangeTracker.Entries()
                //at System.Data.Entity.DbContext.GetValidationErrors()
                //at System.Data.Entity.Internal.InternalContext.SaveChanges()
                //at System.Data.Entity.Internal.LazyInternalContext.SaveChanges()
                //at System.Data.Entity.DbContext.SaveChanges()
                //at EntityFrameworkFkNull.Program.Main(String[] args) in h:\Projects\Spikes\EntityFrameworkFkNull\EntityFrameworkFkNull\Program.cs:line 27
                //at System.AppDomain._nExecuteAssembly(RuntimeAssembly assembly, String[] args)
                //at System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)
                //at Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()
                //at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
                //at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
                //at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
                //at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
                //at System.Threading.ThreadHelper.ThreadStart()
                transaction.Complete();
            }
        }

        private static TransactionScope GetTransaction()
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(60)
            };
            return new TransactionScope(TransactionScopeOption.RequiresNew, options);
        }
    }
}

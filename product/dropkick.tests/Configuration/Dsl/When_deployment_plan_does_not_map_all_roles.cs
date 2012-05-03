using System.Linq;
using NUnit.Framework;
using dropkick.Configuration.Dsl;
using dropkick.Engine;
using dropkick.tests.TestObjects;

namespace dropkick.tests.Configuration.Dsl
{
    [TestFixture]
    public class When_deployment_plan_does_not_map_all_roles
    {
        public TwoRoleDeploy Deployment { get; private set; }
        public DropkickDeploymentInspector Inspector { get; private set; }
        public RoleToServerMap Map { get; private set; }

        [TestFixtureSetUp]
        public void EstablishContext()
        {
            Deployment = new TwoRoleDeploy();
            Deployment.Initialize(new SampleConfiguration());

            Map = new RoleToServerMap();
            Map.AddMap("Web", "SrvTopeka09");
            Map.AddMap("Web", "SrvTopeka19");
            Inspector = new DropkickDeploymentInspector(Map);
        }

        [Test]
        public void It_should_list_roles_that_are_not_mapped()
        {
            var plan = Inspector.GetPlan(Deployment);
            plan.UnmappedRoles.ShouldNotBeNull();
            plan.UnmappedRoles.Count().ShouldBeEqualTo(1);
            plan.UnmappedRoles.First().ShouldBeEqualTo("Db");
        }
    }
}
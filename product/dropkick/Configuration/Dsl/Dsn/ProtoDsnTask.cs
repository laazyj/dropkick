// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace dropkick.Configuration.Dsl.Dsn
{
    using DeploymentModel;
    using Tasks;
    using Tasks.Dsn;

    public class ProtoDsnTask :
        BaseProtoTask,
        DsnOptions
    {
        readonly string _dsnName;
        string _databaseName;

        public ProtoDsnTask(string dsnName)
        {
            _dsnName = ReplaceTokens(dsnName);
        }

        public void ForDatabase(string name)
        {
            _databaseName = ReplaceTokens(name);
        }

        public override void RegisterRealTasks(PhysicalServer s)
        {
            s.AddTask(new DsnTask(s.Name, _dsnName, DsnAction.AddSystemDsn, DsnDriver.Sql(), _databaseName));
        }
    }
}
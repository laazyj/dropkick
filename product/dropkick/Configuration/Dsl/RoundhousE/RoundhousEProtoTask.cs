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

namespace dropkick.Configuration.Dsl.RoundhousE
{
    using System;
    using DeploymentModel;
    using dropkick.Tasks.RoundhousE;
    using Tasks;

    public class RoundhousEProtoTask :
        BaseProtoTask,
        RoundhousEOptions
    {
        string _environmentName;
        string _instanceName;
        string _databaseName;
        string _scriptsLocation;
        bool _useSimpleRecoveryMode;
       // string _databaseType;
        bool _drop;
        private string _userName;
        private string _password;

        //public RoundhousEOptions ForDatabaseType(string type)
        //{
        //    _databaseType = ReplaceTokens(type);
        //    return this;
        //}

        public RoundhousEOptions OnInstance(string name)
        {
            _instanceName = ReplaceTokens(name);
            return this;
        }

        public RoundhousEOptions OnDatabase(string name)
        {
            _databaseName = ReplaceTokens(name);
            return this;
        }

        public RoundhousEOptions DropDatabase(bool drop)
        {
            _drop = drop;
            return this;
        }

        public RoundhousEOptions WithScriptsFolder(string scriptsLocation)
        {
            _scriptsLocation = ReplaceTokens(scriptsLocation);
            return this;
        }

        public RoundhousEOptions ForEnvironment(string environment)
        {
            _environmentName = ReplaceTokens(environment);
            return this;
        }

        public RoundhousEOptions UseSimpleRecoveryMode(bool useSimple)
        {
            _useSimpleRecoveryMode = useSimple;
            return this;
        }

        public RoundhousEOptions WithUserName(string userName)
        {
            _userName = userName;
            return this;
        }

        public RoundhousEOptions WithPassword(string password)
        {
            _password = password;
            return this;
        }

        //public RoundhousEOptions RestoreDatabaseBeforeDeployment(bool restore)
        //{
        //    throw new NotImplementedException();
        //}

        //public RoundhousEOptions RestoreDatabaseFrom(string path)
        //{
        //    throw new NotImplementedException();
        //}

        //public RoundhousEOptions WithRestoreOptions(string options)
        //{
        //    throw new NotImplementedException();
        //}

        public override void RegisterRealTasks(PhysicalServer site)
        {
            var instanceServer = site.Name;
            if (!string.IsNullOrEmpty(_instanceName))
                instanceServer = @"{0}\{1}".FormatWith(instanceServer, _instanceName);

            var connectionString = BuildConnectionString(instanceServer, _databaseName, _userName, _password);

            var task = new RoundhousETask(connectionString,_scriptsLocation, _environmentName,  _drop,
                                          _useSimpleRecoveryMode);

            site.AddTask(task);
        }

        public string BuildConnectionString(string instanceServer, string databaseName, string userName, string password)
        {
            return "data source={0};initial catalog={1};{2}".FormatWith(instanceServer, databaseName, string.IsNullOrEmpty(userName) ? "integrated security=sspi;" : "user id={0};password={1};".FormatWith(userName, password));
        }
    }
}

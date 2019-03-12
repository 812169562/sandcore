﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

#if FULL || BATCH_DELETE || BATCH_UPDATE || QUERY_CACHE
#if EFCORE
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.ExpressionVisitors.TreeEvaluation;
using Remotion.Linq.Parsing.Structure;

namespace Sand.EntityFramework.EFCore
{
    internal static partial class InternalExtensions
    {
        public static IRelationalCommand CreateCommand<T>(this IQueryable<T> source, out RelationalQueryContext queryContext)
        {
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = compilerField.GetValue(source.Provider);

            // REFLECTION: Query.Provider.NodeTypeProvider (Use property for nullable logic)
            var nodeTypeProviderField = compiler.GetType().GetProperty("NodeTypeProvider", BindingFlags.NonPublic | BindingFlags.Instance);
            var nodeTypeProvider = nodeTypeProviderField.GetValue(compiler);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = (IQueryContextFactory)queryContextFactoryField.GetValue(compiler);

            queryContext = (RelationalQueryContext)queryContextFactory.Create();

            var evalutableExpressionFilterField = compiler.GetType().GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Static);

            var evalutableExpressionFilter = (IEvaluatableExpressionFilter)evalutableExpressionFilterField.GetValue(null);
            var databaseField = compiler.GetType().GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance);
            var database = (IDatabase)databaseField.GetValue(compiler);

            // REFLECTION: Query.Provider._queryCompiler
            var queryCompilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryCompiler = queryCompilerField.GetValue(source.Provider);

            // REFLECTION: Query.Provider._queryCompiler._evaluatableExpressionFilter
            var evaluatableExpressionFilterField = queryCompiler.GetType().GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Static);
            var evaluatableExpressionFilter = (IEvaluatableExpressionFilter)evaluatableExpressionFilterField.GetValue(null);

            Expression newQuery;
            IQueryCompilationContextFactory queryCompilationContextFactory;

            var dependenciesProperty = typeof(Database).GetProperty("Dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
            if(dependenciesProperty != null)
            {
                var dependencies = dependenciesProperty.GetValue(database);

                var queryCompilationContextFactoryField = typeof(DbContext).GetTypeFromAssembly_Core("Microsoft.EntityFrameworkCore.Storage.DatabaseDependencies")
                                                                           .GetProperty("QueryCompilationContextFactory", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                queryCompilationContextFactory = (IQueryCompilationContextFactory)queryCompilationContextFactoryField.GetValue(dependencies);

                var dependenciesProperty2 = typeof(QueryCompilationContextFactory).GetProperty("Dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
                var dependencies2 = dependenciesProperty2.GetValue(queryCompilationContextFactory);

                // REFLECTION: Query.Provider._queryCompiler._database._queryCompilationContextFactory.Logger
                var loggerField =  typeof(DbContext).GetTypeFromAssembly_Core("Microsoft.EntityFrameworkCore.Query.Internal.QueryCompilationContextDependencies")
                                                    .GetProperty("Logger", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var logger = loggerField.GetValue(dependencies2);

                var parameterExtractingExpressionVisitorConstructor = typeof(ParameterExtractingExpressionVisitor).GetConstructors().First(x => x.GetParameters().Length == 5);

                var parameterExtractingExpressionVisitor = (ParameterExtractingExpressionVisitor)parameterExtractingExpressionVisitorConstructor.Invoke(new object[] {evaluatableExpressionFilter, queryContext, logger, true, false} );
            
                // CREATE new query from query visitor
                newQuery = parameterExtractingExpressionVisitor.ExtractParameters(source.Expression);
            }
            else
            {
                // REFLECTION: Query.Provider._queryCompiler._database._queryCompilationContextFactory
                var queryCompilationContextFactoryField = typeof(Database).GetField("_queryCompilationContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
                queryCompilationContextFactory = (IQueryCompilationContextFactory)queryCompilationContextFactoryField.GetValue(database);

                // REFLECTION: Query.Provider._queryCompiler._database._queryCompilationContextFactory.Logger
                var loggerField = queryCompilationContextFactory.GetType().GetProperty("Logger", BindingFlags.NonPublic | BindingFlags.Instance);
                var logger = loggerField.GetValue(queryCompilationContextFactory);

                // CREATE new query from query visitor
                var extractParametersMethods = typeof(ParameterExtractingExpressionVisitor).GetMethod("ExtractParameters", BindingFlags.Public | BindingFlags.Static);
                newQuery = (Expression) extractParametersMethods.Invoke(null, new object[] {source.Expression, queryContext, evaluatableExpressionFilter, logger});
            }

            //var query = new QueryAnnotatingExpressionVisitor().Visit(source.Expression);
            //var newQuery = ParameterExtractingExpressionVisitor.ExtractParameters(query, queryContext, evalutableExpressionFilter);

            var queryParserMethod = compiler.GetType().GetMethod("CreateQueryParser", BindingFlags.NonPublic | BindingFlags.Static);
            var queryparser = (QueryParser)queryParserMethod.Invoke(null, new [] { nodeTypeProvider });
            var queryModel = queryparser.GetParsedQuery(newQuery);



            var queryModelVisitor = (RelationalQueryModelVisitor)queryCompilationContextFactory.Create(false).CreateQueryModelVisitor();
            var executor = queryModelVisitor.CreateQueryExecutor<T>(queryModel);

            var queries = queryModelVisitor.Queries;
            var sqlQuery = queries.ToList()[0];

            var command = sqlQuery.CreateDefaultQuerySqlGenerator().GenerateSql(queryContext.ParameterValues);

            return command;
        }

        public static IRelationalCommand CreateCommand(this IQueryable source, out RelationalQueryContext queryContext)
        {
            bool EFCore_2_1 = false;

            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = compilerField.GetValue(source.Provider);

            // REFLECTION: Query.Provider.NodeTypeProvider (Use property for nullable logic)
            var nodeTypeProviderProperty = compiler.GetType().GetProperty("NodeTypeProvider", BindingFlags.NonPublic | BindingFlags.Instance);

            object nodeTypeProvider;
            object QueryModelGenerator = null;

            if (nodeTypeProviderProperty == null)
            {
                EFCore_2_1 = true;

                var QueryModelGeneratorField = compiler.GetType().GetField("_queryModelGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
                QueryModelGenerator = QueryModelGeneratorField.GetValue(compiler);

                var nodeTypeProviderField = QueryModelGenerator.GetType().GetField("_nodeTypeProvider", BindingFlags.NonPublic | BindingFlags.Instance);
                nodeTypeProvider = nodeTypeProviderField.GetValue(QueryModelGenerator);
            }
            else
            {
                nodeTypeProvider = nodeTypeProviderProperty.GetValue(compiler);
            } 

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = (IQueryContextFactory)queryContextFactoryField.GetValue(compiler);

            queryContext = (RelationalQueryContext)queryContextFactory.Create();

            var databaseField = compiler.GetType().GetField("_database", BindingFlags.NonPublic | BindingFlags.Instance);
            var database = (IDatabase)databaseField.GetValue(compiler);

            // REFLECTION: Query.Provider._queryCompiler
            var queryCompilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryCompiler = queryCompilerField.GetValue(source.Provider);

            // REFLECTION: Query.Provider._queryCompiler._evaluatableExpressionFilter
#if NETSTANDARD2_0
           IEvaluatableExpressionFilter evaluatableExpressionFilter = null;

            if (EFCore_2_1)
            {
                evaluatableExpressionFilter = (IEvaluatableExpressionFilter)QueryModelGenerator.GetType().GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(QueryModelGenerator);
            }
            else
            {
                evaluatableExpressionFilter = (IEvaluatableExpressionFilter)compiler.GetType().GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(queryCompiler);
            }
#else
            var evalutableExpressionFilterField = compiler.GetType().GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Static);
            var evalutableExpressionFilter = (IEvaluatableExpressionFilter)evalutableExpressionFilterField.GetValue(null);

            var evaluatableExpressionFilterField = queryCompiler.GetType().GetField("_evaluatableExpressionFilter", BindingFlags.NonPublic | BindingFlags.Static);
            var evaluatableExpressionFilter = (IEvaluatableExpressionFilter)evaluatableExpressionFilterField.GetValue(null);
#endif

            Expression newQuery;
            IQueryCompilationContextFactory queryCompilationContextFactory;

            var dependenciesProperty = typeof(Database).GetProperty("Dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
            if(dependenciesProperty != null)
            {
                var dependencies = dependenciesProperty.GetValue(database);

                var queryCompilationContextFactoryField = typeof(DbContext).GetTypeFromAssembly_Core("Microsoft.EntityFrameworkCore.Storage.DatabaseDependencies")
                                                                           .GetProperty("QueryCompilationContextFactory", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                queryCompilationContextFactory = (IQueryCompilationContextFactory)queryCompilationContextFactoryField.GetValue(dependencies);

                var dependenciesProperty2 = typeof(QueryCompilationContextFactory).GetProperty("Dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
                var dependencies2 = dependenciesProperty2.GetValue(queryCompilationContextFactory);

                // REFLECTION: Query.Provider._queryCompiler._database._queryCompilationContextFactory.Logger
                var loggerField =  typeof(DbContext).GetTypeFromAssembly_Core("Microsoft.EntityFrameworkCore.Query.Internal.QueryCompilationContextDependencies")
                                                    .GetProperty("Logger", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var logger = loggerField.GetValue(dependencies2);

                var parameterExtractingExpressionVisitorConstructors = typeof(ParameterExtractingExpressionVisitor).GetConstructors();

                if (parameterExtractingExpressionVisitorConstructors.Any(x => x.GetParameters().Length == 5))
                {
                    // EF Core 2.1
                    var parameterExtractingExpressionVisitorConstructor = parameterExtractingExpressionVisitorConstructors.First(x => x.GetParameters().Length == 5);
                    var parameterExtractingExpressionVisitor = (ParameterExtractingExpressionVisitor)parameterExtractingExpressionVisitorConstructor.Invoke(new object[] { evaluatableExpressionFilter, queryContext, logger, true, false });

                    // CREATE new query from query visitor
                    newQuery = parameterExtractingExpressionVisitor.ExtractParameters(source.Expression);
                }
                else
                {
                    // EF Core 2.1 Preview 2. 
                    var parameterExtractingExpressionVisitorConstructor = parameterExtractingExpressionVisitorConstructors.First(x => x.GetParameters().Length == 6);
                    var _context = queryContext.GetType().GetProperty("Context", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    var context = _context.GetValue(queryContext);

	                ParameterExtractingExpressionVisitor parameterExtractingExpressionVisitor = null; 

					if (parameterExtractingExpressionVisitorConstructor.GetParameters().Where(x => x.ParameterType == typeof(DbContext)).Any())
	                {
		                parameterExtractingExpressionVisitor = (ParameterExtractingExpressionVisitor)parameterExtractingExpressionVisitorConstructor.Invoke(new object[] { evaluatableExpressionFilter, queryContext, logger, context, true, false });
					}
	                else
	                {
						parameterExtractingExpressionVisitor = (ParameterExtractingExpressionVisitor)parameterExtractingExpressionVisitorConstructor.Invoke(new object[] { evaluatableExpressionFilter, queryContext, logger, null, true, false });
					} 

                    // CREATE new query from query visitor
                    newQuery = parameterExtractingExpressionVisitor.ExtractParameters(source.Expression);
                }  
            }
            else
            {
                // REFLECTION: Query.Provider._queryCompiler._database._queryCompilationContextFactory
                var queryCompilationContextFactoryField = typeof(Database).GetField("_queryCompilationContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
                queryCompilationContextFactory = (IQueryCompilationContextFactory)queryCompilationContextFactoryField.GetValue(database);

                // REFLECTION: Query.Provider._queryCompiler._database._queryCompilationContextFactory.Logger
                var loggerField = queryCompilationContextFactory.GetType().GetProperty("Logger", BindingFlags.NonPublic | BindingFlags.Instance);
                var logger = loggerField.GetValue(queryCompilationContextFactory);

                // CREATE new query from query visitor
                var extractParametersMethods = typeof(ParameterExtractingExpressionVisitor).GetMethod("ExtractParameters", BindingFlags.Public | BindingFlags.Static);
                newQuery = (Expression) extractParametersMethods.Invoke(null, new object[] {source.Expression, queryContext, evaluatableExpressionFilter, logger});
                //ParameterExtractingExpressionVisitor.ExtractParameters(source.Expression, queryContext, evaluatableExpressionFilter, logger);
            }

            //var query = new QueryAnnotatingExpressionVisitor().Visit(source.Expression);
            //var newQuery = ParameterExtractingExpressionVisitor.ExtractParameters(query, queryContext, evalutableExpressionFilter);

#if NETSTANDARD2_0 
            QueryParser queryparser = null;
            if (EFCore_2_1)
            {
                var queryParserMethod = QueryModelGenerator.GetType().GetMethod("CreateQueryParser", BindingFlags.NonPublic | BindingFlags.Instance);
                queryparser = (QueryParser)queryParserMethod.Invoke(QueryModelGenerator, new[] { nodeTypeProvider });
            }
            else
            {
                var queryParserMethod = compiler.GetType().GetMethod("CreateQueryParser", BindingFlags.NonPublic | BindingFlags.Instance);
                queryparser = (QueryParser)queryParserMethod.Invoke(compiler, new[] { nodeTypeProvider });
            }
#else
            var queryParserMethod = compiler.GetType().GetMethod("CreateQueryParser", BindingFlags.NonPublic | BindingFlags.Static);
            var queryparser = (QueryParser)queryParserMethod.Invoke(null, new[] { nodeTypeProvider });
#endif
            var queryModel = queryparser.GetParsedQuery(newQuery);

            var queryModelVisitor = (RelationalQueryModelVisitor)queryCompilationContextFactory.Create(false).CreateQueryModelVisitor();
            var createQueryExecutorMethod = queryModelVisitor.GetType().GetMethod("CreateQueryExecutor");
            var createQueryExecutorMethodGeneric = createQueryExecutorMethod.MakeGenericMethod(source.ElementType);
            createQueryExecutorMethodGeneric.Invoke(queryModelVisitor, new[] { queryModel });

            var queries = queryModelVisitor.Queries;
            var sqlQuery = queries.ToList()[0];


            var command = sqlQuery.CreateDefaultQuerySqlGenerator().GenerateSql(queryContext.ParameterValues);

            return command;
        }
    }
}

#endif
#endif
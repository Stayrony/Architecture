using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Model.Invoke;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Architecture.Model.Database.Extensions;

namespace Architecture.Web.OperationFilters
{
    public class AddEnumOperationFilter : IOperationFilter
    {
        private readonly List<string> _oldDefinitionList;

        public AddEnumOperationFilter()
        {
            _oldDefinitionList = new List<string>();
        }

        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            if (context.SchemaRegistry.Definitions.Count == 4)
                _oldDefinitionList.Clear();

            operation.Description += context.SchemaRegistry.Definitions
                                           .Where(x => !_oldDefinitionList.Contains(x.Key))
                                           .SelectMany(x => x.Value.Properties)
                                           .Where(ps => ps.Value.Enum != null && ps.Value.Enum.Any())
                                           .Where(x => x.Value.Enum.First().GetType() != typeof(ResultCode))
                                           .Select(x => new EnumDescriber
                                           {
                                               Name = x.Key,
                                               Info = EnumInfo(x.Key, x.Value.Enum),
                                           })
                                           .Concat(operation.Parameters
                                                            .OfType<NonBodyParameter>()
                                                            .Where(x => x.Enum != null && x.Enum.Any())
                                                            .Select(nonBodyParameter => new EnumDescriber
                                                            {
                                                                Name = nonBodyParameter.Name,
                                                                Info = EnumInfo(nonBodyParameter.Name, nonBodyParameter.Enum)
                                                            }))
                                           .GroupBy(x => x.Name.ToLower())
                                           .Select(x => x.First())
                                           .Select(x => x.Info)
                                           .JoinStr(string.Empty);

            _oldDefinitionList.AddRange(context.SchemaRegistry.Definitions.Select(x => x.Key));
        }

        private static string EnumToString(IList<object> enumList)
        {
            if (enumList == null) throw new ArgumentNullException(nameof(enumList));

            return enumList.Select(PairEnum).JoinStr();
        }

        private static string EnumInfo(string enumName, IList<object> enumList) => $"\n{enumName}: {EnumToString(enumList)}\n";

        private static string PairEnum(object enumObj) => $"{enumObj}({EnumObjectToInt(enumObj)})";

        private static int EnumObjectToInt(object enumObj) => (int)Enum.Parse(enumObj.GetType(), enumObj.ToString());

        private class EnumDescriber
        {
            public string Name { get; set; }
            public string Info { get; set; }
        }
    }
}

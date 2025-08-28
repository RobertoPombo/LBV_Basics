using LBV_Basics.Models.Common;

namespace LBV_Basics.Models.DTOs
{
    public class FilterDtos<ModelType> where ModelType : class, IBaseModel, new()
    {
        private static readonly DtoType dtosType = DtoType.Filters;
        private dynamic dto = Activator.CreateInstance(GlobalValues.DictDtoModels[typeof(ModelType)][dtosType])!;

        public dynamic Dto
        {
            get { return dto; }
            set { if (value.GetType() == GlobalValues.DictDtoModels[typeof(ModelType)][dtosType]) { dto = value; } }
        }

        public FilterDto<ModelType> Filter { get { return new FilterDto<ModelType> { Dto = Dto.Filter }; } set { Dto.Filter = value.Dto; } }

        public FilterDto<ModelType> FilterMin { get { return new FilterDto<ModelType> { Dto = Dto.FilterMin }; } set { Dto.FilterMin = value.Dto; } }

        public FilterDto<ModelType> FilterMax { get { return new FilterDto<ModelType> { Dto = Dto.FilterMax }; } set { Dto.FilterMax = value.Dto; } }
    }
}

using LBV_Basics.Models.Common;

namespace LBV_Basics.Models.DTOs
{
    public class Mapper<ModelType> where ModelType : class, IBaseModel, new()
    {

        public ModelType Dto2Model(ModelType? obj = null) { if (obj is null) { return Scripts.Map(this, new ModelType()); } else { return Scripts.Map(this, obj); } }

        public void Model2Dto(ModelType obj) { Scripts.Map(obj, this, true); }

        public dynamic Dto2FullDto() { return Scripts.Map(this, Activator.CreateInstance(GlobalValues.DictDtoModels[typeof(ModelType)][DtoType.Full])!); }

        public static dynamic Model2FullDto(ModelType obj) { return Scripts.Map(obj, Activator.CreateInstance(GlobalValues.DictDtoModels[typeof(ModelType)][DtoType.Full])!, true); }

        public bool IsSimilar(ModelType obj, bool acceptNull = false) { return Scripts.IsSimilar(this, obj, acceptNull); }
    }
}

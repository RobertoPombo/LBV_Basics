using LBV_Basics.Models.Common;

namespace LBV_Basics.Models.DTOs
{
    public class UniqPropsDto<ModelType> where ModelType : class, IBaseModel, new()
    {
        private static readonly DtoType dtoType = DtoType.UniqProps;
        private dynamic dto = Activator.CreateInstance(GlobalValues.DictUniqPropsDtoModels[typeof(ModelType)][0])!;
        private int index = 0;

        public dynamic Dto
        {
            get { return dto; }
            set
            {
                if (value.GetType() == GlobalValues.DictUniqPropsDtoModels[typeof(ModelType)][index])
                {
                    dto = GetMappedDto(value);
                }
            }
        }

        public int Index
        {
            get { return index; }
            set
            {
                if (value < GlobalValues.DictUniqPropsDtoModels[typeof(ModelType)].Count)
                {
                    index = value;
                    dto = Activator.CreateInstance(GlobalValues.DictUniqPropsDtoModels[typeof(ModelType)][index])!;
                }
            }
        }

        private dynamic GetMappedDto(dynamic _dto)
        {
            return Scripts.Map(_dto, Activator.CreateInstance(GlobalValues.DictUniqPropsDtoModels[typeof(ModelType)][index])!, true);
        }
    }
}

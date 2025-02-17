using System;
namespace MizukiTool.RecyclePool
{
    internal class EnumIdentifier
    {
        public Type EnumType;
        public int Value;

        public void SetEnum<T>(T t) where T : Enum
        {
            EnumType = typeof(T);
            Value = Convert.ToInt32(t);
        }
        public string GetID()
        {
            return EnumType.Name + Value;
        }
    }

}

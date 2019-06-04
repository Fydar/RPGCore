using System;

namespace RPGCore.Behaviour
{
    public struct LocalId
    {
        private ulong Id;

        public LocalId(string id)
        {
            Id = Convert.ToUInt64(id);
        }

        public override string ToString()
        {
            return Convert.ToString(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
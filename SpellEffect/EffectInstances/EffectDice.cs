using System;
using System.IO;

namespace SpellEffect.EffectInstances
{
    [Serializable]
    public class EffectDice : EffectInteger
    {
        protected short m_diceface;
        protected short m_dicenum;

        public EffectDice()
        {
        }

        public EffectDice(EffectDice copy)
            : this(copy.Id, copy.Value, copy.DiceNum, copy.DiceFace, copy)
        {
            
        }

        public EffectDice(short id, short value, short dicenum, short diceface, EffectBase effect)
            : base(id, value, effect)
        {
            m_dicenum = dicenum;
            m_diceface = diceface;
        }


        public EffectDice(EffectsEnum effect, short value, short dicenum, short diceface)
            : this((short)effect,value,dicenum,diceface,new EffectBase())
        {
            
        }

        public override int ProtocoleId
        {
            get { return 73; }
        }

        public override byte SerializationIdenfitier
        {
            get { return 4; }
        }

        public short DiceNum
        {
            get { return m_dicenum; }
            set
            {
                m_dicenum = value; IsDirty = true;
            }
        }

        public short DiceFace
        {
            get { return m_diceface; }
            set
            {
                m_diceface = value; IsDirty = true;
            }
        }


        public short Min
        {
            get { var min = m_dicenum <= m_diceface ? m_dicenum : m_diceface;
                return min == 0 ? Max : min;
            }
            set
            {
                m_diceface = value;
            }
        }

        public short Max => m_dicenum >= m_diceface ? m_dicenum : m_diceface;

        public override object[] GetValues()
        {
            return new object[] {DiceNum, DiceFace, Value};
        }

        public short GetVa()
        {
            return Value;
        }



        protected override void InternalSerialize(ref BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);

            writer.Write(DiceNum);
            writer.Write(DiceFace);
        }

        protected override void InternalDeserialize(ref BinaryReader reader)
        {
            base.InternalDeserialize(ref reader);

            m_dicenum = reader.ReadInt16();
            m_diceface = reader.ReadInt16();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectDice))
                return false;
            var b = obj as EffectDice;
            return base.Equals(obj) && m_diceface == b.m_diceface && m_dicenum == b.m_dicenum;
        }

        public static bool operator ==(EffectDice a, EffectDice b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectDice a, EffectDice b)
        {
            return !(a == b);
        }

        public bool Equals(EffectDice other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_diceface == m_diceface && other.m_dicenum == m_dicenum;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = base.GetHashCode();
                result = (result*397) ^ m_diceface;
                result = (result*397) ^ m_dicenum;
                return result;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpellEffect.EffectInstances
{
    public enum EffectGenerationContext
    {
        Item,
        Spell,
    }

    public enum EffectGenerationType
    {
        Normal,
        MaxEffects,
        MinEffects,
    }

    [Serializable]
    public class EffectBase : ICloneable
    {

        int m_delay;
        int m_duration;
        int m_group;
        bool m_hidden;
        short m_id;
        int m_modificator;
        int m_random;

        string m_targetMask;
        string m_triggers;
        [NonSerialized]
        List<SpellTargetType> m_targets = new List<SpellTargetType>();
        bool m_trigger;
        uint m_zoneMinSize;
        SpellShapeEnum m_zoneShape;
        uint m_zoneSize;
        int m_zoneEfficiencyPercent;
        int m_zoneMaxEfficiency;


        public EffectBase()
        {
        }

        public EffectBase(EffectBase effect) // clone
        {
            m_id = effect.Id;
            m_targetMask = effect.TargetMask;
            m_delay = effect.Delay;
            m_duration = effect.Duration;
            m_group = effect.Group;
            m_random = effect.Random;
            m_modificator = effect.Modificator;
            m_triggers = effect.Triggers;
            m_hidden = effect.Hidden;
            m_zoneSize = effect.m_zoneSize;
            m_zoneMinSize = effect.m_zoneMinSize;
            m_zoneShape = effect.ZoneShape;
            m_zoneMaxEfficiency = effect.ZoneMaxEfficiency;
            m_zoneEfficiencyPercent = effect.ZoneEfficiencyPercent;
        }

        public EffectBase(short id, EffectBase effect)
             : this(effect)
        {
            Id = id;
        }

        public virtual int ProtocoleId => 76;

        public virtual byte SerializationIdenfitier => 1;

        public short Id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                IsDirty = true;
            }
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
            set
            {
                Id = (short) value;
                IsDirty = true;
            }
        }


        public string TargetMask
        {
            get { return m_targetMask; }
            set { m_targetMask = value; IsDirty = true; }
        }

        public List<SpellTargetType> Targets
        {
            get { return m_targets; }
            set
            {
                m_targets = value;
                IsDirty = true;
            }
        }
        public int Duration
        {
            get { return m_duration; }
            set
            {
                m_duration = value;
                IsDirty = true;
            }
        }

        public int Delay
        {
            get { return m_delay; }
            set
            {
                m_delay = value;
                IsDirty = true;
            }
        }

        public int Random
        {
            get { return m_random; }
            set
            {
                m_random = value;
                IsDirty = true;
            }
        }

        public int Group
        {
            get { return m_group; }
            set
            {
                m_group = value;
                IsDirty = true;
            }
        }

        public int Modificator
        {
            get { return m_modificator; }
            set
            {
                m_modificator = value;
                IsDirty = true;
            }
        }

        public string Triggers
        {
            get { return m_triggers; }
            set
            {
                m_triggers = value;
                IsDirty = true;
            }
        }

        public bool Trigger
        {
            get { return m_trigger; }
            set
            {
                m_trigger = value;
                IsDirty = true;
            }
        }
        public bool Hidden
        {
            get { return m_hidden; }
            set
            {
                m_hidden = value;
                IsDirty = true;
            }
        }

        public uint ZoneSize
        {
            get { return m_zoneSize >= 63 ? (byte) 63 : (byte) m_zoneSize; }
            set
            {
                m_zoneSize = value;
                IsDirty = true;
            }
        }

        public SpellShapeEnum ZoneShape
        {
            get { return m_zoneShape; }
            set
            {
                m_zoneShape = value;
                IsDirty = true;
            }
        }

        public uint ZoneMinSize
        {
            get { return m_zoneMinSize >= 63 ? (byte) 63 : (byte) m_zoneMinSize; }
            set
            {
                m_zoneMinSize = value;
                IsDirty = true;
            }
        }

        public int ZoneEfficiencyPercent
        {
            get { return m_zoneEfficiencyPercent; }
            set
            {
                m_zoneEfficiencyPercent = value;
                IsDirty = true;
            }
        }

        public int ZoneMaxEfficiency
        {
            get { return m_zoneMaxEfficiency; }
            set
            {
                m_zoneMaxEfficiency = value;
                IsDirty = true;
            }
        }

        public bool IsDirty
        {
            get;
            set;
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        static readonly Dictionary<char, SpellTargetType> m_targetsMapping = new Dictionary<char, SpellTargetType>
        {
            {'C', SpellTargetType.SELF_ONLY},
            {'c', SpellTargetType.SELF},

            {'s', SpellTargetType.ALLY_MONSTER_SUMMON},
            {'j', SpellTargetType.ALLY_SUMMON},
            {'i', SpellTargetType.ALLY_NON_MONSTER_SUMMON},
            {'d', SpellTargetType.ALLY_COMPANION},
            {'m', SpellTargetType.ALLY_MONSTER},
            {'h', SpellTargetType.ALLY_SUMMONER},
            {'l', SpellTargetType.ALLY_PLAYER},

            {'a', SpellTargetType.ALLY_ALL},
            {'g', SpellTargetType.ALLY_ALL_EXCEPT_SELF},

            {'S', SpellTargetType.ENEMY_MONSTER_SUMMON},
            {'J', SpellTargetType.ENEMY_SUMMON},
            {'I', SpellTargetType.ENEMY_NON_MONSTER_SUMMON},
            {'D', SpellTargetType.ENEMY_COMPANION},
            {'M', SpellTargetType.ENEMY_MONSTER},
            {'H', SpellTargetType.ENEMY_UNKN_1},
            {'L', SpellTargetType.ENEMY_PLAYER},

            {'A', SpellTargetType.ENEMY_ALL},
        };
        public static SpellTargetType ParseCriterion(string str)
        {

                var caster = str[0] == '*';

                if (caster)
                    str = str.Remove(0, 1);

            if (m_targetsMapping.ContainsKey(str[0]))
            {
                return m_targetsMapping[str[0]];
            }
            else
                return SpellTargetType.NONE;
            
        }
        protected void ParseTargets()
        {
            if (string.IsNullOrEmpty(m_targetMask) || m_targetMask == "a,A" || m_targetMask == "A,a")
            {
                m_targets = new List<SpellTargetType>();
                return; // default target = ALL
            }

            var data = m_targetMask.Split(',');

            m_targets = data.Select(ParseCriterion).ToList();
        }
        protected void ParseRawZone(string rawZone)
        {
            if (string.IsNullOrEmpty(rawZone))
            {
                m_zoneShape = 0;
                m_zoneSize = 0;
                m_zoneMinSize = 0;
                return;
            }

            var shape = (SpellShapeEnum) rawZone[0];
            byte size = 0;
            byte minSize = 0;
            int zoneEfficiency = 0;
            int zoneMaxEfficiency = 0;

            var data = rawZone.Remove(0, 1).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var hasMinSize = shape == SpellShapeEnum.C || shape == SpellShapeEnum.X || shape == SpellShapeEnum.Q || shape == SpellShapeEnum.plus || shape == SpellShapeEnum.sharp;


            try
            {
                if (data.Length >= 4)
                {
                    size = byte.Parse(data[0]);
                    minSize = byte.Parse(data[1]);
                    zoneEfficiency = byte.Parse(data[2]);
                    zoneMaxEfficiency = byte.Parse(data[2]);
                }
                else
                {
                    if (data.Length >= 1)
                        size = byte.Parse(data[0]);

                    if (data.Length >= 2)
                        if (hasMinSize)
                            minSize = byte.Parse(data[1]);
                        else
                            zoneEfficiency = byte.Parse(data[1]);

                    if (data.Length >= 3)
                        if (hasMinSize)
                            zoneEfficiency = byte.Parse(data[2]);
                        else
                            zoneMaxEfficiency = byte.Parse(data[2]);
                }
            }
            catch (Exception ex)
            {
                m_zoneShape = 0;
                m_zoneSize = 0;
                m_zoneMinSize = 0;

                throw new Exception("ParseRawZone() => Cannot parse " + rawZone);
            }

            m_zoneShape = shape;
            m_zoneSize = size;
            m_zoneMinSize = minSize;
            m_zoneEfficiencyPercent = zoneEfficiency;
            m_zoneMaxEfficiency = zoneMaxEfficiency;
        }

        protected string BuildRawZone()
        {
            var builder = new StringBuilder();

            builder.Append((char) (int) ZoneShape);
            builder.Append(ZoneSize);

            var hasMinSize = ZoneShape == SpellShapeEnum.C || ZoneShape == SpellShapeEnum.X || 
                ZoneShape == SpellShapeEnum.Q || ZoneShape == SpellShapeEnum.plus || ZoneShape == SpellShapeEnum.sharp;

            if (hasMinSize)
            {
                if (ZoneMinSize <= 0)
                    return builder.ToString();

                builder.Append(",");
                builder.Append(ZoneMinSize);

                if (ZoneEfficiencyPercent > 0)
                {
                    builder.Append(",");
                    builder.Append(ZoneEfficiencyPercent);

                    if (ZoneMaxEfficiency > 0)
                    {
                        builder.Append(",");
                        builder.Append(ZoneEfficiencyPercent);
                    }
                }
            }
            else
            {
                if (ZoneMinSize <= 0)
                    return builder.ToString();

                builder.Append(",");
                builder.Append(ZoneEfficiencyPercent);

                if (ZoneMaxEfficiency > 0)
                {
                    builder.Append(",");
                    builder.Append(ZoneMaxEfficiency);
                }
            }

            return builder.ToString();
        }

        public virtual object[] GetValues()
        {
            return new object[0];
        }

        public virtual EffectBase GenerateEffect(EffectGenerationContext context,
                                                 EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectBase(this);
        }

        public byte[] Serialize()
        {
            var writer = new BinaryWriter(new MemoryStream());

            writer.Write(SerializationIdenfitier);

            InternalSerialize(ref writer);

            return ((MemoryStream) writer.BaseStream).ToArray();
        }

        protected virtual void InternalSerialize(ref BinaryWriter writer)
        {
            if (string.IsNullOrEmpty(TargetMask) &&
                Duration == 0 &&
                Delay == 0 &&
                Random == 0 &&
                Group == 0 &&
                Modificator == 0 &&
                Trigger == false &&
                Hidden == false &&
                ZoneSize == 0 &&
                ZoneShape == 0)
            {
                writer.Write('C'); // cutted object

                writer.Write(Id);
            }
            else
            {
                writer.Write('F'); // full
                writer.Write(TargetMask);
                writer.Write(Id); // writer id second 'cause targets can't equals to 'C' but id can
                writer.Write(Duration);
                writer.Write(Delay);
                writer.Write(Random);
                writer.Write(Group);
                writer.Write(Modificator);
                writer.Write(Trigger);
                writer.Write(Triggers);
                writer.Write(Hidden);

                string rawZone = BuildRawZone();
                if (rawZone == null)
                    writer.Write(string.Empty);
                else
                    writer.Write(rawZone);
            }
        }

        /// <summary>
        /// Use EffectManager.Deserialize
        /// </summary>
        internal void DeSerialize(byte[] buffer, ref int index)
        {
            var reader = new BinaryReader(new MemoryStream(buffer, index, buffer.Length - index));

            InternalDeserialize(ref reader);

            index += (int) reader.BaseStream.Position;
        }

        protected virtual void InternalDeserialize(ref BinaryReader reader)
        {
            var header = reader.ReadChar();
            if (header == 'C')
            {
                m_id = reader.ReadInt16();
            }
            else if (header == 'F')
            {
                TargetMask = reader.ReadString();
                m_id = reader.ReadInt16();
                m_duration = reader.ReadInt32();
                m_delay = reader.ReadInt32();
                m_random = reader.ReadInt32();
                m_group = reader.ReadInt32();
                m_modificator = reader.ReadInt32();
                m_trigger = reader.ReadBoolean();
                m_triggers = reader.ReadString();
                m_hidden = reader.ReadBoolean();
                ParseRawZone(reader.ReadString());
            }
            else
            {
                throw new Exception($"Wrong header : {header}");
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EffectBase)) return false;
            return Equals((EffectBase) obj);
        }

        public static bool operator ==(EffectBase left, EffectBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectBase left, EffectBase right)
        {
            return !Equals(left, right);
        }

        public bool Equals(EffectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
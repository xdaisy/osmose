using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]
public class CharacterStat {

    public float BaseValue;
    protected float lastBaseValue = float.MinValue;

    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    protected readonly List<StatModifier> statModifiers;
    

    protected bool isDirty = true;
    protected float _value;

    public CharacterStat() {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public CharacterStat(float baseValue): this() {
        BaseValue = baseValue;
    }

    public virtual float Value {
        get {
            if (isDirty || lastBaseValue != BaseValue) {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    public virtual void AddModifier(StatModifier mod) {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public virtual bool RemoveModifier(StatModifier mod) {
        if (statModifiers.Remove(mod)) {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source) {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--) {
            if (statModifiers[i].Source == source) {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b) {
        if (a.Order < b.Order) {
            return -1;
        } else if (a.Order > b.Order) {
            return 1;
        }
        return 0; // if a.Order == b.Order
    }

    protected virtual float CalculateFinalValue() {
        float finalValue = BaseValue;
        float sumPercentAdd = 0; // hold sum of percent add modifiers

        for (int i = 0; i < statModifiers.Count; i++) {
            StatModifier mod = statModifiers[i];
            
            if (mod.Type == StatModType.Flat) {
                finalValue += mod.Value;
            } else if (mod.Type == StatModType.PercentAdd) {
                sumPercentAdd += mod.Value;

                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd) {
                    finalValue *= 1 + sumPercentAdd; // apply the sum percent add
                    sumPercentAdd = 0; // reset back to 0
                }
            } else if (mod.Type == StatModType.PercentMult) {
                finalValue *= 1 + mod.Value;
            }
        }
        // Rounding gets around dumb float calculation errors (like getting 12.0001f, instead of 12f)
        // 4 significant digits is usually precise enough, but feel free to change this to fit your needs
        return (float)Math.Round(finalValue, 4);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerStat : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] private string _name;
        public string Name { get => _name; set { SetField(ref _name, value); } }

        [SerializeField] private float _value;
        public float Value { get => _value; set { SetField(ref _value, value); } }
    }
}
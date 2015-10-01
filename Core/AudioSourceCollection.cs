using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class AudioSourceCollection : ICollection<AudioSource>
    {
        private List<AudioSource> AudioSources { get; set; }

        public AudioSource this[int index]
        {
            get { return AudioSources[index]; }
        }

        public AudioSourceCollection()
        {
            AudioSources = new List<AudioSource>();
        }
        public virtual void Add(AudioSource item)
        {
            AudioSources.Add(item);
        }

        public virtual void Clear()
        {
            AudioSources.Clear();
        }

        public virtual bool Contains(AudioSource item)
        {
            return AudioSources.Contains(item);
        }

        public virtual void CopyTo(AudioSource[] array, int arrayIndex)
        {
            AudioSources.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return AudioSources.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(AudioSource item)
        {
            return AudioSources.Remove(item);
        }

        public virtual IEnumerator<AudioSource> GetEnumerator()
        {
            return AudioSources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return AudioSources.GetEnumerator();
        }
    }
}

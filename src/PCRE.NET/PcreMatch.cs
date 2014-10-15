﻿using System;
using PCRE.Wrapper;

namespace PCRE
{
    public sealed class PcreMatch
    {
        private readonly PcreRegex _regex;
        private readonly string _subject;
        private readonly MatchOffsets _offsets;
        private readonly PcreGroup[] _groups;

        internal PcreMatch(PcreRegex regex, string subject, MatchOffsets offsets)
        {
            _regex = regex;
            _subject = subject;
            _offsets = offsets;
            _groups = new PcreGroup[_regex.CaptureCount + 1];
        }

        public int CaptureCount
        {
            get { return _regex.CaptureCount; }
        }

        public PcreGroup this[int index]
        {
            get
            {
                if (index < 0 || index > CaptureCount)
                    throw new ArgumentOutOfRangeException("index", "Group index out of range");

                var group = _groups[index];
                if (group == null)
                {
                    var startOffset = _offsets._offsets[2*index];

                    if (startOffset >= 0)
                    {
                        var endOffset = _offsets._offsets[2*index + 1];
                        group = new PcreGroup(_subject, startOffset, endOffset);
                    }
                    else
                    {
                        group = PcreGroup.Empty;
                    }

                    _groups[index] = group;
                }

                return group;
            }
        }

        public int Index
        {
            get { return this[0].Index; }
        }

        public int Length
        {
            get { return this[0].Length; }
        }

        public string Value
        {
            get { return this[0]; }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
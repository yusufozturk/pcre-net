﻿using System;
using PCRE.Internal;

namespace PCRE
{
    public sealed class PcreRegexSettings
    {
        private PcreOptions _options;
        private PcreNewLine? _newLine;
        private PcreBackslashR? _backslashR;
        private uint? _parensLimit;
        private uint? _maxPatternLength;
        private PcreExtraCompileOptions _extraCompileOptions;
        private PcreJitCompileOptions _jitCompileOptions;

        public PcreOptions Options
        {
            get => _options;
            set
            {
                EnsureIsMutable();
                _options = value;
            }
        }

        public PcreNewLine NewLine
        {
            get => _newLine ?? PcreBuildInfo.NewLine;
            set
            {
                EnsureIsMutable();
                _newLine = value;
            }
        }

        public PcreBackslashR BackslashR
        {
            get => _backslashR ?? PcreBuildInfo.BackslashR;
            set
            {
                EnsureIsMutable();
                _backslashR = value;
            }
        }

        public uint ParensLimit
        {
            get => _parensLimit ?? PcreBuildInfo.ParensLimit;
            set
            {
                EnsureIsMutable();
                _parensLimit = value;
            }
        }

        public uint? MaxPatternLength
        {
            get => _maxPatternLength;
            set
            {
                EnsureIsMutable();
                _maxPatternLength = value;
            }
        }

        public PcreExtraCompileOptions ExtraCompileOptions
        {
            get => _extraCompileOptions;
            set
            {
                EnsureIsMutable();
                _extraCompileOptions = value;
            }
        }

        public PcreJitCompileOptions JitCompileOptions
        {
            get => _jitCompileOptions | Options.ToJitCompileOptions();
            set
            {
                EnsureIsMutable();
                _jitCompileOptions = value;
            }
        }

        internal bool ReadOnlySettings { get; }

        public PcreRegexSettings()
        {
        }

        internal PcreRegexSettings(PcreOptions options)
        {
            _options = options;
        }

        private PcreRegexSettings(PcreRegexSettings settings, bool readOnly)
        {
            _options = settings._options;
            _newLine = settings._newLine;
            _backslashR = settings._backslashR;
            _parensLimit = settings._parensLimit;
            _maxPatternLength = settings._maxPatternLength;
            _extraCompileOptions = settings._extraCompileOptions;
            _jitCompileOptions= settings._jitCompileOptions;

            ReadOnlySettings = readOnly;
        }

        internal bool CompareValues(PcreRegexSettings other)
        {
            return Options == other.Options
                   && NewLine == other.NewLine
                   && BackslashR == other.BackslashR
                   && ParensLimit == other.ParensLimit
                   && MaxPatternLength == other.MaxPatternLength
                   && ExtraCompileOptions == other.ExtraCompileOptions
                   && JitCompileOptions == other.JitCompileOptions;
        }

        internal PcreRegexSettings AsReadOnly()
        {
            if (ReadOnlySettings)
                return this;

            return new PcreRegexSettings(this, true);
        }

        private void EnsureIsMutable()
        {
            if (ReadOnlySettings)
                throw new InvalidOperationException("Settings of a compiled pattern cannot be changed");
        }

        internal void FillCompileInput(ref Native.compile_input input)
        {
            input.flags = Options.ToPatternOptions();
            input.flags_jit = (uint)JitCompileOptions;
            input.new_line = (uint)_newLine.GetValueOrDefault();
            input.bsr = (uint)_backslashR.GetValueOrDefault();
            input.parens_nest_limit = _parensLimit.GetValueOrDefault();
            input.max_pattern_length = _maxPatternLength.GetValueOrDefault();
            input.compile_extra_options = (uint)_extraCompileOptions;
        }
    }
}

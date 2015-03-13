
#pragma once

#include "CalloutData.h"
#include "MatchResultCode.h"

using namespace System;
using namespace System::Diagnostics::Contracts;

namespace PCRE {
	namespace Wrapper {

		ref class InternalRegex;

		public ref class MatchData sealed
		{
		public:
			MatchData(InternalRegex^ const re, String^ subject);
			MatchData(InternalRegex^ const re, String^ subject, uint32_t oVectorSize);
			~MatchData();
			!MatchData();

			property int RawResultCode {
				public: int get() { return _rawResultCode; }
				internal: void set(int value) { _rawResultCode = value; }
			}

			property MatchResultCode ResultCode {
				public: MatchResultCode get() { return _resultCode; }
				internal: void set(MatchResultCode value) { _resultCode = value; }
			}

			property InternalRegex^ Regex { InternalRegex^ get() { return _re; } }
			property String^ Subject { String^ get() { return _subject; } }
			property String^ Mark { String^ get(); }

			property uint32_t OutputVectorLength { uint32_t get() { return static_cast<uint32_t>(_oVectorCount); } }

			[Pure]
			int GetStartOffset(unsigned int index);

			[Pure]
			int GetEndOffset(unsigned int index);

		internal:
			MatchData(MatchData^ result, pcre2_callout_block *calloutBlock);

			property Exception^ CalloutException;

			property pcre2_match_data* Block {
				pcre2_match_data* get() { return _matchData; }
			}

		private:
			initonly InternalRegex^ _re;
			initonly String^ _subject;
			int _rawResultCode;
			MatchResultCode _resultCode;
			String^ _mark;
			PCRE2_SPTR _markPtr;
			pcre2_match_data* _matchData;
			PCRE2_SIZE* _oVector;
			PCRE2_SIZE _oVectorCount;
		};
	}
}

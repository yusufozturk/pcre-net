
#pragma once

#include "JitCompileOptions.h"
#include "CompileContext.h"
#include "MatchContext.h"

using namespace System;
using namespace System::Collections::Generic;

namespace PCRE {
	namespace Wrapper {

		enum struct PatternOptions : unsigned int;
		enum struct InfoKey;

		public ref class InternalRegex sealed
		{
		public:
			InternalRegex(CompileContext^ context);
			~InternalRegex();
			!InternalRegex();

			MatchData^ Match(MatchContext^ context);
			MatchData^ DfaMatch(MatchContext^ context);

			int GetInfoInt32(InfoKey key);

			property int CaptureCount {
				int get() { return _captureCount; }
			}

			property Dictionary<String^, array<int>^>^ CaptureNames {
				Dictionary<String^, array<int>^>^ get() { return _captureNames; }
			}

		internal:
			property pcre2_code* Code {
				pcre2_code* get() { return _re; }
			}

		private:
			pcre2_code* _re;
			initonly int _captureCount;
			initonly Dictionary<String^, array<int>^>^ _captureNames;
		};
	}
}

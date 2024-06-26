// CliFPSystem.h

#pragma once

#include "..\..\..\LibProj\CsFilePack\stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CliFPSystem {

	public ref class FPSUtil
	{
		// ÆÄÀÏ ÆÑÅ·
	public:
		bool	DoPacking(System::String^ fileName, List<System::String^>^ fileList);

		// ÆÄÀÏ ¾ðÆÑÅ·
		bool	DoUnPacking(System::String^ fileName);
	};
}

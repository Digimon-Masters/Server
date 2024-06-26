// 기본 DLL 파일입니다.

#include "stdafx.h"
#include <msclr\marshal_cppstd.h>
#include "CliFPSystem.h"

namespace CliFPSystem {

// 	void MarshalString(String ^ s, std::string& os) {
// 		using namespace Runtime::InteropServices;
// 		const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
// 		os = chars;
// 		Marshal::FreeHGlobal(IntPtr((void*)chars));
// 	}
// 
// 	void MarshalString(String ^ s, std::wstring& os) {
// 		using namespace Runtime::InteropServices;
// 		const wchar_t* chars = (const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
// 		os = chars;
// 		Marshal::FreeHGlobal(IntPtr((void*)chars));
// 	}

	bool FPSUtil::DoPacking(System::String^ fileName, List<System::String^>^ fileList)
	{
		std::string packFile = msclr::interop::marshal_as<std::string>(fileName);

		if (!CsFPS::CsFPSystem::Initialize(true, packFile.c_str(), true))
			return false;
		
		Int32 fileSize = fileList->Count;
		CsFPS::CsFPSystem::SetAllocCount(fileSize);

		for (Int32 n = 0; n < fileList->Count; ++n)
		{
			std::string packFile = msclr::interop::marshal_as<std::string>(fileList[n]);
			if (!CsFPS::CsFPSystem::AddFile(packFile.c_str(), packFile.c_str()))
			{
				CsFPS::CsFPSystem::Destroy();
				return false;
			}
		}

		CsFPS::CsFPSystem::SaveHashPack();
		CsFPS::CsFPSystem::Destroy();
		return true;
	}

	bool FPSUtil::DoUnPacking(System::String^ fileName)
	{
		//std::string packFile;
		//MarshalString(fileName, packFile);
		std::string packFile = msclr::interop::marshal_as<std::string>(fileName);
		
		if (!CsFPS::CsFPSystem::Initialize(true, packFile.c_str(), false))
			return false;

		// 패킹 파일 언팩킹
		CsFPS::CsFPSystem::UnPacking();
		CsFPS::CsFPSystem::Destroy();
		return true;
	}

}
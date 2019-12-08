#pragma once

#include "winrt/Microsoft.ReactNative.Bridge.h"



using namespace winrt::Microsoft::ReactNative::Bridge;

namespace winrt::ArtefactUi::implementation
{

    struct ReactPackageProvider : winrt::implements<ReactPackageProvider, IReactPackageProvider>
    {
    public: // IReactPackageProvider
        void CreatePackage(IReactPackageBuilder const &packageBuilder) noexcept;
    };

} // namespace winrt::ArtefactUi::implementation



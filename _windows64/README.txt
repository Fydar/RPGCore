Chromium Embedded Framework (CEF) Client Binary Distribution for Windows
-------------------------------------------------------------------------------

Date:             May 05, 2018

CEF Version:      3.3359.1772.gd1df190
CEF URL:          https://bitbucket.org/chromiumembedded/cef.git
                  @d1df1907f31e80d65e0316dfd82cab53511dbafe

Chromium Version: 66.0.3359.117
Chromium URL:     https://chromium.googlesource.com/chromium/src.git
                  @1634af79e3bf0d6f0178a5fdb8829b059ead4b26

This distribution contains a release build of the cefclient sample application
for the Windows platform. Please see the LICENSING section of this document for
licensing terms and conditions.


CONTENTS
--------

Release     Contains a release build of the sample application.


USAGE
-----

Please visit the CEF Website for additional usage information.

https://bitbucket.org/chromiumembedded/cef/


REDISTRIBUTION
--------------

This binary distribution contains the below components.

Required components:

The following components are required. CEF will not function without them.

* CEF core library.
  * libcef.dll

* Crash reporting library.
  * chrome_elf.dll

* Unicode support data.
  * icudtl.dat

* V8 snapshot data.
  * natives_blob.bin
  * snapshot_blob.bin
  * v8_context_snapshot.bin

Optional components:

The following components are optional. If they are missing CEF will continue to
run but any related functionality may become broken or disabled.

* Localized resources.
  Locale file loading can be disabled completely using
  CefSettings.pack_loading_disabled. The locales directory path can be
  customized using CefSettings.locales_dir_path. 
 
  * locales/
    Directory containing localized resources used by CEF, Chromium and Blink. A
    .pak file is loaded from this directory based on the CefSettings.locale
    value. Only configured locales need to be distributed. If no locale is
    configured the default locale of "en-US" will be used. Without these files
    arbitrary Web components may display incorrectly.

* Other resources.
  Pack file loading can be disabled completely using
  CefSettings.pack_loading_disabled. The resources directory path can be
  customized using CefSettings.resources_dir_path.

  * cef.pak
  * cef_100_percent.pak
  * cef_200_percent.pak
    These files contain non-localized resources used by CEF, Chromium and Blink.
    Without these files arbitrary Web components may display incorrectly.

  * cef_extensions.pak
    This file contains non-localized resources required for extension loading.
    Pass the `--disable-extensions` command-line flag to disable use of this
    file. Without this file components that depend on the extension system,
    such as the PDF viewer, will not function.

  * devtools_resources.pak
    This file contains non-localized resources required for Chrome Developer
    Tools. Without this file Chrome Developer Tools will not function.

* Angle and Direct3D support.
  * d3dcompiler_43.dll (required for Windows XP)
  * d3dcompiler_47.dll (required for Windows Vista and newer)
  * libEGL.dll
  * libGLESv2.dll
  Without these files HTML5 accelerated content like 2D canvas, 3D CSS and WebGL
  will not function.

* SwiftShader support.
  * swiftshader/libEGL.dll
  * swiftshader/libGLESv2.dll
  Without these files WebGL will not function in software-only mode when the GPU
  is not available or disabled.

* Widevine CDM support.
  * widevinecdmadapter.dll
    Without this file playback of Widevine projected content will not function.
    See the CefRegisterWidevineCdm() function in cef_web_plugin.h for usage.


LICENSING
---------

The CEF project is BSD licensed. Please read the LICENSE.txt file included with
this binary distribution for licensing terms and conditions. Other software
included in this distribution is provided under other licenses. Please visit
"about:credits" in a CEF-based application for complete Chromium and third-party
licensing information.

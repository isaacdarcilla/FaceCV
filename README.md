# Face Recognition Computer Vision
Real time  multi face recognition software using OpenCV and EmguCV

## What is **Emgu CV**?

Emgu CV is a cross platform .Net wrapper to the OpenCV image processing library. Allowing OpenCV functions to be called from .NET compatible languages such as C#, VB, VC++, IronPython etc. Emgu CV is written entirely in C#. The benefit is that it can be compiled in Mono and therefore is able to run on any platform Mono supports, including iOS, Android, Windows Phone, Mac OS X and Linux. A lot of efforts has been spent to have a pure C# implementation since the headers have to be ported, compared with managed C++ implementation where header files can simply be included. (Source: www.emgu.com)

Emgu CV has two layers of wrapper as shown below.

1. The basic layer (layer 1) contains function, structure and enumeration mappings which directly reflect those in OpenCV.
2. The second layer (layer 2) contains classes that mix in advantanges from the .NET world.

![EmguCV](http://www.emgu.com/wiki/images/EmguCVArchitecture.png)

Its is essentially a huge library of “wrapper” functions that allows calling OpenCV functions from a Visual Studio Windows Forms application. It is necessary because Visual Studio/.NET is an “interpreted” environment that cannot directly call functions written in native C/C++. (Source: [CodeProject](]https://www.codeproject.com/articles/528275/starting-with-emgu-cv))

## What is **Open CV**?

OpenCV (Open Source Computer Vision Library) is an open source computer vision and machine learning software library. OpenCV was built to provide a common infrastructure for computer vision applications and to accelerate the use of machine perception in the commercial products. The library has more than 2500 optimized algorithms, which includes a comprehensive set of both classic and state-of-the-art computer vision and machine learning algorithms. These algorithms can be used to detect and recognize faces, identify objects, classify human actions in videos, track camera movements, track moving objects, extract 3D models of objects, produce 3D point clouds from stereo cameras, stitch images together to produce a high resolution image of an entire scene, find similar images from an image database, remove red eyes from images taken using flash, follow eye movements, recognize scenery and establish markers to overlay it with augmented reality, etc. (Source: https://opencv.org)

![OpenCV](https://www.researchgate.net/profile/Aleksandra_Krolak/publication/252458755/figure/fig1/AS:298195692343297@1448106801398/The-basic-structure-of-the-OpenCV-library.png)

Open CV License [BSD License](https://en.wikipedia.org/wiki/BSD_license) & Emgu CV License [GNU Public License](http://www.gnu.org/licenses/gpl-3.0.txt)

### Contributions are welcome!


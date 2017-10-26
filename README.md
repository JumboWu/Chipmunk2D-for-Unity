# Chipmunk2D-for-Unity
Chipmunk2D is a powerful 2D physics engine,  now we can use it with Unity.

[原创](http://www.jianshu.com/p/26d2935b133a) http://www.jianshu.com/p/26d2935b133a
##背景

* 1、Unity开发一款对物理要求比较高的2D游戏，采用帧同步方案

* 2、需要对物理可控，而Unity内置的2D物理引擎，没有源码，综合考虑采用[Chipmunk2D](http://chipmunk-physics.net/)这款开源的2D物理引擎
* 3、官方之前有过一个Chipmunk2D Unity版的开发工作安排，然后找了很久只是看到一个演示Demo的视频，并没有提供源码插件。之后联系了Chipmunk2D 引擎的作者和主要的开发者，回复大致内容:"Chipmunk2D的Unity版本，他们不再继续开发，因为Unity新的版本对2D物理已经很好的支持了，要实现可控的物理，可以自己集成"
* 4、Chipmunk2D官网也没有提供C#语言版本的API

##解决方案
综上背景约束，只能自己实现Chipmunk2D 对Unity支持，同时需要编写编辑器支持

##示例
我们先来看看Demo的运行效果

![Chipmunk2D Demo](http://upload-images.jianshu.io/upload_images/191918-de5f9dbc7ab674c6.gif?imageMogr2/auto-orient/strip)

[Demo源码](https://github.com/JumpWu/Chipmunk2D-for-Unity)
[原创](http://www.jianshu.com/p/26d2935b133a) http://www.jianshu.com/p/26d2935b133a

##应用
* 1、使用前需要有官方Chipmunk2D相关基础，官方上面有C语言的Demo，可以跑一跑运行下，先做个了解
* 2、Chipmunk2D的Unity版本插件使用Unity 5.6.2f1基于Chipmunk2D 7.0开发，与官方C语言版本API保持一致，便于应用
* 3、添加了刚体和碰撞器的编辑器支持

![刚体和碰撞器](http://upload-images.jianshu.io/upload_images/191918-8422a98f3917f47a.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


[Demo源码](https://github.com/JumpWu/Chipmunk2D-for-Unity)
[原创](http://www.jianshu.com/p/26d2935b133a) http://www.jianshu.com/p/26d2935b133a

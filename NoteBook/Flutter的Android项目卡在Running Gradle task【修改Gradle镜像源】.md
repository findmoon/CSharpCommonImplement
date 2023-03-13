**Flutter的Android项目卡在Running Gradle task 'assembleDebug'...【修改Gradle镜像源】**

[toc]

> 参考自 [flutter卡在Running Gradle task 'assembleDebug'...](https://www.cnblogs.com/lovewhatIlove/p/16323828.html)

# 问题

启动运行Flutter的Android项目时，会发现一直卡在Gradle的构建过程中：`Running Gradle task 'assembleDebug'...`。

原因在于，Gradle构建打包时需要从外网一些资源，如果没有科学上网，可能会导致构建一直卡住，没法继续执行，最后提示无法运行（运行失败）。

比较好的解决办法是 **切换国内的镜像源**，防止后续仍然可能的发生。

# Flutter SDK 中 切换阿里镜像源

> 修改两个文件中、三个地方的镜像源

- 1. 在 Flutter SDK 文件夹中，找到`packages\flutter_tools\gradle\flutter.gradle`文件。

比如，我的路径是`C:\devsoft\flutter\packages\flutter_tools\gradle\flutter.gradle`，修改如下内容：

```dart
    repositories {
        // google()
        // mavenCentral()
        maven { url 'https://maven.aliyun.com/repository/google' }
        maven { url 'https://maven.aliyun.com/repository/jcenter' }
        maven { url 'https://maven.aliyun.com/nexus/content/groups/public' }
    }

    // ....
    // ....

    class FlutterPlugin implements Plugin<Project> {
        // private static final String DEFAULT_MAVEN_HOST = "https://storage.googleapis.com";
        private static final String DEFAULT_MAVEN_HOST = "https://storage.flutter-io.cn";
        
        // .....
    }
```

- 2. 同样，在 Flutter SDK 文件夹中，找到`packages\flutter_tools\gradle\resolve_dependencies.gradle`文件。

比如，我的路径是`C:\devsoft\flutter\packages\flutter_tools\gradle\resolve_dependencies.gradle`，修改如下内容：

```dart
repositories {
    // google()
    // mavenCentral()
    maven { url 'https://maven.aliyun.com/repository/google' }
    maven { url 'https://maven.aliyun.com/repository/jcenter' }
    maven { url 'https://maven.aliyun.com/nexus/content/groups/public' }
    maven {
        // url "$storageUrl/download.flutter.io"
        url "https://storage.flutter-io.cn/download.flutter.io"
    }
}
```

# 当前 # Flutter 项目中切换阿里源

项目中的 `android\build.gradle` 文件。

```dart
buildscript {
    ext.kotlin_version = '1.6.10'
    repositories {
//        google()
//        mavenCentral()
        maven { url 'https://maven.aliyun.com/repository/google' }
        maven { url 'https://maven.aliyun.com/repository/jcenter' }
        maven { url 'https://maven.aliyun.com/nexus/content/groups/public' }
    }

...

allprojects {
    repositories {
//        google()
//        mavenCentral()
        maven { url 'https://maven.aliyun.com/repository/google' }
        maven { url 'https://maven.aliyun.com/repository/jcenter' }
        maven { url 'https://maven.aliyun.com/nexus/content/groups/public' }
    }
}
```

> 注意使用 https。

# flutter upgrade 报错 local changes

更新时提示报错如下：

Your flutter checkout has local changes that would be erased by upgrading. If you want to keep these changes, it is
recommended that you stash them via "git stash" or else commit the changes to a local branch. If it is okay to remove local
changes, then re-run this command with "--force".

原因是为了解决 Gradle 构建时的问题，修改 Gradle 的源为国内镜像。某些(2个)文件被修改了，`--force`强制更新后再修改回镜像源即可。

# 另：也可以参考已经可以成功构建的项目修改Gradle配置解决问题

具体可参见 [Flutter卡在Running ‘gradle assembleDebug‘最完整解决](https://blog.csdn.net/qq_43596067/article/details/107710915)


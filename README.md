![intro](intro.png)

而且得是局域网联机的，因为哥们想玩

# 安装

先装依赖：Window -> Package Manager -> Input System
再装Mirror：https://assetstore.unity.com/packages/tools/network/mirror-129321 导入路径：/P1项目文件夹/Assets/

激活Input System：Edit > Project Settings > Player > Active Input Handling

# 疑难杂症

报错缺少com.unity.feature.2d包：Window -> Package Manager用Unity Registry搜一下2d这个包，装好之后**重启**一下Unity

# 项目多开

1. 修改本项目根目录下的make_link.bat的CURRENT_DIR为本项目绝对路径，TARGET_DIR为一个与本项目路径不同的比较方便的路径
2. 管理员身份运行make_link.bat
3. UnityHub从磁盘添加TARGET_DIR的工程，然后启动Editor
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

# 资产来源

Assets/AIGC/ 下的资产为使用AI做游戏资产生成的一些尝试，生成参数可以从png的meta信息中找到。这些内容在资源可以稳定使用后我们补上模型来源。

map_torii:
```
Game Icon Institute, Game Scene, (extremely detailed CG unity 8k wallpaper),(masterpiece:1.4), (best quality:1.3), (ultra-detailed), (best illustration),(best shadow), (an extremely delicate and beautiful:1.2), finely detail, eastern building structure, 2D, sunrise background, multiple platform, Torii, landscaoe, wide_shot, cartoon, sketch, face to camera, ukiya style
Negative prompt: (easynegative:0.8),black, dark,Low resolution
Steps: 60, Sampler: DPM++ SDE Karras, CFG scale: 7, Seed: 1806341205, Size: 2048x576, Model hash: 879db523c3, Denoising strength: 0.75, Clip skip: 2, Version: v1.5.1
```

map_jungle:
```
Game Icon Institute, Game Scene, (extremely detailed CG unity 8k wallpaper),(masterpiece:1.4), (best quality:1.3), (ultra-detailed), (best illustration),(best shadow), (an extremely delicate and beautiful:1.2), finely detail, south eastern building structure, 2D, dusk background, multiple platform, bamboo jungle, plenty of shrub and bush, landscaoe, wide_shot, cartoon, sketch, face to camera, Stilt house, There is a river over the town
Negative prompt: (easynegative:0.8),black, dark,Low resolution, water marked, text
Steps: 40, Sampler: DPM++ 2M SDE Karras, CFG scale: 7, Seed: 1210484339, Size: 2048x576, Model hash: 879db523c3, Denoising strength: 0.66, Clip skip: 2, Version: v1.5.1
```

map_sea:
```
Game Icon Institute, Game Scene, (extremely detailed CG unity 8k wallpaper),(masterpiece:1.4), (best quality:1.3), (ultra-detailed), (best illustration),(best shadow), (an extremely delicate and beautiful:1.2), finely detail, western building structure, 2D, midnight background, multiple platform, dock, moonlignt, landscaoe, wide_shot, cartoon, sketch, face to camera, seascape house, there is a ocean over the platforms,starry night sky, aquarium
Negative prompt: (easynegative:0.8),black, dark,Low resolution, water marked, text
Steps: 40, Sampler: DPM++ 2M SDE Karras, CFG scale: 7, Seed: 3246154361, Size: 2048x576, Model hash: 879db523c3, Denoising strength: 0.66, Clip skip: 2, Version: v1.5.1
```
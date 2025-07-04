var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i3098 = root || request.c( 'UnityEngine.JointSpring' )
  var i3099 = data
  i3098.spring = i3099[0]
  i3098.damper = i3099[1]
  i3098.targetPosition = i3099[2]
  return i3098
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i3100 = root || request.c( 'UnityEngine.JointMotor' )
  var i3101 = data
  i3100.m_TargetVelocity = i3101[0]
  i3100.m_Force = i3101[1]
  i3100.m_FreeSpin = i3101[2]
  return i3100
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i3102 = root || request.c( 'UnityEngine.JointLimits' )
  var i3103 = data
  i3102.m_Min = i3103[0]
  i3102.m_Max = i3103[1]
  i3102.m_Bounciness = i3103[2]
  i3102.m_BounceMinVelocity = i3103[3]
  i3102.m_ContactDistance = i3103[4]
  i3102.minBounce = i3103[5]
  i3102.maxBounce = i3103[6]
  return i3102
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i3104 = root || request.c( 'UnityEngine.JointDrive' )
  var i3105 = data
  i3104.m_PositionSpring = i3105[0]
  i3104.m_PositionDamper = i3105[1]
  i3104.m_MaximumForce = i3105[2]
  i3104.m_UseAcceleration = i3105[3]
  return i3104
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i3106 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i3107 = data
  i3106.m_Spring = i3107[0]
  i3106.m_Damper = i3107[1]
  return i3106
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i3108 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i3109 = data
  i3108.m_Limit = i3109[0]
  i3108.m_Bounciness = i3109[1]
  i3108.m_ContactDistance = i3109[2]
  return i3108
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i3110 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i3111 = data
  i3110.m_ExtremumSlip = i3111[0]
  i3110.m_ExtremumValue = i3111[1]
  i3110.m_AsymptoteSlip = i3111[2]
  i3110.m_AsymptoteValue = i3111[3]
  i3110.m_Stiffness = i3111[4]
  return i3110
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i3112 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i3113 = data
  i3112.m_LowerAngle = i3113[0]
  i3112.m_UpperAngle = i3113[1]
  return i3112
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i3114 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i3115 = data
  i3114.m_MotorSpeed = i3115[0]
  i3114.m_MaximumMotorTorque = i3115[1]
  return i3114
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i3116 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i3117 = data
  i3116.m_DampingRatio = i3117[0]
  i3116.m_Frequency = i3117[1]
  i3116.m_Angle = i3117[2]
  return i3116
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i3118 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i3119 = data
  i3118.m_LowerTranslation = i3119[0]
  i3118.m_UpperTranslation = i3119[1]
  return i3118
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i3120 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i3121 = data
  i3120.name = i3121[0]
  i3120.width = i3121[1]
  i3120.height = i3121[2]
  i3120.mipmapCount = i3121[3]
  i3120.anisoLevel = i3121[4]
  i3120.filterMode = i3121[5]
  i3120.hdr = !!i3121[6]
  i3120.format = i3121[7]
  i3120.wrapMode = i3121[8]
  i3120.alphaIsTransparency = !!i3121[9]
  i3120.alphaSource = i3121[10]
  i3120.graphicsFormat = i3121[11]
  i3120.sRGBTexture = !!i3121[12]
  i3120.desiredColorSpace = i3121[13]
  i3120.wrapU = i3121[14]
  i3120.wrapV = i3121[15]
  return i3120
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh"] = function (request, data, root) {
  var i3122 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh' )
  var i3123 = data
  i3122.name = i3123[0]
  i3122.halfPrecision = !!i3123[1]
  i3122.useUInt32IndexFormat = !!i3123[2]
  i3122.vertexCount = i3123[3]
  i3122.aabb = i3123[4]
  var i3125 = i3123[5]
  var i3124 = []
  for(var i = 0; i < i3125.length; i += 1) {
    i3124.push( !!i3125[i + 0] );
  }
  i3122.streams = i3124
  i3122.vertices = i3123[6]
  var i3127 = i3123[7]
  var i3126 = []
  for(var i = 0; i < i3127.length; i += 1) {
    i3126.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh', i3127[i + 0]) );
  }
  i3122.subMeshes = i3126
  var i3129 = i3123[8]
  var i3128 = []
  for(var i = 0; i < i3129.length; i += 16) {
    i3128.push( new pc.Mat4().setData(i3129[i + 0], i3129[i + 1], i3129[i + 2], i3129[i + 3],  i3129[i + 4], i3129[i + 5], i3129[i + 6], i3129[i + 7],  i3129[i + 8], i3129[i + 9], i3129[i + 10], i3129[i + 11],  i3129[i + 12], i3129[i + 13], i3129[i + 14], i3129[i + 15]) );
  }
  i3122.bindposes = i3128
  var i3131 = i3123[9]
  var i3130 = []
  for(var i = 0; i < i3131.length; i += 1) {
    i3130.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape', i3131[i + 0]) );
  }
  i3122.blendShapes = i3130
  return i3122
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh"] = function (request, data, root) {
  var i3136 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh' )
  var i3137 = data
  i3136.triangles = i3137[0]
  return i3136
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape"] = function (request, data, root) {
  var i3142 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape' )
  var i3143 = data
  i3142.name = i3143[0]
  var i3145 = i3143[1]
  var i3144 = []
  for(var i = 0; i < i3145.length; i += 1) {
    i3144.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame', i3145[i + 0]) );
  }
  i3142.frames = i3144
  return i3142
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i3146 = root || new pc.UnityMaterial()
  var i3147 = data
  i3146.name = i3147[0]
  request.r(i3147[1], i3147[2], 0, i3146, 'shader')
  i3146.renderQueue = i3147[3]
  i3146.enableInstancing = !!i3147[4]
  var i3149 = i3147[5]
  var i3148 = []
  for(var i = 0; i < i3149.length; i += 1) {
    i3148.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i3149[i + 0]) );
  }
  i3146.floatParameters = i3148
  var i3151 = i3147[6]
  var i3150 = []
  for(var i = 0; i < i3151.length; i += 1) {
    i3150.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i3151[i + 0]) );
  }
  i3146.colorParameters = i3150
  var i3153 = i3147[7]
  var i3152 = []
  for(var i = 0; i < i3153.length; i += 1) {
    i3152.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i3153[i + 0]) );
  }
  i3146.vectorParameters = i3152
  var i3155 = i3147[8]
  var i3154 = []
  for(var i = 0; i < i3155.length; i += 1) {
    i3154.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i3155[i + 0]) );
  }
  i3146.textureParameters = i3154
  var i3157 = i3147[9]
  var i3156 = []
  for(var i = 0; i < i3157.length; i += 1) {
    i3156.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i3157[i + 0]) );
  }
  i3146.materialFlags = i3156
  return i3146
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i3160 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i3161 = data
  i3160.name = i3161[0]
  i3160.value = i3161[1]
  return i3160
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i3164 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i3165 = data
  i3164.name = i3165[0]
  i3164.value = new pc.Color(i3165[1], i3165[2], i3165[3], i3165[4])
  return i3164
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i3168 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i3169 = data
  i3168.name = i3169[0]
  i3168.value = new pc.Vec4( i3169[1], i3169[2], i3169[3], i3169[4] )
  return i3168
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i3172 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i3173 = data
  i3172.name = i3173[0]
  request.r(i3173[1], i3173[2], 0, i3172, 'value')
  return i3172
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i3176 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i3177 = data
  i3176.name = i3177[0]
  i3176.enabled = !!i3177[1]
  return i3176
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Transform"] = function (request, data, root) {
  var i3178 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Transform' )
  var i3179 = data
  i3178.position = new pc.Vec3( i3179[0], i3179[1], i3179[2] )
  i3178.scale = new pc.Vec3( i3179[3], i3179[4], i3179[5] )
  i3178.rotation = new pc.Quat(i3179[6], i3179[7], i3179[8], i3179[9])
  return i3178
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshRenderer"] = function (request, data, root) {
  var i3180 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshRenderer' )
  var i3181 = data
  request.r(i3181[0], i3181[1], 0, i3180, 'additionalVertexStreams')
  i3180.enabled = !!i3181[2]
  request.r(i3181[3], i3181[4], 0, i3180, 'sharedMaterial')
  var i3183 = i3181[5]
  var i3182 = []
  for(var i = 0; i < i3183.length; i += 2) {
  request.r(i3183[i + 0], i3183[i + 1], 2, i3182, '')
  }
  i3180.sharedMaterials = i3182
  i3180.receiveShadows = !!i3181[6]
  i3180.shadowCastingMode = i3181[7]
  i3180.sortingLayerID = i3181[8]
  i3180.sortingOrder = i3181[9]
  i3180.lightmapIndex = i3181[10]
  i3180.lightmapSceneIndex = i3181[11]
  i3180.lightmapScaleOffset = new pc.Vec4( i3181[12], i3181[13], i3181[14], i3181[15] )
  i3180.lightProbeUsage = i3181[16]
  i3180.reflectionProbeUsage = i3181[17]
  return i3180
}

Deserializers["QueueTargetControl"] = function (request, data, root) {
  var i3186 = root || request.c( 'QueueTargetControl' )
  var i3187 = data
  return i3186
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshFilter"] = function (request, data, root) {
  var i3188 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshFilter' )
  var i3189 = data
  request.r(i3189[0], i3189[1], 0, i3188, 'sharedMesh')
  return i3188
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i3190 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i3191 = data
  i3190.name = i3191[0]
  i3190.tagId = i3191[1]
  i3190.enabled = !!i3191[2]
  i3190.isStatic = !!i3191[3]
  i3190.layer = i3191[4]
  return i3190
}

Deserializers["YarnWoolAnimation"] = function (request, data, root) {
  var i3192 = root || request.c( 'YarnWoolAnimation' )
  var i3193 = data
  request.r(i3193[0], i3193[1], 0, i3192, 'WoolAnimationData')
  request.r(i3193[2], i3193[3], 0, i3192, 'LineRenderer')
  return i3192
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.LineRenderer"] = function (request, data, root) {
  var i3194 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.LineRenderer' )
  var i3195 = data
  i3194.textureMode = i3195[0]
  i3194.alignment = i3195[1]
  i3194.widthCurve = new pc.AnimationCurve( { keys_flow: i3195[2] } )
  i3194.colorGradient = i3195[3] ? new pc.ColorGradient(i3195[3][0], i3195[3][1], i3195[3][2]) : null
  var i3197 = i3195[4]
  var i3196 = []
  for(var i = 0; i < i3197.length; i += 3) {
    i3196.push( new pc.Vec3( i3197[i + 0], i3197[i + 1], i3197[i + 2] ) );
  }
  i3194.positions = i3196
  i3194.positionCount = i3195[5]
  i3194.widthMultiplier = i3195[6]
  i3194.startWidth = i3195[7]
  i3194.endWidth = i3195[8]
  i3194.numCornerVertices = i3195[9]
  i3194.numCapVertices = i3195[10]
  i3194.useWorldSpace = !!i3195[11]
  i3194.loop = !!i3195[12]
  i3194.startColor = new pc.Color(i3195[13], i3195[14], i3195[15], i3195[16])
  i3194.endColor = new pc.Color(i3195[17], i3195[18], i3195[19], i3195[20])
  i3194.generateLightingData = !!i3195[21]
  i3194.enabled = !!i3195[22]
  request.r(i3195[23], i3195[24], 0, i3194, 'sharedMaterial')
  var i3199 = i3195[25]
  var i3198 = []
  for(var i = 0; i < i3199.length; i += 2) {
  request.r(i3199[i + 0], i3199[i + 1], 2, i3198, '')
  }
  i3194.sharedMaterials = i3198
  i3194.receiveShadows = !!i3195[26]
  i3194.shadowCastingMode = i3195[27]
  i3194.sortingLayerID = i3195[28]
  i3194.sortingOrder = i3195[29]
  i3194.lightmapIndex = i3195[30]
  i3194.lightmapSceneIndex = i3195[31]
  i3194.lightmapScaleOffset = new pc.Vec4( i3195[32], i3195[33], i3195[34], i3195[35] )
  i3194.lightProbeUsage = i3195[36]
  i3194.reflectionProbeUsage = i3195[37]
  return i3194
}

Deserializers["RollWoolAnimation"] = function (request, data, root) {
  var i3202 = root || request.c( 'RollWoolAnimation' )
  var i3203 = data
  request.r(i3203[0], i3203[1], 0, i3202, 'woolClip1')
  request.r(i3203[2], i3203[3], 0, i3202, 'woolClip2')
  request.r(i3203[4], i3203[5], 0, i3202, 'WoolAnimationData')
  var i3205 = i3203[6]
  var i3204 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.MeshRenderer')))
  for(var i = 0; i < i3205.length; i += 2) {
  request.r(i3205[i + 0], i3205[i + 1], 1, i3204, '')
  }
  i3202.MeshRenderers = i3204
  i3202._localScale = new pc.Vec3( i3203[7], i3203[8], i3203[9] )
  return i3202
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Cubemap"] = function (request, data, root) {
  var i3208 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Cubemap' )
  var i3209 = data
  i3208.name = i3209[0]
  i3208.atlasId = i3209[1]
  i3208.mipmapCount = i3209[2]
  i3208.hdr = !!i3209[3]
  i3208.size = i3209[4]
  i3208.anisoLevel = i3209[5]
  i3208.filterMode = i3209[6]
  var i3211 = i3209[7]
  var i3210 = []
  for(var i = 0; i < i3211.length; i += 4) {
    i3210.push( UnityEngine.Rect.MinMaxRect(i3211[i + 0], i3211[i + 1], i3211[i + 2], i3211[i + 3]) );
  }
  i3208.rects = i3210
  i3208.wrapU = i3209[8]
  i3208.wrapV = i3209[9]
  return i3208
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i3214 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i3215 = data
  i3214.name = i3215[0]
  i3214.index = i3215[1]
  i3214.startup = !!i3215[2]
  return i3214
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Light"] = function (request, data, root) {
  var i3216 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Light' )
  var i3217 = data
  i3216.enabled = !!i3217[0]
  i3216.type = i3217[1]
  i3216.color = new pc.Color(i3217[2], i3217[3], i3217[4], i3217[5])
  i3216.cullingMask = i3217[6]
  i3216.intensity = i3217[7]
  i3216.range = i3217[8]
  i3216.spotAngle = i3217[9]
  i3216.shadows = i3217[10]
  i3216.shadowNormalBias = i3217[11]
  i3216.shadowBias = i3217[12]
  i3216.shadowStrength = i3217[13]
  i3216.shadowResolution = i3217[14]
  i3216.lightmapBakeType = i3217[15]
  i3216.renderMode = i3217[16]
  request.r(i3217[17], i3217[18], 0, i3216, 'cookie')
  i3216.cookieSize = i3217[19]
  return i3216
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.RectTransform"] = function (request, data, root) {
  var i3218 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.RectTransform' )
  var i3219 = data
  i3218.pivot = new pc.Vec2( i3219[0], i3219[1] )
  i3218.anchorMin = new pc.Vec2( i3219[2], i3219[3] )
  i3218.anchorMax = new pc.Vec2( i3219[4], i3219[5] )
  i3218.sizeDelta = new pc.Vec2( i3219[6], i3219[7] )
  i3218.anchoredPosition3D = new pc.Vec3( i3219[8], i3219[9], i3219[10] )
  i3218.rotation = new pc.Quat(i3219[11], i3219[12], i3219[13], i3219[14])
  i3218.scale = new pc.Vec3( i3219[15], i3219[16], i3219[17] )
  return i3218
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Canvas"] = function (request, data, root) {
  var i3220 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Canvas' )
  var i3221 = data
  i3220.enabled = !!i3221[0]
  i3220.planeDistance = i3221[1]
  i3220.referencePixelsPerUnit = i3221[2]
  i3220.isFallbackOverlay = !!i3221[3]
  i3220.renderMode = i3221[4]
  i3220.renderOrder = i3221[5]
  i3220.sortingLayerName = i3221[6]
  i3220.sortingOrder = i3221[7]
  i3220.scaleFactor = i3221[8]
  request.r(i3221[9], i3221[10], 0, i3220, 'worldCamera')
  i3220.overrideSorting = !!i3221[11]
  i3220.pixelPerfect = !!i3221[12]
  i3220.targetDisplay = i3221[13]
  i3220.overridePixelPerfect = !!i3221[14]
  return i3220
}

Deserializers["UnityEngine.UI.GraphicRaycaster"] = function (request, data, root) {
  var i3222 = root || request.c( 'UnityEngine.UI.GraphicRaycaster' )
  var i3223 = data
  i3222.m_IgnoreReversedGraphics = !!i3223[0]
  i3222.m_BlockingObjects = i3223[1]
  i3222.m_BlockingMask = UnityEngine.LayerMask.FromIntegerValue( i3223[2] )
  return i3222
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasGroup"] = function (request, data, root) {
  var i3224 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasGroup' )
  var i3225 = data
  i3224.m_Alpha = i3225[0]
  i3224.m_Interactable = !!i3225[1]
  i3224.m_BlocksRaycasts = !!i3225[2]
  i3224.m_IgnoreParentGroups = !!i3225[3]
  i3224.enabled = !!i3225[4]
  return i3224
}

Deserializers["UnityEngine.UI.CanvasScaler"] = function (request, data, root) {
  var i3226 = root || request.c( 'UnityEngine.UI.CanvasScaler' )
  var i3227 = data
  i3226.m_UiScaleMode = i3227[0]
  i3226.m_ReferencePixelsPerUnit = i3227[1]
  i3226.m_ScaleFactor = i3227[2]
  i3226.m_ReferenceResolution = new pc.Vec2( i3227[3], i3227[4] )
  i3226.m_ScreenMatchMode = i3227[5]
  i3226.m_MatchWidthOrHeight = i3227[6]
  i3226.m_PhysicalUnit = i3227[7]
  i3226.m_FallbackScreenDPI = i3227[8]
  i3226.m_DefaultSpriteDPI = i3227[9]
  i3226.m_DynamicPixelsPerUnit = i3227[10]
  i3226.m_PresetInfoIsWorld = !!i3227[11]
  return i3226
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer"] = function (request, data, root) {
  var i3228 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer' )
  var i3229 = data
  i3228.cullTransparentMesh = !!i3229[0]
  return i3228
}

Deserializers["UnityEngine.UI.Image"] = function (request, data, root) {
  var i3230 = root || request.c( 'UnityEngine.UI.Image' )
  var i3231 = data
  request.r(i3231[0], i3231[1], 0, i3230, 'm_Sprite')
  i3230.m_Type = i3231[2]
  i3230.m_PreserveAspect = !!i3231[3]
  i3230.m_FillCenter = !!i3231[4]
  i3230.m_FillMethod = i3231[5]
  i3230.m_FillAmount = i3231[6]
  i3230.m_FillClockwise = !!i3231[7]
  i3230.m_FillOrigin = i3231[8]
  i3230.m_UseSpriteMesh = !!i3231[9]
  i3230.m_PixelsPerUnitMultiplier = i3231[10]
  request.r(i3231[11], i3231[12], 0, i3230, 'm_Material')
  i3230.m_Maskable = !!i3231[13]
  i3230.m_Color = new pc.Color(i3231[14], i3231[15], i3231[16], i3231[17])
  i3230.m_RaycastTarget = !!i3231[18]
  i3230.m_RaycastPadding = new pc.Vec4( i3231[19], i3231[20], i3231[21], i3231[22] )
  return i3230
}

Deserializers["Interactable"] = function (request, data, root) {
  var i3232 = root || request.c( 'Interactable' )
  var i3233 = data
  i3232.HoldThreshold = i3233[0]
  i3232.SwipeThreshold = i3233[1]
  return i3232
}

Deserializers["UnityEngine.UI.Button"] = function (request, data, root) {
  var i3234 = root || request.c( 'UnityEngine.UI.Button' )
  var i3235 = data
  i3234.m_OnClick = request.d('UnityEngine.UI.Button+ButtonClickedEvent', i3235[0], i3234.m_OnClick)
  i3234.m_Navigation = request.d('UnityEngine.UI.Navigation', i3235[1], i3234.m_Navigation)
  i3234.m_Transition = i3235[2]
  i3234.m_Colors = request.d('UnityEngine.UI.ColorBlock', i3235[3], i3234.m_Colors)
  i3234.m_SpriteState = request.d('UnityEngine.UI.SpriteState', i3235[4], i3234.m_SpriteState)
  i3234.m_AnimationTriggers = request.d('UnityEngine.UI.AnimationTriggers', i3235[5], i3234.m_AnimationTriggers)
  i3234.m_Interactable = !!i3235[6]
  request.r(i3235[7], i3235[8], 0, i3234, 'm_TargetGraphic')
  return i3234
}

Deserializers["UnityEngine.UI.Button+ButtonClickedEvent"] = function (request, data, root) {
  var i3236 = root || request.c( 'UnityEngine.UI.Button+ButtonClickedEvent' )
  var i3237 = data
  i3236.m_PersistentCalls = request.d('UnityEngine.Events.PersistentCallGroup', i3237[0], i3236.m_PersistentCalls)
  return i3236
}

Deserializers["UnityEngine.Events.PersistentCallGroup"] = function (request, data, root) {
  var i3238 = root || request.c( 'UnityEngine.Events.PersistentCallGroup' )
  var i3239 = data
  var i3241 = i3239[0]
  var i3240 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Events.PersistentCall')))
  for(var i = 0; i < i3241.length; i += 1) {
    i3240.add(request.d('UnityEngine.Events.PersistentCall', i3241[i + 0]));
  }
  i3238.m_Calls = i3240
  return i3238
}

Deserializers["UnityEngine.Events.PersistentCall"] = function (request, data, root) {
  var i3244 = root || request.c( 'UnityEngine.Events.PersistentCall' )
  var i3245 = data
  request.r(i3245[0], i3245[1], 0, i3244, 'm_Target')
  i3244.m_TargetAssemblyTypeName = i3245[2]
  i3244.m_MethodName = i3245[3]
  i3244.m_Mode = i3245[4]
  i3244.m_Arguments = request.d('UnityEngine.Events.ArgumentCache', i3245[5], i3244.m_Arguments)
  i3244.m_CallState = i3245[6]
  return i3244
}

Deserializers["UnityEngine.UI.Navigation"] = function (request, data, root) {
  var i3246 = root || request.c( 'UnityEngine.UI.Navigation' )
  var i3247 = data
  i3246.m_Mode = i3247[0]
  i3246.m_WrapAround = !!i3247[1]
  request.r(i3247[2], i3247[3], 0, i3246, 'm_SelectOnUp')
  request.r(i3247[4], i3247[5], 0, i3246, 'm_SelectOnDown')
  request.r(i3247[6], i3247[7], 0, i3246, 'm_SelectOnLeft')
  request.r(i3247[8], i3247[9], 0, i3246, 'm_SelectOnRight')
  return i3246
}

Deserializers["UnityEngine.UI.ColorBlock"] = function (request, data, root) {
  var i3248 = root || request.c( 'UnityEngine.UI.ColorBlock' )
  var i3249 = data
  i3248.m_NormalColor = new pc.Color(i3249[0], i3249[1], i3249[2], i3249[3])
  i3248.m_HighlightedColor = new pc.Color(i3249[4], i3249[5], i3249[6], i3249[7])
  i3248.m_PressedColor = new pc.Color(i3249[8], i3249[9], i3249[10], i3249[11])
  i3248.m_SelectedColor = new pc.Color(i3249[12], i3249[13], i3249[14], i3249[15])
  i3248.m_DisabledColor = new pc.Color(i3249[16], i3249[17], i3249[18], i3249[19])
  i3248.m_ColorMultiplier = i3249[20]
  i3248.m_FadeDuration = i3249[21]
  return i3248
}

Deserializers["UnityEngine.UI.SpriteState"] = function (request, data, root) {
  var i3250 = root || request.c( 'UnityEngine.UI.SpriteState' )
  var i3251 = data
  request.r(i3251[0], i3251[1], 0, i3250, 'm_HighlightedSprite')
  request.r(i3251[2], i3251[3], 0, i3250, 'm_PressedSprite')
  request.r(i3251[4], i3251[5], 0, i3250, 'm_SelectedSprite')
  request.r(i3251[6], i3251[7], 0, i3250, 'm_DisabledSprite')
  return i3250
}

Deserializers["UnityEngine.UI.AnimationTriggers"] = function (request, data, root) {
  var i3252 = root || request.c( 'UnityEngine.UI.AnimationTriggers' )
  var i3253 = data
  i3252.m_NormalTrigger = i3253[0]
  i3252.m_HighlightedTrigger = i3253[1]
  i3252.m_PressedTrigger = i3253[2]
  i3252.m_SelectedTrigger = i3253[3]
  i3252.m_DisabledTrigger = i3253[4]
  return i3252
}

Deserializers["SoundUIElement"] = function (request, data, root) {
  var i3254 = root || request.c( 'SoundUIElement' )
  var i3255 = data
  i3254.Sound = request.d('SoundDefine', i3255[0], i3254.Sound)
  i3254.PlayOnEnable = !!i3255[1]
  i3254.StopOnDisable = !!i3255[2]
  i3254.playWithInteractable = !!i3255[3]
  i3254.isPlayRandomBackGroundMusic = !!i3255[4]
  return i3254
}

Deserializers["SoundDefine"] = function (request, data, root) {
  var i3256 = root || request.c( 'SoundDefine' )
  var i3257 = data
  i3256.soundType = i3257[0]
  i3256.Loop = !!i3257[1]
  request.r(i3257[2], i3257[3], 0, i3256, 'Clip')
  var i3259 = i3257[4]
  var i3258 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.AudioClip')))
  for(var i = 0; i < i3259.length; i += 2) {
  request.r(i3259[i + 0], i3259[i + 1], 1, i3258, '')
  }
  i3256.ClipList = i3258
  return i3256
}

Deserializers["PlayNowButtonAnim"] = function (request, data, root) {
  var i3262 = root || request.c( 'PlayNowButtonAnim' )
  var i3263 = data
  request.r(i3263[0], i3263[1], 0, i3262, 'playerNowButton')
  i3262.maxScale = new pc.Vec3( i3263[2], i3263[3], i3263[4] )
  i3262.minScale = new pc.Vec3( i3263[5], i3263[6], i3263[7] )
  i3262.scaleDuration = i3263[8]
  return i3262
}

Deserializers["UnityEngine.UI.Text"] = function (request, data, root) {
  var i3264 = root || request.c( 'UnityEngine.UI.Text' )
  var i3265 = data
  i3264.m_FontData = request.d('UnityEngine.UI.FontData', i3265[0], i3264.m_FontData)
  i3264.m_Text = i3265[1]
  request.r(i3265[2], i3265[3], 0, i3264, 'm_Material')
  i3264.m_Maskable = !!i3265[4]
  i3264.m_Color = new pc.Color(i3265[5], i3265[6], i3265[7], i3265[8])
  i3264.m_RaycastTarget = !!i3265[9]
  i3264.m_RaycastPadding = new pc.Vec4( i3265[10], i3265[11], i3265[12], i3265[13] )
  return i3264
}

Deserializers["UnityEngine.UI.FontData"] = function (request, data, root) {
  var i3266 = root || request.c( 'UnityEngine.UI.FontData' )
  var i3267 = data
  request.r(i3267[0], i3267[1], 0, i3266, 'm_Font')
  i3266.m_FontSize = i3267[2]
  i3266.m_FontStyle = i3267[3]
  i3266.m_BestFit = !!i3267[4]
  i3266.m_MinSize = i3267[5]
  i3266.m_MaxSize = i3267[6]
  i3266.m_Alignment = i3267[7]
  i3266.m_AlignByGeometry = !!i3267[8]
  i3266.m_RichText = !!i3267[9]
  i3266.m_HorizontalOverflow = i3267[10]
  i3266.m_VerticalOverflow = i3267[11]
  i3266.m_LineSpacing = i3267[12]
  return i3266
}

Deserializers["EndGameUI"] = function (request, data, root) {
  var i3268 = root || request.c( 'EndGameUI' )
  var i3269 = data
  request.r(i3269[0], i3269[1], 0, i3268, 'replayButton')
  i3268.maxScale = new pc.Vec3( i3269[2], i3269[3], i3269[4] )
  i3268.minScale = new pc.Vec3( i3269[5], i3269[6], i3269[7] )
  i3268.scaleDuration = i3269[8]
  return i3268
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.AudioSource"] = function (request, data, root) {
  var i3270 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.AudioSource' )
  var i3271 = data
  request.r(i3271[0], i3271[1], 0, i3270, 'clip')
  request.r(i3271[2], i3271[3], 0, i3270, 'outputAudioMixerGroup')
  i3270.playOnAwake = !!i3271[4]
  i3270.loop = !!i3271[5]
  i3270.time = i3271[6]
  i3270.volume = i3271[7]
  i3270.pitch = i3271[8]
  i3270.enabled = !!i3271[9]
  return i3270
}

Deserializers["CameraController"] = function (request, data, root) {
  var i3272 = root || request.c( 'CameraController' )
  var i3273 = data
  request.r(i3273[0], i3273[1], 0, i3272, 'InputInteractable')
  request.r(i3273[2], i3273[3], 0, i3272, 'ZoomCameraData')
  request.r(i3273[4], i3273[5], 0, i3272, 'BackGround')
  request.r(i3273[6], i3273[7], 0, i3272, 'SpawnPoint')
  request.r(i3273[8], i3273[9], 0, i3272, 'ModelPrefab')
  i3272.targetRotation = new pc.Quat(i3273[10], i3273[11], i3273[12], i3273[13])
  i3272.Friction = i3273[14]
  i3272.RotationSensitivity = new pc.Vec2( i3273[15], i3273[16] )
  i3272.AccelerationRange = new pc.Vec2( i3273[17], i3273[18] )
  i3272.RotationSpeed = i3273[19]
  i3272.RotationAutoSpeed = i3273[20]
  i3272.SmoothingTime = i3273[21]
  i3272.TimeAFKToAutoRotation = i3273[22]
  i3272.IntroLenght = i3273[23]
  i3272.ModelRotationIntroSpeed = i3273[24]
  i3272.IntroCameraZoomInDuration = i3273[25]
  i3272.IntroStartFOV = i3273[26]
  i3272.IntroEndFOV = i3273[27]
  i3272.DragStyle = i3273[28]
  i3272.DraggingSpeed = i3273[29]
  i3272.SmoothFactor = i3273[30]
  i3272.ZoomStyle = i3273[31]
  i3272.layerMask = UnityEngine.LayerMask.FromIntegerValue( i3273[32] )
  i3272._tapRadius = i3273[33]
  i3272._isRotateObjectInMainMenu = !!i3273[34]
  i3272._cameraPosMainMenuDefault = new pc.Vec3( i3273[35], i3273[36], i3273[37] )
  i3272._cameraRoteMainMenuDefault = new pc.Vec3( i3273[38], i3273[39], i3273[40] )
  i3272._cameraPosGamePlayDefault = new pc.Vec3( i3273[41], i3273[42], i3273[43] )
  i3272._cameraRoteGamePlayDefault = new pc.Vec3( i3273[44], i3273[45], i3273[46] )
  return i3272
}

Deserializers["GamePlaySystem"] = function (request, data, root) {
  var i3274 = root || request.c( 'GamePlaySystem' )
  var i3275 = data
  request.r(i3275[0], i3275[1], 0, i3274, 'CameraController')
  request.r(i3275[2], i3275[3], 0, i3274, 'BoxChainReaction3D')
  var i3277 = i3275[4]
  var i3276 = new (System.Collections.Generic.List$1(Bridge.ns('CubeTargetControl')))
  for(var i = 0; i < i3277.length; i += 2) {
  request.r(i3277[i + 0], i3277[i + 1], 1, i3276, '')
  }
  i3274.CurrentCubeTargets = i3276
  var i3279 = i3275[5]
  var i3278 = new (System.Collections.Generic.List$1(Bridge.ns('QueueTargetControl')))
  for(var i = 0; i < i3279.length; i += 2) {
  request.r(i3279[i + 0], i3279[i + 1], 1, i3278, '')
  }
  i3274.CurrentQueueTargets = i3278
  request.r(i3275[6], i3275[7], 0, i3274, 'YarnWoolPrefab')
  request.r(i3275[8], i3275[9], 0, i3274, 'RollWoolPrefab')
  i3274.TotalCubeActive = i3275[10]
  i3274.CubeReadyCount = i3275[11]
  request.r(i3275[12], i3275[13], 0, i3274, 'woolXoayClip')
  request.r(i3275[14], i3275[15], 0, i3274, 'wool1Clip')
  request.r(i3275[16], i3275[17], 0, i3274, 'loseSound')
  request.r(i3275[18], i3275[19], 0, i3274, 'winSound')
  i3274.IsGoToStore = !!i3275[20]
  request.r(i3275[21], i3275[22], 0, i3274, 'handController')
  i3274.cubeCountClaimed = i3275[23]
  i3274.totalCountClaimed = i3275[24]
  request.r(i3275[25], i3275[26], 0, i3274, 'endGamePanel')
  i3274._cubeTargetCountDefault = i3275[27]
  request.r(i3275[28], i3275[29], 0, i3274, '_levelPrefab')
  i3274.spacingCubeTarget = i3275[30]
  return i3274
}

Deserializers["SoundManager"] = function (request, data, root) {
  var i3284 = root || request.c( 'SoundManager' )
  var i3285 = data
  request.r(i3285[0], i3285[1], 0, i3284, 'audioMixer')
  request.r(i3285[2], i3285[3], 0, i3284, 'fxMusicSource')
  request.r(i3285[4], i3285[5], 0, i3284, 'specialBgmSource')
  i3284.BGM = request.d('SoundDefine', i3285[6], i3284.BGM)
  return i3284
}

Deserializers["CubeTargetControl"] = function (request, data, root) {
  var i3286 = root || request.c( 'CubeTargetControl' )
  var i3287 = data
  request.r(i3287[0], i3287[1], 0, i3286, 'group')
  var i3289 = i3287[2]
  var i3288 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Transform')))
  for(var i = 0; i < i3289.length; i += 2) {
  request.r(i3289[i + 0], i3289[i + 1], 1, i3288, '')
  }
  i3286.TargetChildren = i3288
  var i3291 = i3287[3]
  var i3290 = []
  for(var i = 0; i < i3291.length; i += 2) {
  request.r(i3291[i + 0], i3291[i + 1], 2, i3290, '')
  }
  i3286.MeshRenderer = i3290
  i3286.IsActive = !!i3287[4]
  i3286.RollWoolTime = i3287[5]
  i3286.DelayTime = i3287[6]
  i3286.VibrationStrength = i3287[7]
  request.r(i3287[8], i3287[9], 0, i3286, 'AddCubeIcon')
  request.r(i3287[10], i3287[11], 0, i3286, '_boxAnimation')
  i3286._boxMoveAnimation = i3287[12]
  request.r(i3287[13], i3287[14], 0, i3286, '_boxCollider')
  return i3286
}

Deserializers["TargetBoxAnimation"] = function (request, data, root) {
  var i3296 = root || request.c( 'TargetBoxAnimation' )
  var i3297 = data
  i3296._boxMoveTime = i3297[0]
  i3296._capMoveTime = i3297[1]
  i3296._hopDownTime = i3297[2]
  i3296._hopUpTime = i3297[3]
  request.r(i3297[4], i3297[5], 0, i3296, 'boxWhooshClip')
  request.r(i3297[6], i3297[7], 0, i3296, '_cap')
  request.r(i3297[8], i3297[9], 0, i3296, '_closeParticle')
  i3296._boxMoveDistance = i3297[10]
  i3296._capMoveDistance = i3297[11]
  i3296._capScaleTime = i3297[12]
  i3296._hopScale = i3297[13]
  i3296.offsetMoveY = i3297[14]
  return i3296
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.BoxCollider"] = function (request, data, root) {
  var i3298 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.BoxCollider' )
  var i3299 = data
  i3298.center = new pc.Vec3( i3299[0], i3299[1], i3299[2] )
  i3298.size = new pc.Vec3( i3299[3], i3299[4], i3299[5] )
  i3298.enabled = !!i3299[6]
  i3298.isTrigger = !!i3299[7]
  request.r(i3299[8], i3299[9], 0, i3298, 'material')
  return i3298
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.ParticleSystem"] = function (request, data, root) {
  var i3300 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.ParticleSystem' )
  var i3301 = data
  i3300.main = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule', i3301[0], i3300.main)
  i3300.colorBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule', i3301[1], i3300.colorBySpeed)
  i3300.colorOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule', i3301[2], i3300.colorOverLifetime)
  i3300.emission = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule', i3301[3], i3300.emission)
  i3300.rotationBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule', i3301[4], i3300.rotationBySpeed)
  i3300.rotationOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule', i3301[5], i3300.rotationOverLifetime)
  i3300.shape = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule', i3301[6], i3300.shape)
  i3300.sizeBySpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule', i3301[7], i3300.sizeBySpeed)
  i3300.sizeOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule', i3301[8], i3300.sizeOverLifetime)
  i3300.textureSheetAnimation = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule', i3301[9], i3300.textureSheetAnimation)
  i3300.velocityOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule', i3301[10], i3300.velocityOverLifetime)
  i3300.noise = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule', i3301[11], i3300.noise)
  i3300.inheritVelocity = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule', i3301[12], i3300.inheritVelocity)
  i3300.forceOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule', i3301[13], i3300.forceOverLifetime)
  i3300.limitVelocityOverLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule', i3301[14], i3300.limitVelocityOverLifetime)
  i3300.useAutoRandomSeed = !!i3301[15]
  i3300.randomSeed = i3301[16]
  return i3300
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule"] = function (request, data, root) {
  var i3302 = root || new pc.ParticleSystemMain()
  var i3303 = data
  i3302.duration = i3303[0]
  i3302.loop = !!i3303[1]
  i3302.prewarm = !!i3303[2]
  i3302.startDelay = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[3], i3302.startDelay)
  i3302.startLifetime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[4], i3302.startLifetime)
  i3302.startSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[5], i3302.startSpeed)
  i3302.startSize3D = !!i3303[6]
  i3302.startSizeX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[7], i3302.startSizeX)
  i3302.startSizeY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[8], i3302.startSizeY)
  i3302.startSizeZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[9], i3302.startSizeZ)
  i3302.startRotation3D = !!i3303[10]
  i3302.startRotationX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[11], i3302.startRotationX)
  i3302.startRotationY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[12], i3302.startRotationY)
  i3302.startRotationZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[13], i3302.startRotationZ)
  i3302.startColor = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i3303[14], i3302.startColor)
  i3302.gravityModifier = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3303[15], i3302.gravityModifier)
  i3302.simulationSpace = i3303[16]
  request.r(i3303[17], i3303[18], 0, i3302, 'customSimulationSpace')
  i3302.simulationSpeed = i3303[19]
  i3302.useUnscaledTime = !!i3303[20]
  i3302.scalingMode = i3303[21]
  i3302.playOnAwake = !!i3303[22]
  i3302.maxParticles = i3303[23]
  i3302.emitterVelocityMode = i3303[24]
  i3302.stopAction = i3303[25]
  return i3302
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve"] = function (request, data, root) {
  var i3304 = root || new pc.MinMaxCurve()
  var i3305 = data
  i3304.mode = i3305[0]
  i3304.curveMin = new pc.AnimationCurve( { keys_flow: i3305[1] } )
  i3304.curveMax = new pc.AnimationCurve( { keys_flow: i3305[2] } )
  i3304.curveMultiplier = i3305[3]
  i3304.constantMin = i3305[4]
  i3304.constantMax = i3305[5]
  return i3304
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient"] = function (request, data, root) {
  var i3306 = root || new pc.MinMaxGradient()
  var i3307 = data
  i3306.mode = i3307[0]
  i3306.gradientMin = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient', i3307[1], i3306.gradientMin)
  i3306.gradientMax = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient', i3307[2], i3306.gradientMax)
  i3306.colorMin = new pc.Color(i3307[3], i3307[4], i3307[5], i3307[6])
  i3306.colorMax = new pc.Color(i3307[7], i3307[8], i3307[9], i3307[10])
  return i3306
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient"] = function (request, data, root) {
  var i3308 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient' )
  var i3309 = data
  i3308.mode = i3309[0]
  var i3311 = i3309[1]
  var i3310 = []
  for(var i = 0; i < i3311.length; i += 1) {
    i3310.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey', i3311[i + 0]) );
  }
  i3308.colorKeys = i3310
  var i3313 = i3309[2]
  var i3312 = []
  for(var i = 0; i < i3313.length; i += 1) {
    i3312.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey', i3313[i + 0]) );
  }
  i3308.alphaKeys = i3312
  return i3308
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule"] = function (request, data, root) {
  var i3314 = root || new pc.ParticleSystemColorBySpeed()
  var i3315 = data
  i3314.enabled = !!i3315[0]
  i3314.color = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i3315[1], i3314.color)
  i3314.range = new pc.Vec2( i3315[2], i3315[3] )
  return i3314
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey"] = function (request, data, root) {
  var i3318 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey' )
  var i3319 = data
  i3318.color = new pc.Color(i3319[0], i3319[1], i3319[2], i3319[3])
  i3318.time = i3319[4]
  return i3318
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey"] = function (request, data, root) {
  var i3322 = root || request.c( 'Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey' )
  var i3323 = data
  i3322.alpha = i3323[0]
  i3322.time = i3323[1]
  return i3322
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule"] = function (request, data, root) {
  var i3324 = root || new pc.ParticleSystemColorOverLifetime()
  var i3325 = data
  i3324.enabled = !!i3325[0]
  i3324.color = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient', i3325[1], i3324.color)
  return i3324
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule"] = function (request, data, root) {
  var i3326 = root || new pc.ParticleSystemEmitter()
  var i3327 = data
  i3326.enabled = !!i3327[0]
  i3326.rateOverTime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3327[1], i3326.rateOverTime)
  i3326.rateOverDistance = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3327[2], i3326.rateOverDistance)
  var i3329 = i3327[3]
  var i3328 = []
  for(var i = 0; i < i3329.length; i += 1) {
    i3328.push( request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst', i3329[i + 0]) );
  }
  i3326.bursts = i3328
  return i3326
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst"] = function (request, data, root) {
  var i3332 = root || new pc.ParticleSystemBurst()
  var i3333 = data
  i3332.count = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3333[0], i3332.count)
  i3332.cycleCount = i3333[1]
  i3332.minCount = i3333[2]
  i3332.maxCount = i3333[3]
  i3332.repeatInterval = i3333[4]
  i3332.time = i3333[5]
  return i3332
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule"] = function (request, data, root) {
  var i3334 = root || new pc.ParticleSystemRotationBySpeed()
  var i3335 = data
  i3334.enabled = !!i3335[0]
  i3334.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3335[1], i3334.x)
  i3334.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3335[2], i3334.y)
  i3334.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3335[3], i3334.z)
  i3334.separateAxes = !!i3335[4]
  i3334.range = new pc.Vec2( i3335[5], i3335[6] )
  return i3334
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule"] = function (request, data, root) {
  var i3336 = root || new pc.ParticleSystemRotationOverLifetime()
  var i3337 = data
  i3336.enabled = !!i3337[0]
  i3336.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3337[1], i3336.x)
  i3336.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3337[2], i3336.y)
  i3336.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3337[3], i3336.z)
  i3336.separateAxes = !!i3337[4]
  return i3336
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule"] = function (request, data, root) {
  var i3338 = root || new pc.ParticleSystemShape()
  var i3339 = data
  i3338.enabled = !!i3339[0]
  i3338.shapeType = i3339[1]
  i3338.randomDirectionAmount = i3339[2]
  i3338.sphericalDirectionAmount = i3339[3]
  i3338.randomPositionAmount = i3339[4]
  i3338.alignToDirection = !!i3339[5]
  i3338.radius = i3339[6]
  i3338.radiusMode = i3339[7]
  i3338.radiusSpread = i3339[8]
  i3338.radiusSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3339[9], i3338.radiusSpeed)
  i3338.radiusThickness = i3339[10]
  i3338.angle = i3339[11]
  i3338.length = i3339[12]
  i3338.boxThickness = new pc.Vec3( i3339[13], i3339[14], i3339[15] )
  i3338.meshShapeType = i3339[16]
  request.r(i3339[17], i3339[18], 0, i3338, 'mesh')
  request.r(i3339[19], i3339[20], 0, i3338, 'meshRenderer')
  request.r(i3339[21], i3339[22], 0, i3338, 'skinnedMeshRenderer')
  i3338.useMeshMaterialIndex = !!i3339[23]
  i3338.meshMaterialIndex = i3339[24]
  i3338.useMeshColors = !!i3339[25]
  i3338.normalOffset = i3339[26]
  i3338.arc = i3339[27]
  i3338.arcMode = i3339[28]
  i3338.arcSpread = i3339[29]
  i3338.arcSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3339[30], i3338.arcSpeed)
  i3338.donutRadius = i3339[31]
  i3338.position = new pc.Vec3( i3339[32], i3339[33], i3339[34] )
  i3338.rotation = new pc.Vec3( i3339[35], i3339[36], i3339[37] )
  i3338.scale = new pc.Vec3( i3339[38], i3339[39], i3339[40] )
  return i3338
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule"] = function (request, data, root) {
  var i3340 = root || new pc.ParticleSystemSizeBySpeed()
  var i3341 = data
  i3340.enabled = !!i3341[0]
  i3340.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3341[1], i3340.x)
  i3340.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3341[2], i3340.y)
  i3340.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3341[3], i3340.z)
  i3340.separateAxes = !!i3341[4]
  i3340.range = new pc.Vec2( i3341[5], i3341[6] )
  return i3340
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule"] = function (request, data, root) {
  var i3342 = root || new pc.ParticleSystemSizeOverLifetime()
  var i3343 = data
  i3342.enabled = !!i3343[0]
  i3342.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3343[1], i3342.x)
  i3342.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3343[2], i3342.y)
  i3342.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3343[3], i3342.z)
  i3342.separateAxes = !!i3343[4]
  return i3342
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule"] = function (request, data, root) {
  var i3344 = root || new pc.ParticleSystemTextureSheetAnimation()
  var i3345 = data
  i3344.enabled = !!i3345[0]
  i3344.mode = i3345[1]
  i3344.animation = i3345[2]
  i3344.numTilesX = i3345[3]
  i3344.numTilesY = i3345[4]
  i3344.useRandomRow = !!i3345[5]
  i3344.frameOverTime = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3345[6], i3344.frameOverTime)
  i3344.startFrame = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3345[7], i3344.startFrame)
  i3344.cycleCount = i3345[8]
  i3344.rowIndex = i3345[9]
  i3344.flipU = i3345[10]
  i3344.flipV = i3345[11]
  i3344.spriteCount = i3345[12]
  var i3347 = i3345[13]
  var i3346 = []
  for(var i = 0; i < i3347.length; i += 2) {
  request.r(i3347[i + 0], i3347[i + 1], 2, i3346, '')
  }
  i3344.sprites = i3346
  return i3344
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule"] = function (request, data, root) {
  var i3350 = root || new pc.ParticleSystemVelocityOverLifetime()
  var i3351 = data
  i3350.enabled = !!i3351[0]
  i3350.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[1], i3350.x)
  i3350.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[2], i3350.y)
  i3350.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[3], i3350.z)
  i3350.radial = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[4], i3350.radial)
  i3350.speedModifier = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[5], i3350.speedModifier)
  i3350.space = i3351[6]
  i3350.orbitalX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[7], i3350.orbitalX)
  i3350.orbitalY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[8], i3350.orbitalY)
  i3350.orbitalZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[9], i3350.orbitalZ)
  i3350.orbitalOffsetX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[10], i3350.orbitalOffsetX)
  i3350.orbitalOffsetY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[11], i3350.orbitalOffsetY)
  i3350.orbitalOffsetZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3351[12], i3350.orbitalOffsetZ)
  return i3350
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule"] = function (request, data, root) {
  var i3352 = root || new pc.ParticleSystemNoise()
  var i3353 = data
  i3352.enabled = !!i3353[0]
  i3352.separateAxes = !!i3353[1]
  i3352.strengthX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[2], i3352.strengthX)
  i3352.strengthY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[3], i3352.strengthY)
  i3352.strengthZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[4], i3352.strengthZ)
  i3352.frequency = i3353[5]
  i3352.damping = !!i3353[6]
  i3352.octaveCount = i3353[7]
  i3352.octaveMultiplier = i3353[8]
  i3352.octaveScale = i3353[9]
  i3352.quality = i3353[10]
  i3352.scrollSpeed = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[11], i3352.scrollSpeed)
  i3352.scrollSpeedMultiplier = i3353[12]
  i3352.remapEnabled = !!i3353[13]
  i3352.remapX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[14], i3352.remapX)
  i3352.remapY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[15], i3352.remapY)
  i3352.remapZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[16], i3352.remapZ)
  i3352.positionAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[17], i3352.positionAmount)
  i3352.rotationAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[18], i3352.rotationAmount)
  i3352.sizeAmount = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3353[19], i3352.sizeAmount)
  return i3352
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule"] = function (request, data, root) {
  var i3354 = root || new pc.ParticleSystemInheritVelocity()
  var i3355 = data
  i3354.enabled = !!i3355[0]
  i3354.mode = i3355[1]
  i3354.curve = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3355[2], i3354.curve)
  return i3354
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule"] = function (request, data, root) {
  var i3356 = root || new pc.ParticleSystemForceOverLifetime()
  var i3357 = data
  i3356.enabled = !!i3357[0]
  i3356.x = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3357[1], i3356.x)
  i3356.y = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3357[2], i3356.y)
  i3356.z = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3357[3], i3356.z)
  i3356.space = i3357[4]
  i3356.randomized = !!i3357[5]
  return i3356
}

Deserializers["Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule"] = function (request, data, root) {
  var i3358 = root || new pc.ParticleSystemLimitVelocityOverLifetime()
  var i3359 = data
  i3358.enabled = !!i3359[0]
  i3358.limit = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3359[1], i3358.limit)
  i3358.limitX = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3359[2], i3358.limitX)
  i3358.limitY = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3359[3], i3358.limitY)
  i3358.limitZ = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3359[4], i3358.limitZ)
  i3358.dampen = i3359[5]
  i3358.separateAxes = !!i3359[6]
  i3358.space = i3359[7]
  i3358.drag = request.d('Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve', i3359[8], i3358.drag)
  i3358.multiplyDragByParticleSize = !!i3359[9]
  i3358.multiplyDragByParticleVelocity = !!i3359[10]
  return i3358
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer"] = function (request, data, root) {
  var i3360 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer' )
  var i3361 = data
  i3360.enabled = !!i3361[0]
  request.r(i3361[1], i3361[2], 0, i3360, 'sharedMaterial')
  var i3363 = i3361[3]
  var i3362 = []
  for(var i = 0; i < i3363.length; i += 2) {
  request.r(i3363[i + 0], i3363[i + 1], 2, i3362, '')
  }
  i3360.sharedMaterials = i3362
  i3360.receiveShadows = !!i3361[4]
  i3360.shadowCastingMode = i3361[5]
  i3360.sortingLayerID = i3361[6]
  i3360.sortingOrder = i3361[7]
  i3360.lightmapIndex = i3361[8]
  i3360.lightmapSceneIndex = i3361[9]
  i3360.lightmapScaleOffset = new pc.Vec4( i3361[10], i3361[11], i3361[12], i3361[13] )
  i3360.lightProbeUsage = i3361[14]
  i3360.reflectionProbeUsage = i3361[15]
  request.r(i3361[16], i3361[17], 0, i3360, 'mesh')
  i3360.meshCount = i3361[18]
  i3360.activeVertexStreamsCount = i3361[19]
  i3360.alignment = i3361[20]
  i3360.renderMode = i3361[21]
  i3360.sortMode = i3361[22]
  i3360.lengthScale = i3361[23]
  i3360.velocityScale = i3361[24]
  i3360.cameraVelocityScale = i3361[25]
  i3360.normalDirection = i3361[26]
  i3360.sortingFudge = i3361[27]
  i3360.minParticleSize = i3361[28]
  i3360.maxParticleSize = i3361[29]
  i3360.pivot = new pc.Vec3( i3361[30], i3361[31], i3361[32] )
  request.r(i3361[33], i3361[34], 0, i3360, 'trailMaterial')
  return i3360
}

Deserializers["BoxChainReaction3D"] = function (request, data, root) {
  var i3364 = root || request.c( 'BoxChainReaction3D' )
  var i3365 = data
  i3364.InitialBoxCount = i3365[0]
  request.r(i3365[1], i3365[2], 0, i3364, 'boosterWhoosh1Clip')
  request.r(i3365[3], i3365[4], 0, i3364, 'boosterImpact1Clip')
  request.r(i3365[5], i3365[6], 0, i3364, 'boxContainer')
  request.r(i3365[7], i3365[8], 0, i3364, 'boxPrefab')
  i3364.initialSpacing = i3365[9]
  i3364.boxWidth = i3365[10]
  i3364.flyInDuration = i3365[11]
  i3364.collisionDuration = i3365[12]
  i3364.repositionDuration = i3365[13]
  i3364.collisionOffset = i3365[14]
  i3364.flyInStartPosition = new pc.Vec3( i3365[15], i3365[16], i3365[17] )
  i3364.flyInStretch = i3365[18]
  i3364.landingSquash = i3365[19]
  i3364.collisionSquash = i3365[20]
  i3364.collisionStretch = i3365[21]
  return i3364
}

Deserializers["GamePlayMeshController"] = function (request, data, root) {
  var i3366 = root || request.c( 'GamePlayMeshController' )
  var i3367 = data
  i3366.LevelId = i3367[0]
  i3366.IrgnoreLevelId = !!i3367[1]
  i3366.LevelData = request.d('LevelData', i3367[2], i3366.LevelData)
  var i3369 = i3367[3]
  var i3368 = new (System.Collections.Generic.List$1(Bridge.ns('WoolControl')))
  for(var i = 0; i < i3369.length; i += 2) {
  request.r(i3369[i + 0], i3369[i + 1], 1, i3368, '')
  }
  i3366.WoolControls = i3368
  request.r(i3367[4], i3367[5], 0, i3366, 'InterestCurveData')
  request.r(i3367[6], i3367[7], 0, i3366, 'WoolAnimationData')
  i3366.TotalColor = i3367[8]
  request.r(i3367[9], i3367[10], 0, i3366, 'WoolMaterial')
  request.r(i3367[11], i3367[12], 0, i3366, 'WoolChildMaterial')
  i3366.MaxLayerHasThreeSameColor = i3367[13]
  request.r(i3367[14], i3367[15], 0, i3366, 'MainMotionAnimator')
  request.r(i3367[16], i3367[17], 0, i3366, 'MainAudioSource')
  var i3371 = i3367[18]
  var i3370 = []
  for(var i = 0; i < i3371.length; i += 2) {
  request.r(i3371[i + 0], i3371[i + 1], 2, i3370, '')
  }
  i3366.MotionAudioClips = i3370
  request.r(i3367[19], i3367[20], 0, i3366, 'CenterTransform')
  i3366._maxDistanceFromCetner = i3367[21]
  return i3366
}

Deserializers["LevelData"] = function (request, data, root) {
  var i3372 = root || request.c( 'LevelData' )
  var i3373 = data
  i3372.LevelId = i3373[0]
  i3372.CurrentcyLevel = i3373[1]
  i3372.DynamicDif = i3373[2]
  var i3375 = i3373[3]
  var i3374 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Color')))
  for(var i = 0; i < i3375.length; i += 4) {
    i3374.add(new pc.Color(i3375[i + 0], i3375[i + 1], i3375[i + 2], i3375[i + 3]));
  }
  i3372.ColorList = i3374
  var i3377 = i3373[4]
  var i3376 = new (System.Collections.Generic.List$1(Bridge.ns('System.Int32')))
  for(var i = 0; i < i3377.length; i += 1) {
    i3376.add(i3377[i + 0]);
  }
  i3372.ColorCountList = i3376
  return i3372
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Animator"] = function (request, data, root) {
  var i3386 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Animator' )
  var i3387 = data
  request.r(i3387[0], i3387[1], 0, i3386, 'animatorController')
  request.r(i3387[2], i3387[3], 0, i3386, 'avatar')
  i3386.updateMode = i3387[4]
  i3386.hasTransformHierarchy = !!i3387[5]
  i3386.applyRootMotion = !!i3387[6]
  var i3389 = i3387[7]
  var i3388 = []
  for(var i = 0; i < i3389.length; i += 2) {
  request.r(i3389[i + 0], i3389[i + 1], 2, i3388, '')
  }
  i3386.humanBones = i3388
  i3386.enabled = !!i3387[8]
  return i3386
}

Deserializers["AnimEvent"] = function (request, data, root) {
  var i3392 = root || request.c( 'AnimEvent' )
  var i3393 = data
  request.r(i3393[0], i3393[1], 0, i3392, 'MainAudioSource')
  var i3395 = i3393[2]
  var i3394 = []
  for(var i = 0; i < i3395.length; i += 2) {
  request.r(i3395[i + 0], i3395[i + 1], 2, i3394, '')
  }
  i3392.MotionAudioClips = i3394
  return i3392
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer"] = function (request, data, root) {
  var i3396 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer' )
  var i3397 = data
  i3396.enabled = !!i3397[0]
  request.r(i3397[1], i3397[2], 0, i3396, 'sharedMaterial')
  var i3399 = i3397[3]
  var i3398 = []
  for(var i = 0; i < i3399.length; i += 2) {
  request.r(i3399[i + 0], i3399[i + 1], 2, i3398, '')
  }
  i3396.sharedMaterials = i3398
  i3396.receiveShadows = !!i3397[4]
  i3396.shadowCastingMode = i3397[5]
  i3396.sortingLayerID = i3397[6]
  i3396.sortingOrder = i3397[7]
  i3396.lightmapIndex = i3397[8]
  i3396.lightmapSceneIndex = i3397[9]
  i3396.lightmapScaleOffset = new pc.Vec4( i3397[10], i3397[11], i3397[12], i3397[13] )
  i3396.lightProbeUsage = i3397[14]
  i3396.reflectionProbeUsage = i3397[15]
  request.r(i3397[16], i3397[17], 0, i3396, 'sharedMesh')
  var i3401 = i3397[18]
  var i3400 = []
  for(var i = 0; i < i3401.length; i += 2) {
  request.r(i3401[i + 0], i3401[i + 1], 2, i3400, '')
  }
  i3396.bones = i3400
  i3396.updateWhenOffscreen = !!i3397[19]
  i3396.localBounds = i3397[20]
  request.r(i3397[21], i3397[22], 0, i3396, 'rootBone')
  var i3403 = i3397[23]
  var i3402 = []
  for(var i = 0; i < i3403.length; i += 1) {
    i3402.push( request.d('Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight', i3403[i + 0]) );
  }
  i3396.blendShapesWeights = i3402
  return i3396
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight"] = function (request, data, root) {
  var i3406 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight' )
  var i3407 = data
  i3406.weight = i3407[0]
  return i3406
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshCollider"] = function (request, data, root) {
  var i3408 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshCollider' )
  var i3409 = data
  i3408.enabled = !!i3409[0]
  i3408.isTrigger = !!i3409[1]
  request.r(i3409[2], i3409[3], 0, i3408, 'material')
  request.r(i3409[4], i3409[5], 0, i3408, 'sharedMesh')
  i3408.convex = !!i3409[6]
  return i3408
}

Deserializers["WoolControl"] = function (request, data, root) {
  var i3410 = root || request.c( 'WoolControl' )
  var i3411 = data
  i3410.debugUV = !!i3411[0]
  i3410.WoolOrder = i3411[1]
  request.r(i3411[2], i3411[3], 0, i3410, 'woolTransform')
  i3410.MeshObjectData = request.d('MeshObjectData', i3411[4], i3410.MeshObjectData)
  request.r(i3411[5], i3411[6], 0, i3410, 'TopMeshRenderer')
  request.r(i3411[7], i3411[8], 0, i3410, 'HideMeshRenderer')
  request.r(i3411[9], i3411[10], 0, i3410, 'BoxCollider')
  request.r(i3411[11], i3411[12], 0, i3410, 'MainMaterial')
  request.r(i3411[13], i3411[14], 0, i3410, 'TranparentMaterial')
  request.r(i3411[15], i3411[16], 0, i3410, 'WoolAnimationData')
  var i3413 = i3411[17]
  var i3412 = new (System.Collections.Generic.List$1(Bridge.ns('DecoreControl')))
  for(var i = 0; i < i3413.length; i += 2) {
  request.r(i3413[i + 0], i3413[i + 1], 1, i3412, '')
  }
  i3410.DecoreControls = i3412
  var i3415 = i3411[18]
  var i3414 = new (System.Collections.Generic.List$1(Bridge.ns('DecoreControl')))
  for(var i = 0; i < i3415.length; i += 2) {
  request.r(i3415[i + 0], i3415[i + 1], 1, i3414, '')
  }
  i3410.RemovedDecoreControls = i3414
  request.r(i3411[19], i3411[20], 0, i3410, 'MeshFilter')
  i3410.IsSetColorHightest = !!i3411[21]
  var i3417 = i3411[22]
  var i3416 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Vector3')))
  for(var i = 0; i < i3417.length; i += 3) {
    i3416.add(new pc.Vec3( i3417[i + 0], i3417[i + 1], i3417[i + 2] ));
  }
  i3410._spiralPath = i3416
  var i3419 = i3411[23]
  var i3418 = new (System.Collections.Generic.List$1(Bridge.ns('System.Single')))
  for(var i = 0; i < i3419.length; i += 1) {
    i3418.add(i3419[i + 0]);
  }
  i3410._spiralPathUVY = i3418
  return i3410
}

Deserializers["MeshObjectData"] = function (request, data, root) {
  var i3420 = root || request.c( 'MeshObjectData' )
  var i3421 = data
  i3420.TotalLayer = i3421[0]
  i3420.HightestColor = new pc.Color(i3421[1], i3421[2], i3421[3], i3421[4])
  var i3423 = i3421[5]
  var i3422 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Color')))
  for(var i = 0; i < i3423.length; i += 4) {
    i3422.add(new pc.Color(i3423[i + 0], i3423[i + 1], i3423[i + 2], i3423[i + 3]));
  }
  i3420.ColorStack = i3422
  return i3420
}

Deserializers["HandController"] = function (request, data, root) {
  var i3430 = root || request.c( 'HandController' )
  var i3431 = data
  var i3433 = i3431[0]
  var i3432 = new (System.Collections.Generic.List$1(Bridge.ns('WoolControl')))
  for(var i = 0; i < i3433.length; i += 2) {
  request.r(i3433[i + 0], i3433[i + 1], 1, i3432, '')
  }
  i3430.WoolControls = i3432
  var i3435 = i3431[1]
  var i3434 = new (System.Collections.Generic.List$1(Bridge.ns('UnityEngine.Sprite')))
  for(var i = 0; i < i3435.length; i += 2) {
  request.r(i3435[i + 0], i3435[i + 1], 1, i3434, '')
  }
  i3430.handSprites = i3434
  request.r(i3431[2], i3431[3], 0, i3430, 'handSpriteRenderer')
  i3430.positionShow = new pc.Vec3( i3431[4], i3431[5], i3431[6] )
  i3430.positionHide = new pc.Vec3( i3431[7], i3431[8], i3431[9] )
  i3430.offset = new pc.Vec3( i3431[10], i3431[11], i3431[12] )
  i3430.delayTime = i3431[13]
  return i3430
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer"] = function (request, data, root) {
  var i3438 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer' )
  var i3439 = data
  i3438.enabled = !!i3439[0]
  request.r(i3439[1], i3439[2], 0, i3438, 'sharedMaterial')
  var i3441 = i3439[3]
  var i3440 = []
  for(var i = 0; i < i3441.length; i += 2) {
  request.r(i3441[i + 0], i3441[i + 1], 2, i3440, '')
  }
  i3438.sharedMaterials = i3440
  i3438.receiveShadows = !!i3439[4]
  i3438.shadowCastingMode = i3439[5]
  i3438.sortingLayerID = i3439[6]
  i3438.sortingOrder = i3439[7]
  i3438.lightmapIndex = i3439[8]
  i3438.lightmapSceneIndex = i3439[9]
  i3438.lightmapScaleOffset = new pc.Vec4( i3439[10], i3439[11], i3439[12], i3439[13] )
  i3438.lightProbeUsage = i3439[14]
  i3438.reflectionProbeUsage = i3439[15]
  i3438.color = new pc.Color(i3439[16], i3439[17], i3439[18], i3439[19])
  request.r(i3439[20], i3439[21], 0, i3438, 'sprite')
  i3438.flipX = !!i3439[22]
  i3438.flipY = !!i3439[23]
  i3438.drawMode = i3439[24]
  i3438.size = new pc.Vec2( i3439[25], i3439[26] )
  i3438.tileMode = i3439[27]
  i3438.adaptiveModeThreshold = i3439[28]
  i3438.maskInteraction = i3439[29]
  i3438.spriteSortPoint = i3439[30]
  return i3438
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i3442 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i3443 = data
  i3442.enabled = !!i3443[0]
  i3442.aspect = i3443[1]
  i3442.orthographic = !!i3443[2]
  i3442.orthographicSize = i3443[3]
  i3442.backgroundColor = new pc.Color(i3443[4], i3443[5], i3443[6], i3443[7])
  i3442.nearClipPlane = i3443[8]
  i3442.farClipPlane = i3443[9]
  i3442.fieldOfView = i3443[10]
  i3442.depth = i3443[11]
  i3442.clearFlags = i3443[12]
  i3442.cullingMask = i3443[13]
  i3442.rect = i3443[14]
  request.r(i3443[15], i3443[16], 0, i3442, 'targetTexture')
  i3442.usePhysicalProperties = !!i3443[17]
  i3442.focalLength = i3443[18]
  i3442.sensorSize = new pc.Vec2( i3443[19], i3443[20] )
  i3442.lensShift = new pc.Vec2( i3443[21], i3443[22] )
  i3442.gateFit = i3443[23]
  i3442.commandBufferCount = i3443[24]
  i3442.cameraType = i3443[25]
  return i3442
}

Deserializers["CameraContainer"] = function (request, data, root) {
  var i3444 = root || request.c( 'CameraContainer' )
  var i3445 = data
  request.r(i3445[0], i3445[1], 0, i3444, 'MainCamera')
  request.r(i3445[2], i3445[3], 0, i3444, 'FakeUICamera')
  request.r(i3445[4], i3445[5], 0, i3444, 'EndgameModelCamera')
  return i3444
}

Deserializers["HandleTapByPointSelection"] = function (request, data, root) {
  var i3446 = root || request.c( 'HandleTapByPointSelection' )
  var i3447 = data
  i3446.selectedIndex = i3447[0]
  i3446.currrentIndex = i3447[1]
  request.r(i3447[2], i3447[3], 0, i3446, 'InputInteractable')
  i3446.layerMask = UnityEngine.LayerMask.FromIntegerValue( i3447[4] )
  var i3449 = i3447[5]
  var i3448 = []
  for(var i = 0; i < i3449.length; i += 1) {
    i3448.push( request.d('WoolPointData', i3449[i + 0]) );
  }
  i3446.woolPoints = i3448
  request.r(i3447[6], i3447[7], 0, i3446, 'handScript')
  i3446.offset = new pc.Vec3( i3447[8], i3447[9], i3447[10] )
  return i3446
}

Deserializers["WoolPointData"] = function (request, data, root) {
  var i3452 = root || request.c( 'WoolPointData' )
  var i3453 = data
  request.r(i3453[0], i3453[1], 0, i3452, 'targetTransform')
  request.r(i3453[2], i3453[3], 0, i3452, 'woolControl')
  request.r(i3453[4], i3453[5], 0, i3452, 'referenceTransform')
  return i3452
}

Deserializers["UnityEngine.EventSystems.EventSystem"] = function (request, data, root) {
  var i3454 = root || request.c( 'UnityEngine.EventSystems.EventSystem' )
  var i3455 = data
  request.r(i3455[0], i3455[1], 0, i3454, 'm_FirstSelected')
  i3454.m_sendNavigationEvents = !!i3455[2]
  i3454.m_DragThreshold = i3455[3]
  return i3454
}

Deserializers["UnityEngine.EventSystems.StandaloneInputModule"] = function (request, data, root) {
  var i3456 = root || request.c( 'UnityEngine.EventSystems.StandaloneInputModule' )
  var i3457 = data
  i3456.m_HorizontalAxis = i3457[0]
  i3456.m_VerticalAxis = i3457[1]
  i3456.m_SubmitButton = i3457[2]
  i3456.m_CancelButton = i3457[3]
  i3456.m_InputActionsPerSecond = i3457[4]
  i3456.m_RepeatDelay = i3457[5]
  i3456.m_ForceModuleActive = !!i3457[6]
  i3456.m_SendPointerHoverToParent = !!i3457[7]
  return i3456
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i3458 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i3459 = data
  i3458.ambientIntensity = i3459[0]
  i3458.reflectionIntensity = i3459[1]
  i3458.ambientMode = i3459[2]
  i3458.ambientLight = new pc.Color(i3459[3], i3459[4], i3459[5], i3459[6])
  i3458.ambientSkyColor = new pc.Color(i3459[7], i3459[8], i3459[9], i3459[10])
  i3458.ambientGroundColor = new pc.Color(i3459[11], i3459[12], i3459[13], i3459[14])
  i3458.ambientEquatorColor = new pc.Color(i3459[15], i3459[16], i3459[17], i3459[18])
  i3458.fogColor = new pc.Color(i3459[19], i3459[20], i3459[21], i3459[22])
  i3458.fogEndDistance = i3459[23]
  i3458.fogStartDistance = i3459[24]
  i3458.fogDensity = i3459[25]
  i3458.fog = !!i3459[26]
  request.r(i3459[27], i3459[28], 0, i3458, 'skybox')
  i3458.fogMode = i3459[29]
  var i3461 = i3459[30]
  var i3460 = []
  for(var i = 0; i < i3461.length; i += 1) {
    i3460.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i3461[i + 0]) );
  }
  i3458.lightmaps = i3460
  i3458.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i3459[31], i3458.lightProbes)
  i3458.lightmapsMode = i3459[32]
  i3458.mixedBakeMode = i3459[33]
  i3458.environmentLightingMode = i3459[34]
  i3458.ambientProbe = new pc.SphericalHarmonicsL2(i3459[35])
  i3458.referenceAmbientProbe = new pc.SphericalHarmonicsL2(i3459[36])
  i3458.useReferenceAmbientProbe = !!i3459[37]
  request.r(i3459[38], i3459[39], 0, i3458, 'customReflection')
  request.r(i3459[40], i3459[41], 0, i3458, 'defaultReflection')
  i3458.defaultReflectionMode = i3459[42]
  i3458.defaultReflectionResolution = i3459[43]
  i3458.sunLightObjectId = i3459[44]
  i3458.pixelLightCount = i3459[45]
  i3458.defaultReflectionHDR = !!i3459[46]
  i3458.hasLightDataAsset = !!i3459[47]
  i3458.hasManualGenerate = !!i3459[48]
  return i3458
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i3464 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i3465 = data
  request.r(i3465[0], i3465[1], 0, i3464, 'lightmapColor')
  request.r(i3465[2], i3465[3], 0, i3464, 'lightmapDirection')
  return i3464
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i3466 = root || new UnityEngine.LightProbes()
  var i3467 = data
  return i3466
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i3472 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i3473 = data
  var i3475 = i3473[0]
  var i3474 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i3475.length; i += 1) {
    i3474.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i3475[i + 0]));
  }
  i3472.ShaderCompilationErrors = i3474
  i3472.name = i3473[1]
  i3472.guid = i3473[2]
  var i3477 = i3473[3]
  var i3476 = []
  for(var i = 0; i < i3477.length; i += 1) {
    i3476.push( i3477[i + 0] );
  }
  i3472.shaderDefinedKeywords = i3476
  var i3479 = i3473[4]
  var i3478 = []
  for(var i = 0; i < i3479.length; i += 1) {
    i3478.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i3479[i + 0]) );
  }
  i3472.passes = i3478
  var i3481 = i3473[5]
  var i3480 = []
  for(var i = 0; i < i3481.length; i += 1) {
    i3480.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i3481[i + 0]) );
  }
  i3472.usePasses = i3480
  var i3483 = i3473[6]
  var i3482 = []
  for(var i = 0; i < i3483.length; i += 1) {
    i3482.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i3483[i + 0]) );
  }
  i3472.defaultParameterValues = i3482
  request.r(i3473[7], i3473[8], 0, i3472, 'unityFallbackShader')
  i3472.readDepth = !!i3473[9]
  i3472.isCreatedByShaderGraph = !!i3473[10]
  i3472.compiled = !!i3473[11]
  return i3472
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i3486 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i3487 = data
  i3486.shaderName = i3487[0]
  i3486.errorMessage = i3487[1]
  return i3486
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i3492 = root || new pc.UnityShaderPass()
  var i3493 = data
  i3492.id = i3493[0]
  i3492.subShaderIndex = i3493[1]
  i3492.name = i3493[2]
  i3492.passType = i3493[3]
  i3492.grabPassTextureName = i3493[4]
  i3492.usePass = !!i3493[5]
  i3492.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[6], i3492.zTest)
  i3492.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[7], i3492.zWrite)
  i3492.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[8], i3492.culling)
  i3492.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i3493[9], i3492.blending)
  i3492.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i3493[10], i3492.alphaBlending)
  i3492.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[11], i3492.colorWriteMask)
  i3492.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[12], i3492.offsetUnits)
  i3492.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[13], i3492.offsetFactor)
  i3492.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[14], i3492.stencilRef)
  i3492.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[15], i3492.stencilReadMask)
  i3492.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3493[16], i3492.stencilWriteMask)
  i3492.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i3493[17], i3492.stencilOp)
  i3492.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i3493[18], i3492.stencilOpFront)
  i3492.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i3493[19], i3492.stencilOpBack)
  var i3495 = i3493[20]
  var i3494 = []
  for(var i = 0; i < i3495.length; i += 1) {
    i3494.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i3495[i + 0]) );
  }
  i3492.tags = i3494
  var i3497 = i3493[21]
  var i3496 = []
  for(var i = 0; i < i3497.length; i += 1) {
    i3496.push( i3497[i + 0] );
  }
  i3492.passDefinedKeywords = i3496
  var i3499 = i3493[22]
  var i3498 = []
  for(var i = 0; i < i3499.length; i += 1) {
    i3498.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i3499[i + 0]) );
  }
  i3492.passDefinedKeywordGroups = i3498
  var i3501 = i3493[23]
  var i3500 = []
  for(var i = 0; i < i3501.length; i += 1) {
    i3500.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i3501[i + 0]) );
  }
  i3492.variants = i3500
  var i3503 = i3493[24]
  var i3502 = []
  for(var i = 0; i < i3503.length; i += 1) {
    i3502.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i3503[i + 0]) );
  }
  i3492.excludedVariants = i3502
  i3492.hasDepthReader = !!i3493[25]
  return i3492
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i3504 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i3505 = data
  i3504.val = i3505[0]
  i3504.name = i3505[1]
  return i3504
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i3506 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i3507 = data
  i3506.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3507[0], i3506.src)
  i3506.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3507[1], i3506.dst)
  i3506.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3507[2], i3506.op)
  return i3506
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i3508 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i3509 = data
  i3508.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3509[0], i3508.pass)
  i3508.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3509[1], i3508.fail)
  i3508.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3509[2], i3508.zFail)
  i3508.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i3509[3], i3508.comp)
  return i3508
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i3512 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i3513 = data
  i3512.name = i3513[0]
  i3512.value = i3513[1]
  return i3512
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i3516 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i3517 = data
  var i3519 = i3517[0]
  var i3518 = []
  for(var i = 0; i < i3519.length; i += 1) {
    i3518.push( i3519[i + 0] );
  }
  i3516.keywords = i3518
  i3516.hasDiscard = !!i3517[1]
  return i3516
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i3522 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i3523 = data
  i3522.passId = i3523[0]
  i3522.subShaderIndex = i3523[1]
  var i3525 = i3523[2]
  var i3524 = []
  for(var i = 0; i < i3525.length; i += 1) {
    i3524.push( i3525[i + 0] );
  }
  i3522.keywords = i3524
  i3522.vertexProgram = i3523[3]
  i3522.fragmentProgram = i3523[4]
  i3522.exportedForWebGl2 = !!i3523[5]
  i3522.readDepth = !!i3523[6]
  return i3522
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i3528 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i3529 = data
  request.r(i3529[0], i3529[1], 0, i3528, 'shader')
  i3528.pass = i3529[2]
  return i3528
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i3532 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i3533 = data
  i3532.name = i3533[0]
  i3532.type = i3533[1]
  i3532.value = new pc.Vec4( i3533[2], i3533[3], i3533[4], i3533[5] )
  i3532.textureValue = i3533[6]
  i3532.shaderPropertyFlag = i3533[7]
  return i3532
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Sprite"] = function (request, data, root) {
  var i3534 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Sprite' )
  var i3535 = data
  i3534.name = i3535[0]
  request.r(i3535[1], i3535[2], 0, i3534, 'texture')
  i3534.aabb = i3535[3]
  i3534.vertices = i3535[4]
  i3534.triangles = i3535[5]
  i3534.textureRect = UnityEngine.Rect.MinMaxRect(i3535[6], i3535[7], i3535[8], i3535[9])
  i3534.packedRect = UnityEngine.Rect.MinMaxRect(i3535[10], i3535[11], i3535[12], i3535[13])
  i3534.border = new pc.Vec4( i3535[14], i3535[15], i3535[16], i3535[17] )
  i3534.transparency = i3535[18]
  i3534.bounds = i3535[19]
  i3534.pixelsPerUnit = i3535[20]
  i3534.textureWidth = i3535[21]
  i3534.textureHeight = i3535[22]
  i3534.nativeSize = new pc.Vec2( i3535[23], i3535[24] )
  i3534.pivot = new pc.Vec2( i3535[25], i3535[26] )
  i3534.textureRectOffset = new pc.Vec2( i3535[27], i3535[28] )
  return i3534
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.AudioClip"] = function (request, data, root) {
  var i3536 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.AudioClip' )
  var i3537 = data
  i3536.name = i3537[0]
  return i3536
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip"] = function (request, data, root) {
  var i3538 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip' )
  var i3539 = data
  i3538.name = i3539[0]
  i3538.wrapMode = i3539[1]
  i3538.isLooping = !!i3539[2]
  i3538.length = i3539[3]
  var i3541 = i3539[4]
  var i3540 = []
  for(var i = 0; i < i3541.length; i += 1) {
    i3540.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve', i3541[i + 0]) );
  }
  i3538.curves = i3540
  var i3543 = i3539[5]
  var i3542 = []
  for(var i = 0; i < i3543.length; i += 1) {
    i3542.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent', i3543[i + 0]) );
  }
  i3538.events = i3542
  i3538.halfPrecision = !!i3539[6]
  i3538._frameRate = i3539[7]
  i3538.localBounds = request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds', i3539[8], i3538.localBounds)
  i3538.hasMuscleCurves = !!i3539[9]
  var i3545 = i3539[10]
  var i3544 = []
  for(var i = 0; i < i3545.length; i += 1) {
    i3544.push( i3545[i + 0] );
  }
  i3538.clipMuscleConstant = i3544
  i3538.clipBindingConstant = request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant', i3539[11], i3538.clipBindingConstant)
  return i3538
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve"] = function (request, data, root) {
  var i3548 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve' )
  var i3549 = data
  i3548.path = i3549[0]
  i3548.hash = i3549[1]
  i3548.componentType = i3549[2]
  i3548.property = i3549[3]
  i3548.keys = i3549[4]
  var i3551 = i3549[5]
  var i3550 = []
  for(var i = 0; i < i3551.length; i += 1) {
    i3550.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey', i3551[i + 0]) );
  }
  i3548.objectReferenceKeys = i3550
  return i3548
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey"] = function (request, data, root) {
  var i3554 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey' )
  var i3555 = data
  i3554.time = i3555[0]
  request.r(i3555[1], i3555[2], 0, i3554, 'value')
  return i3554
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent"] = function (request, data, root) {
  var i3558 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent' )
  var i3559 = data
  i3558.functionName = i3559[0]
  i3558.floatParameter = i3559[1]
  i3558.intParameter = i3559[2]
  i3558.stringParameter = i3559[3]
  request.r(i3559[4], i3559[5], 0, i3558, 'objectReferenceParameter')
  i3558.time = i3559[6]
  return i3558
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds"] = function (request, data, root) {
  var i3560 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds' )
  var i3561 = data
  i3560.center = new pc.Vec3( i3561[0], i3561[1], i3561[2] )
  i3560.extends = new pc.Vec3( i3561[3], i3561[4], i3561[5] )
  return i3560
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant"] = function (request, data, root) {
  var i3564 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant' )
  var i3565 = data
  var i3567 = i3565[0]
  var i3566 = []
  for(var i = 0; i < i3567.length; i += 1) {
    i3566.push( i3567[i + 0] );
  }
  i3564.genericBindings = i3566
  var i3569 = i3565[1]
  var i3568 = []
  for(var i = 0; i < i3569.length; i += 1) {
    i3568.push( i3569[i + 0] );
  }
  i3564.pptrCurveMapping = i3568
  return i3564
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font"] = function (request, data, root) {
  var i3570 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font' )
  var i3571 = data
  i3570.name = i3571[0]
  i3570.ascent = i3571[1]
  i3570.originalLineHeight = i3571[2]
  i3570.fontSize = i3571[3]
  var i3573 = i3571[4]
  var i3572 = []
  for(var i = 0; i < i3573.length; i += 1) {
    i3572.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo', i3573[i + 0]) );
  }
  i3570.characterInfo = i3572
  request.r(i3571[5], i3571[6], 0, i3570, 'texture')
  i3570.originalFontSize = i3571[7]
  return i3570
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo"] = function (request, data, root) {
  var i3576 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo' )
  var i3577 = data
  i3576.index = i3577[0]
  i3576.advance = i3577[1]
  i3576.bearing = i3577[2]
  i3576.glyphWidth = i3577[3]
  i3576.glyphHeight = i3577[4]
  i3576.minX = i3577[5]
  i3576.maxX = i3577[6]
  i3576.minY = i3577[7]
  i3576.maxY = i3577[8]
  i3576.uvBottomLeftX = i3577[9]
  i3576.uvBottomLeftY = i3577[10]
  i3576.uvBottomRightX = i3577[11]
  i3576.uvBottomRightY = i3577[12]
  i3576.uvTopLeftX = i3577[13]
  i3576.uvTopLeftY = i3577[14]
  i3576.uvTopRightX = i3577[15]
  i3576.uvTopRightY = i3577[16]
  return i3576
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorController"] = function (request, data, root) {
  var i3578 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorController' )
  var i3579 = data
  i3578.name = i3579[0]
  var i3581 = i3579[1]
  var i3580 = []
  for(var i = 0; i < i3581.length; i += 1) {
    i3580.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer', i3581[i + 0]) );
  }
  i3578.layers = i3580
  var i3583 = i3579[2]
  var i3582 = []
  for(var i = 0; i < i3583.length; i += 1) {
    i3582.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter', i3583[i + 0]) );
  }
  i3578.parameters = i3582
  i3578.animationClips = i3579[3]
  i3578.avatarUnsupported = i3579[4]
  return i3578
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer"] = function (request, data, root) {
  var i3586 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer' )
  var i3587 = data
  i3586.name = i3587[0]
  i3586.defaultWeight = i3587[1]
  i3586.blendingMode = i3587[2]
  i3586.avatarMask = i3587[3]
  i3586.syncedLayerIndex = i3587[4]
  i3586.syncedLayerAffectsTiming = !!i3587[5]
  i3586.syncedLayers = i3587[6]
  i3586.stateMachine = request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine', i3587[7], i3586.stateMachine)
  return i3586
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine"] = function (request, data, root) {
  var i3588 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine' )
  var i3589 = data
  i3588.id = i3589[0]
  i3588.name = i3589[1]
  i3588.path = i3589[2]
  var i3591 = i3589[3]
  var i3590 = []
  for(var i = 0; i < i3591.length; i += 1) {
    i3590.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState', i3591[i + 0]) );
  }
  i3588.states = i3590
  var i3593 = i3589[4]
  var i3592 = []
  for(var i = 0; i < i3593.length; i += 1) {
    i3592.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine', i3593[i + 0]) );
  }
  i3588.machines = i3592
  var i3595 = i3589[5]
  var i3594 = []
  for(var i = 0; i < i3595.length; i += 1) {
    i3594.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition', i3595[i + 0]) );
  }
  i3588.entryStateTransitions = i3594
  var i3597 = i3589[6]
  var i3596 = []
  for(var i = 0; i < i3597.length; i += 1) {
    i3596.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition', i3597[i + 0]) );
  }
  i3588.exitStateTransitions = i3596
  var i3599 = i3589[7]
  var i3598 = []
  for(var i = 0; i < i3599.length; i += 1) {
    i3598.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition', i3599[i + 0]) );
  }
  i3588.anyStateTransitions = i3598
  i3588.defaultStateId = i3589[8]
  return i3588
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState"] = function (request, data, root) {
  var i3602 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState' )
  var i3603 = data
  i3602.id = i3603[0]
  i3602.name = i3603[1]
  i3602.cycleOffset = i3603[2]
  i3602.cycleOffsetParameter = i3603[3]
  i3602.cycleOffsetParameterActive = !!i3603[4]
  i3602.mirror = !!i3603[5]
  i3602.mirrorParameter = i3603[6]
  i3602.mirrorParameterActive = !!i3603[7]
  i3602.motionId = i3603[8]
  i3602.nameHash = i3603[9]
  i3602.fullPathHash = i3603[10]
  i3602.speed = i3603[11]
  i3602.speedParameter = i3603[12]
  i3602.speedParameterActive = !!i3603[13]
  i3602.tag = i3603[14]
  i3602.tagHash = i3603[15]
  i3602.writeDefaultValues = !!i3603[16]
  var i3605 = i3603[17]
  var i3604 = []
  for(var i = 0; i < i3605.length; i += 2) {
  request.r(i3605[i + 0], i3605[i + 1], 2, i3604, '')
  }
  i3602.behaviours = i3604
  var i3607 = i3603[18]
  var i3606 = []
  for(var i = 0; i < i3607.length; i += 1) {
    i3606.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition', i3607[i + 0]) );
  }
  i3602.transitions = i3606
  return i3602
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition"] = function (request, data, root) {
  var i3612 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition' )
  var i3613 = data
  i3612.fullPath = i3613[0]
  i3612.canTransitionToSelf = !!i3613[1]
  i3612.duration = i3613[2]
  i3612.exitTime = i3613[3]
  i3612.hasExitTime = !!i3613[4]
  i3612.hasFixedDuration = !!i3613[5]
  i3612.interruptionSource = i3613[6]
  i3612.offset = i3613[7]
  i3612.orderedInterruption = !!i3613[8]
  i3612.destinationStateId = i3613[9]
  i3612.isExit = !!i3613[10]
  i3612.mute = !!i3613[11]
  i3612.solo = !!i3613[12]
  var i3615 = i3613[13]
  var i3614 = []
  for(var i = 0; i < i3615.length; i += 1) {
    i3614.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition', i3615[i + 0]) );
  }
  i3612.conditions = i3614
  return i3612
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition"] = function (request, data, root) {
  var i3618 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition' )
  var i3619 = data
  i3618.mode = i3619[0]
  i3618.parameter = i3619[1]
  i3618.threshold = i3619[2]
  return i3618
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition"] = function (request, data, root) {
  var i3624 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition' )
  var i3625 = data
  i3624.destinationStateId = i3625[0]
  i3624.isExit = !!i3625[1]
  i3624.mute = !!i3625[2]
  i3624.solo = !!i3625[3]
  var i3627 = i3625[4]
  var i3626 = []
  for(var i = 0; i < i3627.length; i += 1) {
    i3626.push( request.d('Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition', i3627[i + 0]) );
  }
  i3624.conditions = i3626
  return i3624
}

Deserializers["Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter"] = function (request, data, root) {
  var i3630 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter' )
  var i3631 = data
  i3630.defaultBool = !!i3631[0]
  i3630.defaultFloat = i3631[1]
  i3630.defaultInt = i3631[2]
  i3630.name = i3631[3]
  i3630.nameHash = i3631[4]
  i3630.type = i3631[5]
  return i3630
}

Deserializers["WoolAnimationData"] = function (request, data, root) {
  var i3632 = root || request.c( 'WoolAnimationData' )
  var i3633 = data
  i3632.Duration = i3633[0]
  i3632.DurationHideWool = i3633[1]
  i3632.OffSet = i3633[2]
  i3632.ForceValue = i3633[3]
  i3632.RandomDirrectionFactor = i3633[4]
  return i3632
}

Deserializers["ZoomCameraData"] = function (request, data, root) {
  var i3634 = root || request.c( 'ZoomCameraData' )
  var i3635 = data
  i3634.ZoomSpeed = i3635[0]
  i3634.MinFOV = i3635[1]
  i3634.MaxFOV = i3635[2]
  i3634.DefaultFOV = i3635[3]
  return i3634
}

Deserializers["DG.Tweening.Core.DOTweenSettings"] = function (request, data, root) {
  var i3636 = root || request.c( 'DG.Tweening.Core.DOTweenSettings' )
  var i3637 = data
  i3636.useSafeMode = !!i3637[0]
  i3636.safeModeOptions = request.d('DG.Tweening.Core.DOTweenSettings+SafeModeOptions', i3637[1], i3636.safeModeOptions)
  i3636.timeScale = i3637[2]
  i3636.unscaledTimeScale = i3637[3]
  i3636.useSmoothDeltaTime = !!i3637[4]
  i3636.maxSmoothUnscaledTime = i3637[5]
  i3636.rewindCallbackMode = i3637[6]
  i3636.showUnityEditorReport = !!i3637[7]
  i3636.logBehaviour = i3637[8]
  i3636.drawGizmos = !!i3637[9]
  i3636.defaultRecyclable = !!i3637[10]
  i3636.defaultAutoPlay = i3637[11]
  i3636.defaultUpdateType = i3637[12]
  i3636.defaultTimeScaleIndependent = !!i3637[13]
  i3636.defaultEaseType = i3637[14]
  i3636.defaultEaseOvershootOrAmplitude = i3637[15]
  i3636.defaultEasePeriod = i3637[16]
  i3636.defaultAutoKill = !!i3637[17]
  i3636.defaultLoopType = i3637[18]
  i3636.debugMode = !!i3637[19]
  i3636.debugStoreTargetId = !!i3637[20]
  i3636.showPreviewPanel = !!i3637[21]
  i3636.storeSettingsLocation = i3637[22]
  i3636.modules = request.d('DG.Tweening.Core.DOTweenSettings+ModulesSetup', i3637[23], i3636.modules)
  i3636.createASMDEF = !!i3637[24]
  i3636.showPlayingTweens = !!i3637[25]
  i3636.showPausedTweens = !!i3637[26]
  return i3636
}

Deserializers["DG.Tweening.Core.DOTweenSettings+SafeModeOptions"] = function (request, data, root) {
  var i3638 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+SafeModeOptions' )
  var i3639 = data
  i3638.logBehaviour = i3639[0]
  i3638.nestedTweenFailureBehaviour = i3639[1]
  return i3638
}

Deserializers["DG.Tweening.Core.DOTweenSettings+ModulesSetup"] = function (request, data, root) {
  var i3640 = root || request.c( 'DG.Tweening.Core.DOTweenSettings+ModulesSetup' )
  var i3641 = data
  i3640.showPanel = !!i3641[0]
  i3640.audioEnabled = !!i3641[1]
  i3640.physicsEnabled = !!i3641[2]
  i3640.physics2DEnabled = !!i3641[3]
  i3640.spriteEnabled = !!i3641[4]
  i3640.uiEnabled = !!i3641[5]
  i3640.textMeshProEnabled = !!i3641[6]
  i3640.tk2DEnabled = !!i3641[7]
  i3640.deAudioEnabled = !!i3641[8]
  i3640.deUnityExtendedEnabled = !!i3641[9]
  i3640.epoOutlineEnabled = !!i3641[10]
  return i3640
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i3642 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i3643 = data
  var i3645 = i3643[0]
  var i3644 = []
  for(var i = 0; i < i3645.length; i += 1) {
    i3644.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i3645[i + 0]) );
  }
  i3642.files = i3644
  i3642.componentToPrefabIds = i3643[1]
  return i3642
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i3648 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i3649 = data
  i3648.path = i3649[0]
  request.r(i3649[1], i3649[2], 0, i3648, 'unityObject')
  return i3648
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i3650 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i3651 = data
  var i3653 = i3651[0]
  var i3652 = []
  for(var i = 0; i < i3653.length; i += 1) {
    i3652.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i3653[i + 0]) );
  }
  i3650.scriptsExecutionOrder = i3652
  var i3655 = i3651[1]
  var i3654 = []
  for(var i = 0; i < i3655.length; i += 1) {
    i3654.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i3655[i + 0]) );
  }
  i3650.sortingLayers = i3654
  var i3657 = i3651[2]
  var i3656 = []
  for(var i = 0; i < i3657.length; i += 1) {
    i3656.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i3657[i + 0]) );
  }
  i3650.cullingLayers = i3656
  i3650.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i3651[3], i3650.timeSettings)
  i3650.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i3651[4], i3650.physicsSettings)
  i3650.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i3651[5], i3650.physics2DSettings)
  i3650.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i3651[6], i3650.qualitySettings)
  i3650.enableRealtimeShadows = !!i3651[7]
  i3650.enableAutoInstancing = !!i3651[8]
  i3650.enableDynamicBatching = !!i3651[9]
  i3650.lightmapEncodingQuality = i3651[10]
  i3650.desiredColorSpace = i3651[11]
  var i3659 = i3651[12]
  var i3658 = []
  for(var i = 0; i < i3659.length; i += 1) {
    i3658.push( i3659[i + 0] );
  }
  i3650.allTags = i3658
  return i3650
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i3662 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i3663 = data
  i3662.name = i3663[0]
  i3662.value = i3663[1]
  return i3662
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i3666 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i3667 = data
  i3666.id = i3667[0]
  i3666.name = i3667[1]
  i3666.value = i3667[2]
  return i3666
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i3670 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i3671 = data
  i3670.id = i3671[0]
  i3670.name = i3671[1]
  return i3670
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i3672 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i3673 = data
  i3672.fixedDeltaTime = i3673[0]
  i3672.maximumDeltaTime = i3673[1]
  i3672.timeScale = i3673[2]
  i3672.maximumParticleTimestep = i3673[3]
  return i3672
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i3674 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i3675 = data
  i3674.gravity = new pc.Vec3( i3675[0], i3675[1], i3675[2] )
  i3674.defaultSolverIterations = i3675[3]
  i3674.bounceThreshold = i3675[4]
  i3674.autoSyncTransforms = !!i3675[5]
  i3674.autoSimulation = !!i3675[6]
  var i3677 = i3675[7]
  var i3676 = []
  for(var i = 0; i < i3677.length; i += 1) {
    i3676.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i3677[i + 0]) );
  }
  i3674.collisionMatrix = i3676
  return i3674
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i3680 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i3681 = data
  i3680.enabled = !!i3681[0]
  i3680.layerId = i3681[1]
  i3680.otherLayerId = i3681[2]
  return i3680
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i3682 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i3683 = data
  request.r(i3683[0], i3683[1], 0, i3682, 'material')
  i3682.gravity = new pc.Vec2( i3683[2], i3683[3] )
  i3682.positionIterations = i3683[4]
  i3682.velocityIterations = i3683[5]
  i3682.velocityThreshold = i3683[6]
  i3682.maxLinearCorrection = i3683[7]
  i3682.maxAngularCorrection = i3683[8]
  i3682.maxTranslationSpeed = i3683[9]
  i3682.maxRotationSpeed = i3683[10]
  i3682.baumgarteScale = i3683[11]
  i3682.baumgarteTOIScale = i3683[12]
  i3682.timeToSleep = i3683[13]
  i3682.linearSleepTolerance = i3683[14]
  i3682.angularSleepTolerance = i3683[15]
  i3682.defaultContactOffset = i3683[16]
  i3682.autoSimulation = !!i3683[17]
  i3682.queriesHitTriggers = !!i3683[18]
  i3682.queriesStartInColliders = !!i3683[19]
  i3682.callbacksOnDisable = !!i3683[20]
  i3682.reuseCollisionCallbacks = !!i3683[21]
  i3682.autoSyncTransforms = !!i3683[22]
  var i3685 = i3683[23]
  var i3684 = []
  for(var i = 0; i < i3685.length; i += 1) {
    i3684.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i3685[i + 0]) );
  }
  i3682.collisionMatrix = i3684
  return i3682
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i3688 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i3689 = data
  i3688.enabled = !!i3689[0]
  i3688.layerId = i3689[1]
  i3688.otherLayerId = i3689[2]
  return i3688
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i3690 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i3691 = data
  var i3693 = i3691[0]
  var i3692 = []
  for(var i = 0; i < i3693.length; i += 1) {
    i3692.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i3693[i + 0]) );
  }
  i3690.qualityLevels = i3692
  var i3695 = i3691[1]
  var i3694 = []
  for(var i = 0; i < i3695.length; i += 1) {
    i3694.push( i3695[i + 0] );
  }
  i3690.names = i3694
  i3690.shadows = i3691[2]
  i3690.anisotropicFiltering = i3691[3]
  i3690.antiAliasing = i3691[4]
  i3690.lodBias = i3691[5]
  i3690.shadowCascades = i3691[6]
  i3690.shadowDistance = i3691[7]
  i3690.shadowmaskMode = i3691[8]
  i3690.shadowProjection = i3691[9]
  i3690.shadowResolution = i3691[10]
  i3690.softParticles = !!i3691[11]
  i3690.softVegetation = !!i3691[12]
  i3690.activeColorSpace = i3691[13]
  i3690.desiredColorSpace = i3691[14]
  i3690.masterTextureLimit = i3691[15]
  i3690.maxQueuedFrames = i3691[16]
  i3690.particleRaycastBudget = i3691[17]
  i3690.pixelLightCount = i3691[18]
  i3690.realtimeReflectionProbes = !!i3691[19]
  i3690.shadowCascade2Split = i3691[20]
  i3690.shadowCascade4Split = new pc.Vec3( i3691[21], i3691[22], i3691[23] )
  i3690.streamingMipmapsActive = !!i3691[24]
  i3690.vSyncCount = i3691[25]
  i3690.asyncUploadBufferSize = i3691[26]
  i3690.asyncUploadTimeSlice = i3691[27]
  i3690.billboardsFaceCameraPosition = !!i3691[28]
  i3690.shadowNearPlaneOffset = i3691[29]
  i3690.streamingMipmapsMemoryBudget = i3691[30]
  i3690.maximumLODLevel = i3691[31]
  i3690.streamingMipmapsAddAllCameras = !!i3691[32]
  i3690.streamingMipmapsMaxLevelReduction = i3691[33]
  i3690.streamingMipmapsRenderersPerFrame = i3691[34]
  i3690.resolutionScalingFixedDPIFactor = i3691[35]
  i3690.streamingMipmapsMaxFileIORequests = i3691[36]
  i3690.currentQualityLevel = i3691[37]
  return i3690
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixer"] = function (request, data, root) {
  var i3698 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixer' )
  var i3699 = data
  var i3701 = i3699[0]
  var i3700 = []
  for(var i = 0; i < i3701.length; i += 1) {
    i3700.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup', i3701[i + 0]) );
  }
  i3698.groups = i3700
  var i3703 = i3699[1]
  var i3702 = []
  for(var i = 0; i < i3703.length; i += 1) {
    i3702.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot', i3703[i + 0]) );
  }
  i3698.snapshots = i3702
  return i3698
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup"] = function (request, data, root) {
  var i3706 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup' )
  var i3707 = data
  i3706.id = i3707[0]
  i3706.childGroupIds = i3707[1]
  i3706.name = i3707[2]
  return i3706
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot"] = function (request, data, root) {
  var i3710 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot' )
  var i3711 = data
  i3710.id = i3711[0]
  var i3713 = i3711[1]
  var i3712 = []
  for(var i = 0; i < i3713.length; i += 1) {
    i3712.push( request.d('Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter', i3713[i + 0]) );
  }
  i3710.parameters = i3712
  return i3710
}

Deserializers["Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter"] = function (request, data, root) {
  var i3716 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter' )
  var i3717 = data
  i3716.name = i3717[0]
  i3716.value = i3717[1]
  return i3716
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame"] = function (request, data, root) {
  var i3720 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame' )
  var i3721 = data
  i3720.weight = i3721[0]
  i3720.vertices = i3721[1]
  i3720.normals = i3721[2]
  i3720.tangents = i3721[3]
  return i3720
}

Deserializers["UnityEngine.Events.ArgumentCache"] = function (request, data, root) {
  var i3722 = root || request.c( 'UnityEngine.Events.ArgumentCache' )
  var i3723 = data
  request.r(i3723[0], i3723[1], 0, i3722, 'm_ObjectArgument')
  i3722.m_ObjectArgumentAssemblyTypeName = i3723[2]
  i3722.m_IntArgument = i3723[3]
  i3722.m_FloatArgument = i3723[4]
  i3722.m_StringArgument = i3723[5]
  i3722.m_BoolArgument = !!i3723[6]
  return i3722
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Textures.Texture2D":{"name":0,"width":1,"height":2,"mipmapCount":3,"anisoLevel":4,"filterMode":5,"hdr":6,"format":7,"wrapMode":8,"alphaIsTransparency":9,"alphaSource":10,"graphicsFormat":11,"sRGBTexture":12,"desiredColorSpace":13,"wrapU":14,"wrapV":15},"Luna.Unity.DTO.UnityEngine.Assets.Mesh":{"name":0,"halfPrecision":1,"useUInt32IndexFormat":2,"vertexCount":3,"aabb":4,"streams":5,"vertices":6,"subMeshes":7,"bindposes":8,"blendShapes":9},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh":{"triangles":0},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape":{"name":0,"frames":1},"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Components.Transform":{"position":0,"scale":3,"rotation":6},"Luna.Unity.DTO.UnityEngine.Components.MeshRenderer":{"additionalVertexStreams":0,"enabled":2,"sharedMaterial":3,"sharedMaterials":5,"receiveShadows":6,"shadowCastingMode":7,"sortingLayerID":8,"sortingOrder":9,"lightmapIndex":10,"lightmapSceneIndex":11,"lightmapScaleOffset":12,"lightProbeUsage":16,"reflectionProbeUsage":17},"Luna.Unity.DTO.UnityEngine.Components.MeshFilter":{"sharedMesh":0},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Components.LineRenderer":{"textureMode":0,"alignment":1,"widthCurve":2,"colorGradient":3,"positions":4,"positionCount":5,"widthMultiplier":6,"startWidth":7,"endWidth":8,"numCornerVertices":9,"numCapVertices":10,"useWorldSpace":11,"loop":12,"startColor":13,"endColor":17,"generateLightingData":21,"enabled":22,"sharedMaterial":23,"sharedMaterials":25,"receiveShadows":26,"shadowCastingMode":27,"sortingLayerID":28,"sortingOrder":29,"lightmapIndex":30,"lightmapSceneIndex":31,"lightmapScaleOffset":32,"lightProbeUsage":36,"reflectionProbeUsage":37},"Luna.Unity.DTO.UnityEngine.Textures.Cubemap":{"name":0,"atlasId":1,"mipmapCount":2,"hdr":3,"size":4,"anisoLevel":5,"filterMode":6,"rects":7,"wrapU":8,"wrapV":9},"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.Light":{"enabled":0,"type":1,"color":2,"cullingMask":6,"intensity":7,"range":8,"spotAngle":9,"shadows":10,"shadowNormalBias":11,"shadowBias":12,"shadowStrength":13,"shadowResolution":14,"lightmapBakeType":15,"renderMode":16,"cookie":17,"cookieSize":19},"Luna.Unity.DTO.UnityEngine.Components.RectTransform":{"pivot":0,"anchorMin":2,"anchorMax":4,"sizeDelta":6,"anchoredPosition3D":8,"rotation":11,"scale":15},"Luna.Unity.DTO.UnityEngine.Components.Canvas":{"enabled":0,"planeDistance":1,"referencePixelsPerUnit":2,"isFallbackOverlay":3,"renderMode":4,"renderOrder":5,"sortingLayerName":6,"sortingOrder":7,"scaleFactor":8,"worldCamera":9,"overrideSorting":11,"pixelPerfect":12,"targetDisplay":13,"overridePixelPerfect":14},"Luna.Unity.DTO.UnityEngine.Components.CanvasGroup":{"m_Alpha":0,"m_Interactable":1,"m_BlocksRaycasts":2,"m_IgnoreParentGroups":3,"enabled":4},"Luna.Unity.DTO.UnityEngine.Components.CanvasRenderer":{"cullTransparentMesh":0},"Luna.Unity.DTO.UnityEngine.Components.AudioSource":{"clip":0,"outputAudioMixerGroup":2,"playOnAwake":4,"loop":5,"time":6,"volume":7,"pitch":8,"enabled":9},"Luna.Unity.DTO.UnityEngine.Components.BoxCollider":{"center":0,"size":3,"enabled":6,"isTrigger":7,"material":8},"Luna.Unity.DTO.UnityEngine.Components.ParticleSystem":{"main":0,"colorBySpeed":1,"colorOverLifetime":2,"emission":3,"rotationBySpeed":4,"rotationOverLifetime":5,"shape":6,"sizeBySpeed":7,"sizeOverLifetime":8,"textureSheetAnimation":9,"velocityOverLifetime":10,"noise":11,"inheritVelocity":12,"forceOverLifetime":13,"limitVelocityOverLifetime":14,"useAutoRandomSeed":15,"randomSeed":16},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.MainModule":{"duration":0,"loop":1,"prewarm":2,"startDelay":3,"startLifetime":4,"startSpeed":5,"startSize3D":6,"startSizeX":7,"startSizeY":8,"startSizeZ":9,"startRotation3D":10,"startRotationX":11,"startRotationY":12,"startRotationZ":13,"startColor":14,"gravityModifier":15,"simulationSpace":16,"customSimulationSpace":17,"simulationSpeed":19,"useUnscaledTime":20,"scalingMode":21,"playOnAwake":22,"maxParticles":23,"emitterVelocityMode":24,"stopAction":25},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxCurve":{"mode":0,"curveMin":1,"curveMax":2,"curveMultiplier":3,"constantMin":4,"constantMax":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.MinMaxGradient":{"mode":0,"gradientMin":1,"gradientMax":2,"colorMin":3,"colorMax":7},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Gradient":{"mode":0,"colorKeys":1,"alphaKeys":2},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorBySpeedModule":{"enabled":0,"color":1,"range":2},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientColorKey":{"color":0,"time":4},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Data.GradientAlphaKey":{"alpha":0,"time":1},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ColorOverLifetimeModule":{"enabled":0,"color":1},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.EmissionModule":{"enabled":0,"rateOverTime":1,"rateOverDistance":2,"bursts":3},"Luna.Unity.DTO.UnityEngine.ParticleSystemTypes.Burst":{"count":0,"cycleCount":1,"minCount":2,"maxCount":3,"repeatInterval":4,"time":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationBySpeedModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4,"range":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.RotationOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ShapeModule":{"enabled":0,"shapeType":1,"randomDirectionAmount":2,"sphericalDirectionAmount":3,"randomPositionAmount":4,"alignToDirection":5,"radius":6,"radiusMode":7,"radiusSpread":8,"radiusSpeed":9,"radiusThickness":10,"angle":11,"length":12,"boxThickness":13,"meshShapeType":16,"mesh":17,"meshRenderer":19,"skinnedMeshRenderer":21,"useMeshMaterialIndex":23,"meshMaterialIndex":24,"useMeshColors":25,"normalOffset":26,"arc":27,"arcMode":28,"arcSpread":29,"arcSpeed":30,"donutRadius":31,"position":32,"rotation":35,"scale":38},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeBySpeedModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4,"range":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.SizeOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"separateAxes":4},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.TextureSheetAnimationModule":{"enabled":0,"mode":1,"animation":2,"numTilesX":3,"numTilesY":4,"useRandomRow":5,"frameOverTime":6,"startFrame":7,"cycleCount":8,"rowIndex":9,"flipU":10,"flipV":11,"spriteCount":12,"sprites":13},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.VelocityOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"radial":4,"speedModifier":5,"space":6,"orbitalX":7,"orbitalY":8,"orbitalZ":9,"orbitalOffsetX":10,"orbitalOffsetY":11,"orbitalOffsetZ":12},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.NoiseModule":{"enabled":0,"separateAxes":1,"strengthX":2,"strengthY":3,"strengthZ":4,"frequency":5,"damping":6,"octaveCount":7,"octaveMultiplier":8,"octaveScale":9,"quality":10,"scrollSpeed":11,"scrollSpeedMultiplier":12,"remapEnabled":13,"remapX":14,"remapY":15,"remapZ":16,"positionAmount":17,"rotationAmount":18,"sizeAmount":19},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.InheritVelocityModule":{"enabled":0,"mode":1,"curve":2},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.ForceOverLifetimeModule":{"enabled":0,"x":1,"y":2,"z":3,"space":4,"randomized":5},"Luna.Unity.DTO.UnityEngine.ParticleSystemModules.LimitVelocityOverLifetimeModule":{"enabled":0,"limit":1,"limitX":2,"limitY":3,"limitZ":4,"dampen":5,"separateAxes":6,"space":7,"drag":8,"multiplyDragByParticleSize":9,"multiplyDragByParticleVelocity":10},"Luna.Unity.DTO.UnityEngine.Components.ParticleSystemRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"mesh":16,"meshCount":18,"activeVertexStreamsCount":19,"alignment":20,"renderMode":21,"sortMode":22,"lengthScale":23,"velocityScale":24,"cameraVelocityScale":25,"normalDirection":26,"sortingFudge":27,"minParticleSize":28,"maxParticleSize":29,"pivot":30,"trailMaterial":33},"Luna.Unity.DTO.UnityEngine.Components.Animator":{"animatorController":0,"avatar":2,"updateMode":4,"hasTransformHierarchy":5,"applyRootMotion":6,"humanBones":7,"enabled":8},"Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"sharedMesh":16,"bones":18,"updateWhenOffscreen":19,"localBounds":20,"rootBone":21,"blendShapesWeights":23},"Luna.Unity.DTO.UnityEngine.Components.SkinnedMeshRenderer+BlendShapeWeight":{"weight":0},"Luna.Unity.DTO.UnityEngine.Components.MeshCollider":{"enabled":0,"isTrigger":1,"material":2,"sharedMesh":4,"convex":6},"Luna.Unity.DTO.UnityEngine.Components.SpriteRenderer":{"enabled":0,"sharedMaterial":1,"sharedMaterials":3,"receiveShadows":4,"shadowCastingMode":5,"sortingLayerID":6,"sortingOrder":7,"lightmapIndex":8,"lightmapSceneIndex":9,"lightmapScaleOffset":10,"lightProbeUsage":14,"reflectionProbeUsage":15,"color":16,"sprite":20,"flipX":22,"flipY":23,"drawMode":24,"size":25,"tileMode":27,"adaptiveModeThreshold":28,"maskInteraction":29,"spriteSortPoint":30},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"enabled":0,"aspect":1,"orthographic":2,"orthographicSize":3,"backgroundColor":4,"nearClipPlane":8,"farClipPlane":9,"fieldOfView":10,"depth":11,"clearFlags":12,"cullingMask":13,"rect":14,"targetTexture":15,"usePhysicalProperties":17,"focalLength":18,"sensorSize":19,"lensShift":21,"gateFit":23,"commandBufferCount":24,"cameraType":25},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"referenceAmbientProbe":36,"useReferenceAmbientProbe":37,"customReflection":38,"defaultReflection":40,"defaultReflectionMode":42,"defaultReflectionResolution":43,"sunLightObjectId":44,"pixelLightCount":45,"defaultReflectionHDR":46,"hasLightDataAsset":47,"hasManualGenerate":48},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"isCreatedByShaderGraph":10,"compiled":11},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Textures.Sprite":{"name":0,"texture":1,"aabb":3,"vertices":4,"triangles":5,"textureRect":6,"packedRect":10,"border":14,"transparency":18,"bounds":19,"pixelsPerUnit":20,"textureWidth":21,"textureHeight":22,"nativeSize":23,"pivot":25,"textureRectOffset":27},"Luna.Unity.DTO.UnityEngine.Assets.AudioClip":{"name":0},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip":{"name":0,"wrapMode":1,"isLooping":2,"length":3,"curves":4,"events":5,"halfPrecision":6,"_frameRate":7,"localBounds":8,"hasMuscleCurves":9,"clipMuscleConstant":10,"clipBindingConstant":11},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve":{"path":0,"hash":1,"componentType":2,"property":3,"keys":4,"objectReferenceKeys":5},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationCurve+ObjectReferenceKey":{"time":0,"value":1},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationEvent":{"functionName":0,"floatParameter":1,"intParameter":2,"stringParameter":3,"objectReferenceParameter":4,"time":6},"Luna.Unity.DTO.UnityEngine.Animation.Data.Bounds":{"center":0,"extends":3},"Luna.Unity.DTO.UnityEngine.Animation.Data.AnimationClip+AnimationClipBindingConstant":{"genericBindings":0,"pptrCurveMapping":1},"Luna.Unity.DTO.UnityEngine.Assets.Font":{"name":0,"ascent":1,"originalLineHeight":2,"fontSize":3,"characterInfo":4,"texture":5,"originalFontSize":7},"Luna.Unity.DTO.UnityEngine.Assets.Font+CharacterInfo":{"index":0,"advance":1,"bearing":2,"glyphWidth":3,"glyphHeight":4,"minX":5,"maxX":6,"minY":7,"maxY":8,"uvBottomLeftX":9,"uvBottomLeftY":10,"uvBottomRightX":11,"uvBottomRightY":12,"uvTopLeftX":13,"uvTopLeftY":14,"uvTopRightX":15,"uvTopRightY":16},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorController":{"name":0,"layers":1,"parameters":2,"animationClips":3,"avatarUnsupported":4},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerLayer":{"name":0,"defaultWeight":1,"blendingMode":2,"avatarMask":3,"syncedLayerIndex":4,"syncedLayerAffectsTiming":5,"syncedLayers":6,"stateMachine":7},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateMachine":{"id":0,"name":1,"path":2,"states":3,"machines":4,"entryStateTransitions":5,"exitStateTransitions":6,"anyStateTransitions":7,"defaultStateId":8},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorState":{"id":0,"name":1,"cycleOffset":2,"cycleOffsetParameter":3,"cycleOffsetParameterActive":4,"mirror":5,"mirrorParameter":6,"mirrorParameterActive":7,"motionId":8,"nameHash":9,"fullPathHash":10,"speed":11,"speedParameter":12,"speedParameterActive":13,"tag":14,"tagHash":15,"writeDefaultValues":16,"behaviours":17,"transitions":18},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorStateTransition":{"fullPath":0,"canTransitionToSelf":1,"duration":2,"exitTime":3,"hasExitTime":4,"hasFixedDuration":5,"interruptionSource":6,"offset":7,"orderedInterruption":8,"destinationStateId":9,"isExit":10,"mute":11,"solo":12,"conditions":13},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorCondition":{"mode":0,"parameter":1,"threshold":2},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorTransition":{"destinationStateId":0,"isExit":1,"mute":2,"solo":3,"conditions":4},"Luna.Unity.DTO.UnityEngine.Animation.Mecanim.AnimatorControllerParameter":{"defaultBool":0,"defaultFloat":1,"defaultInt":2,"name":3,"nameHash":4,"type":5},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableDynamicBatching":9,"lightmapEncodingQuality":10,"desiredColorSpace":11,"allTags":12},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixer":{"groups":0,"snapshots":1},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerGroup":{"id":0,"childGroupIds":1,"name":2},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot":{"id":0,"parameters":1},"Luna.Unity.DTO.UnityEngine.Audio.AudioMixerSnapshot+Parameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame":{"weight":0,"vertices":1,"normals":2,"tangents":3}}

Deserializers.requiredComponents = {"63":[64],"65":[64],"66":[64],"67":[64],"68":[64],"69":[64],"70":[52],"71":[17],"72":[73],"74":[73],"75":[73],"76":[73],"77":[73],"78":[73],"79":[73],"80":[81],"82":[81],"83":[81],"84":[81],"85":[81],"86":[81],"87":[81],"88":[81],"89":[81],"90":[81],"91":[81],"92":[81],"93":[81],"94":[17],"95":[3],"96":[97],"98":[97],"16":[15],"99":[100],"101":[102],"103":[102],"104":[15],"105":[15],"19":[16],"23":[22,15],"106":[15],"21":[16],"107":[15],"108":[15],"109":[15],"110":[15],"111":[15],"112":[15],"113":[15],"114":[15],"115":[15],"116":[22,15],"117":[15],"118":[15],"119":[15],"120":[15],"29":[22,15],"121":[15],"122":[58],"123":[58],"59":[58],"124":[58],"125":[17],"126":[17],"127":[102]}

Deserializers.types = ["UnityEngine.Shader","UnityEngine.Texture2D","UnityEngine.Transform","UnityEngine.MeshRenderer","UnityEngine.Material","UnityEngine.MonoBehaviour","QueueTargetControl","UnityEngine.MeshFilter","UnityEngine.Mesh","YarnWoolAnimation","WoolAnimationData","UnityEngine.LineRenderer","RollWoolAnimation","UnityEngine.AudioClip","UnityEngine.Light","UnityEngine.RectTransform","UnityEngine.Canvas","UnityEngine.Camera","UnityEngine.EventSystems.UIBehaviour","UnityEngine.UI.GraphicRaycaster","UnityEngine.CanvasGroup","UnityEngine.UI.CanvasScaler","UnityEngine.CanvasRenderer","UnityEngine.UI.Image","Interactable","UnityEngine.Sprite","UnityEngine.UI.Button","SoundUIElement","PlayNowButtonAnim","UnityEngine.UI.Text","UnityEngine.Font","EndGameUI","UnityEngine.AudioSource","CameraController","ZoomCameraData","UnityEngine.GameObject","GamePlaySystem","BoxChainReaction3D","CubeTargetControl","HandController","SoundManager","UnityEditor.Audio.AudioMixerController","UnityEditor.Audio.AudioMixerGroupController","TargetBoxAnimation","UnityEngine.BoxCollider","UnityEngine.ParticleSystem","UnityEngine.ParticleSystemRenderer","GamePlayMeshController","WoolControl","UnityEngine.Animator","UnityEditor.Animations.AnimatorController","AnimEvent","UnityEngine.SkinnedMeshRenderer","UnityEngine.MeshCollider","UnityEngine.SpriteRenderer","UnityEngine.AudioListener","CameraContainer","HandleTapByPointSelection","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.StandaloneInputModule","UnityEngine.Cubemap","UnityEditor.MonoScript","DG.Tweening.Core.DOTweenSettings","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.FlareLayer","UnityEngine.ConstantForce","UnityEngine.Rigidbody","UnityEngine.Joint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.FixedJoint","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.CompositeCollider2D","UnityEngine.Rigidbody2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","OutlineController","UnityEngine.Renderer","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","Unity.VisualScripting.StateMachine","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.AspectRatioFitter","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutElement","UnityEngine.UI.LayoutGroup","UnityEngine.UI.VerticalLayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.Scrollbar","UnityEngine.UI.ScrollRect","UnityEngine.UI.Slider","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster","Unity.VisualScripting.ScriptMachine"]

Deserializers.unityVersion = "2022.3.45f1";

Deserializers.productName = "WD_PlayableAds";

Deserializers.lunaInitializationTime = "07/03/2025 02:11:06";

Deserializers.lunaDaysRunning = "1.0";

Deserializers.lunaVersion = "6.3.0";

Deserializers.lunaSHA = "7c1090235e749b60367a931fd9d8e53ca14842b9";

Deserializers.creativeName = "WoolDom_3D_1";

Deserializers.lunaAppID = "29703";

Deserializers.projectId = "1d50191528698274c8378a20ad162070";

Deserializers.packagesInfo = "com.unity.ugui: 1.0.0";

Deserializers.externalJsLibraries = "";

Deserializers.androidLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.androidLink?window.$environment.packageConfig.androidLink:'Empty';

Deserializers.iosLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.iosLink?window.$environment.packageConfig.iosLink:'Empty';

Deserializers.base64Enabled = "False";

Deserializers.minifyEnabled = "True";

Deserializers.isForceUncompressed = "False";

Deserializers.isAntiAliasingEnabled = "False";

Deserializers.isRuntimeAnalysisEnabledForCode = "True";

Deserializers.runtimeAnalysisExcludedClassesCount = "1577";

Deserializers.runtimeAnalysisExcludedMethodsCount = "3676";

Deserializers.runtimeAnalysisExcludedModules = "physics2d";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isReferenceAmbientProbeBaked = "False";

Deserializers.isLunaCompilerV2Used = "False";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "WebGL";

Deserializers.applicationIdentifier = "com.DefaultCompany.WD-PlayableAds";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 28;

Deserializers.linearColorSpace = false;

Deserializers.buildID = "f358d00e-4f29-4e40-a8a2-e41f7ed9a287";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[],[],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

